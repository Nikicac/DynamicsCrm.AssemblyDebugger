﻿using Microsoft.Xrm.Sdk;
using System;


namespace DynamicsCrm.AssemblyDebugger.Fakes
{
    public class ServiceProviderFake : IServiceProvider
    {

        public PluginExecutionContextFake MyPluginExecutionContext;
        public TracingServiceFake MyTracingServiceFake;
        public ServiceEndpointNotificationServiceFake MyServiceEndpointNotificationServiceFake;
        public OrganizationServiceFactoryFake MyOrganizationServiceFactory;

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IPluginExecutionContext))
                return MyPluginExecutionContext;

            if (serviceType == typeof(ITracingService))
                return MyTracingServiceFake;

            if (serviceType == typeof(IOrganizationServiceFactory))
                return MyOrganizationServiceFactory;

            if (serviceType == typeof(IServiceEndpointNotificationService))
                return MyServiceEndpointNotificationServiceFake;

            return null;
        }
    }
}
