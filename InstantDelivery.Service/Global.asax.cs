﻿using Autofac;
using Autofac.Integration.WebApi;
using InstantDelivery.Domain;
using System.Reflection;
using System.Web.Http;

namespace InstantDelivery.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperConfig.RegisterMappings();

            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.Register(c => new InstantDeliveryContext())
                .AsSelf()
                .InstancePerRequest();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config);

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}