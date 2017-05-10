using SofCoAr.Resolver;
using Microsoft.Practices.Unity;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Swashbuckle.Application;
using SofCoAr.Repositories;
using SofCoAr.Models;
using System.Web.Http.Cors;

namespace SofCoAr
{
    public class Startup
    {
        // This code configures Web API. The Startup class is specified as a type
        // parameter in the WebApp.Start method.
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //Cors
            var cors = new EnableCorsAttribute("*", "*", "*", "*");
            config.EnableCors(cors);

            //Swashbuckle
            config
                .EnableSwagger(c => c.SingleApiVersion("v1", "Self Hosted WebApi"))
                .EnableSwaggerUi();

            //Unity
            var container = new UnityContainer();
            
            
            container.RegisterType<IBillingMilestoneRepo, BillingMilestoneRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<ICurrencySignRepo, CurrencySignRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerRepo, CustomerRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<ICustomerServiceRepo, CustomerServiceRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<IDocumentTypeRepo, DocumentTypeRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<IPaymentMethodRepo, PaymentMethodRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<IProjectRepo, ProjectRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<IProvinceRepo, ProvinceRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<ISolFacHistRepo, SolFacHistRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<ISolFacStateRepo, SolFacStateRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<IStatusRepo, StatusRepo>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepo, UserRepo>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);

            //Json
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter
                .SerializerSettings
                .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            appBuilder.UseWebApi(config);
        }
    }
}
