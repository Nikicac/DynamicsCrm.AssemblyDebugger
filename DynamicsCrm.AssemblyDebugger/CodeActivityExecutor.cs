using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Tooling.Connector;
using System.Activities;
using System.Collections.Generic;
using DynamicsCrm.AssemblyDebugger.Fakes;

namespace DynamicsCrm.AssemblyDebugger
{
    public static class CodeActivityExecutor
    {

        public static IDictionary<string, object> Execute(
            CodeActivity workflow
            , CrmServiceClient service, CodeActivityContextFake context, Dictionary<string, object> inputs
            , ITracingService tracingService = null
            , IServiceEndpointNotificationService notificationService = null
        )
        {

            WorkflowInvoker invoker = new WorkflowInvoker(workflow);
            invoker.Extensions.Add<IOrganizationService>(() => service);
            invoker.Extensions.Add<IWorkflowContext>(() => context);
            if(tracingService != null)
                invoker.Extensions.Add<ITracingService>(() => tracingService);
            else
                invoker.Extensions.Add<ITracingService>(() => new TracingServiceFake());
            
            ServiceFactoryFake serviceFactory = new ServiceFactoryFake();
            serviceFactory.service = (IOrganizationService)service?.OrganizationServiceProxy;
            invoker.Extensions.Add<IOrganizationServiceFactory>(() => serviceFactory);

            if (notificationService != null)
                invoker.Extensions.Add<IServiceEndpointNotificationService>(() => notificationService);
            else
                invoker.Extensions.Add<IServiceEndpointNotificationService>(() => new ServiceEndpointNotificationServiceFake());

            return invoker.Invoke(inputs);
        }
    }
}
