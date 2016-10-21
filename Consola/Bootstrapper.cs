using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Responses;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Consola.Library;

namespace Consola
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            var assembly = GetType().Assembly;
            ResourceViewLocationProvider
                .RootNamespaces.Add(assembly, "Consola.Views");
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

            pipelines.AfterRequest.AddItemToEndOfPipeline((ctx) =>
            {
                ctx.Response.WithHeader("Access-Control-Allow-Origin", "http://*")
                            .WithHeader("Access-Control-Allow-Methods", "GET")
                            .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            });
        }

        private static Func<NancyContext, string, Response> embeddedContentHandler = (ctx, rootPath) => {
            var pathDir = Path.GetDirectoryName(ctx.Request.Url.Path) ?? string.Empty;
            const string resourcePath = @"\APP";
            if (!pathDir.StartsWith(resourcePath, System.StringComparison.OrdinalIgnoreCase))
                return null;
            var builder = new StringBuilder(ctx.Request.Url.Path.Substring(1, ctx.Request.Url.Path.Length - 1));
            builder = builder.Replace('/', '.', 0, ctx.Request.Url.Path.LastIndexOf('.') + 1);
            var path = builder.ToString();
            var thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string[] resources = thisExe.GetManifestResourceNames();
            string compareName = "Consola." + path;
            if (resources.Contains(compareName, StringComparer.CurrentCultureIgnoreCase))
            {
                Response response = new EmbeddedFileResponse(thisExe,
                                                "Consola." + pathDir.Substring(1, pathDir.Length - 1).Replace("\\", "."),
                                                Path.GetFileName(ctx.Request.Url.Path));
                return response.WithHeader("Access-Control-Allow-Origin", "*")
                               .WithHeader("Access-Control-Allow-Methods", "GET")
                               .WithHeader("Access-Control-Allow-Headers", "Accept, Origin, Content-type");
            }

            return null;
        };

        private void loadScriptObjects()
        {
            IEnumerable<Type> scriptables = getQualifiedTypes(typeof(Scriptable));

            foreach(Type scriptable in scriptables)
            {
                ConstructorInfo CI = scriptable.GetConstructor(new Type[1] { typeof(ScriptSession) });
                if (CI != null)
                    ScriptSession.scriptObjects.Add(scriptable);
            }
        }

        internal static IEnumerable<Type> getQualifiedTypes(Type T)
        {
            Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            return loadedAssemblies.Where((assembly) => {
                try
                {
                    if (!assembly.IsDynamic)
                    {
                        var i = assembly.ExportedTypes;
                        return true;
                    }
                    return false;
                }
                catch
                {
                    return false;
                }
            }).SelectMany((assembly) =>
            {
                return assembly.ExportedTypes.Where((t) => {
                    return T.IsAssignableFrom(t);
                });
            });
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}