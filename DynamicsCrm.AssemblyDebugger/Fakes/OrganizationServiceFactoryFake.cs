using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicsCrm.AssemblyDebugger.Fakes
{
    public class OrganizationServiceFactoryFake : IOrganizationServiceFactory
    {
        public IOrganizationService Service;
        public IOrganizationService CreateOrganizationService(Guid? userId)
        {
            return Service;
        }
    }
}
