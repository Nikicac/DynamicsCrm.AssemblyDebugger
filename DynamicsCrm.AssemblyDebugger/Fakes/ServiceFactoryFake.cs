using Microsoft.Xrm.Sdk;
using System;

namespace DynamicsCrm.AssemblyDebugger.Fakes
{
    public class ServiceFactoryFake : IOrganizationServiceFactory
    {
        public IOrganizationService service; 
        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return service;
        }
    }
}
