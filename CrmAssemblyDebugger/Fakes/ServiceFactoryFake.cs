using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrmAssemblyDebugger.Fakes
{
    class ServiceFactoryFake : IOrganizationServiceFactory
    {
        public IOrganizationService service; 
        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return service;
        }
    }
}
