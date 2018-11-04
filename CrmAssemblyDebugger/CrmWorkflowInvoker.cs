using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Tooling.Connector;
using System.Activities;
using System.Collections.Generic;
using CrmAssemblyDebugger.Fakes;

namespace CrmAssemblyDebugger
{
    public static class CrmWorkflowInvoker
    {

        public static IDictionary<string, object> Invoke(CodeActivity workflow
            , CrmServiceClient service, CodeActivityContextFake context, Dictionary<string, object> inputs )
        {

            WorkflowInvoker invoker = new WorkflowInvoker(workflow);
            invoker.Extensions.Add<IOrganizationService>(() => service);
            invoker.Extensions.Add<IWorkflowContext>(() => context);
            invoker.Extensions.Add<ITracingService>(() => new TracingServiceFake());
            
            ServiceFactoryFake serviceFactory = new ServiceFactoryFake();
            serviceFactory.service = (IOrganizationService)service.OrganizationServiceProxy;
            invoker.Extensions.Add<IOrganizationServiceFactory>(() => serviceFactory);
        
            return invoker.Invoke(inputs);
        }
    }
}
