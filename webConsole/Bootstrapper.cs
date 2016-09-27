using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;
using scriptConsole.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace scriptConsole
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("APP", @"/APP"));
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("Content", @"/Content"));
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("fonts", @"/fonts"));
            this.Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("templates", @"/templates"));
            loadScriptObjects();
        }

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