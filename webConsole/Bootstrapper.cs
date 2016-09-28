using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Responses;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Microsoft.Owin.Host;
using scriptConsole.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace scriptConsole
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            var assembly = GetType().Assembly;
            ResourceViewLocationProvider
                .RootNamespaces.Add(assembly, "scriptConsole.Views");
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(OnConfigurationBuilder);
            }
        }

        void OnConfigurationBuilder(NancyInternalConfiguration x)
        {
            x.ViewLocationProvider = typeof(ResourceViewLocationProvider);
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            this.Conventions.StaticContentsConventions.Add(embeddedContentHandler);
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", @"/Content"));
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("fonts", @"/fonts"));
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("templates", @"/templates"));
            loadScriptObjects();
            
        }

        public static Func<NancyContext, string, Response> embeddedContentHandler = (ctx, rootPath) => {
            var pathDir = Path.GetDirectoryName(ctx.Request.Url.Path) ?? string.Empty;
            const string resourcePath = @"\APP";
            if (!pathDir.StartsWith(resourcePath, System.StringComparison.OrdinalIgnoreCase))
                return null;
            var builder = new StringBuilder(ctx.Request.Url.Path.Substring(1, ctx.Request.Url.Path.Length - 1));
            builder = builder.Replace('/', '.', 0, ctx.Request.Url.Path.LastIndexOf('.') + 1);
            var path = builder.ToString();
            var thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string[] resources = thisExe.GetManifestResourceNames();
            string compareName = "scriptConsole." + path;
            if (resources.Contains(compareName, StringComparer.CurrentCultureIgnoreCase))
                return new EmbeddedFileResponse(thisExe,
                                                "scriptConsole." + pathDir.Substring(1, pathDir.Length - 1).Replace("\\", "."),
                                                Path.GetFileName(ctx.Request.Url.Path));

            return null;
        };

        private void loadScriptObjects()
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> scriptables = loadedAssemblies.Where((assembly) => {
                try
                {
                    var i = assembly.ExportedTypes;
                    return !assembly.IsDynamic;
                }
                catch (Exception e)
                {
                    return false;
                }
                }).SelectMany((assembly) =>
            {
                    return assembly.ExportedTypes.Where((t) => t is IScriptable);
            });

            foreach(Type scriptable in scriptables)
            {
                ConstructorInfo CI = scriptable.GetConstructor(new Type[1] { scriptable });
                IScriptable obj = (IScriptable)CI.Invoke(new dynamic[0]);
                ScriptSession.scriptObjects.Add(obj);
            }
        }
    }
}