using System;
using System.Collections.Generic;
using System.Text;
using CrmAssemblyDebugger.Fakes;
using Microsoft.Xrm.Sdk;

namespace CrmAssemblyDebugger
{
    public class PluginExecutor
    {
        public static void Execute(IPlugin plugin
            , IOrganizationService service, PluginExecutionContextFake context
            , ITracingService tracingService = null
            , IServiceEndpointNotificationService notificationService = null
            , IOrganizationServiceFactory factory = null)
        {
            ServiceProviderFake serviceProvider = new ServiceProviderFake();
            serviceProvider.MyPluginExecutionContext = context;

           serviceProvider.MyTracingServiceFake = new TracingServiceFake();
            if(tracingService != null)
                serviceProvider.MyTracingServiceFake.crmTracingService = tracingService;

            serviceProvider.MyOrganizationServiceFactory = new OrganizationServiceFactoryFake();
            serviceProvider.MyOrganizationServiceFactory.Service = service;

            serviceProvider.MyServiceEndpointNotificationServiceFake = new ServiceEndpointNotificationServiceFake();
            if (notificationService != null)
                serviceProvider.MyServiceEndpointNotificationServiceFake.crmServiceEndpointNotificationService 
                    = notificationService;

             plugin.Execute(serviceProvider);
        }
    }
}
