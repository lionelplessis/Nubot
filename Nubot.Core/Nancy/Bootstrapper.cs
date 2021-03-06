namespace Nubot.Core.Nancy
{
    using System.Linq;
    using Abstractions;
    using global::Nancy;
    using global::Nancy.Conventions;
    using global::Nancy.TinyIoc;

    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private readonly IRobot _robot;

        public Bootstrapper(IRobot robot)
        {
            _robot = robot;
        }

        /// <summary>
        /// Configures the container using AutoRegister followed by registration
        ///             of default INancyModuleCatalog and IRouteResolver.
        /// </summary>
        /// <param name="container">Container instance</param>
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            container.Register(_robot);
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            var httpPluginsStaticPaths = _robot.RobotPlugins.OfType<HttpPluginBase>().SelectMany(httpPlugin => httpPlugin.StaticPaths);

            foreach (var staticPath in httpPluginsStaticPaths)
            {
                nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory(staticPath.Item1, staticPath.Item2));
            }

            nancyConventions.ViewLocationConventions.Add((viewName, model, viewLocationContext) => string.Concat("plugins", viewLocationContext.ModulePath, "/Views/", viewName));
        }
    }
}