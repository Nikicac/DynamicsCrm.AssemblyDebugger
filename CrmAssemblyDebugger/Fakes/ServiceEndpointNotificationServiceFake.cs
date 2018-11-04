using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmAssemblyDebugger.Fakes
{
    public class ServiceEndpointNotificationServiceFake : IServiceEndpointNotificationService
    {
        public IServiceEndpointNotificationService crmServiceEndpointNotificationService;
        public string Execute(EntityReference serviceEndpoint, IExecutionContext context)
        {
            if (crmServiceEndpointNotificationService != null)
                return crmServiceEndpointNotificationService.Execute(serviceEndpoint, context);
            else
                throw new NotImplementedException();
        }
    }
}
