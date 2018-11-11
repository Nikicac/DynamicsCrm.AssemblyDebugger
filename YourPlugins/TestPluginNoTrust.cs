using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace YourPlugins
{
    public class TestPluginNoTrust : IPlugin
    {
        // test interface 
        private IOrganizationServiceFactory ServiceFactoryLocal;
        public void SetOrganizationServiceFactory(IOrganizationServiceFactory factory)
        {
            ServiceFactoryLocal = factory;
        }


        public void Execute(IServiceProvider serviceProvider)

        {
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));

            // notificationService.execute is not supported
            var notificationService = (IServiceEndpointNotificationService)serviceProvider.GetService(typeof(IServiceEndpointNotificationService));

            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            /// do your plugin stuff here: like...
            if (context.InputParameters.Contains("Target") &&
                  context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];

                 if (entity.LogicalName != "account")
                    return;
                try
                {
                    if (ServiceFactoryLocal == null)
                        ServiceFactoryLocal = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                    var service = ServiceFactoryLocal.CreateOrganizationService(context.UserId);
                    var account = service.Retrieve("account", new Guid("12345678-1234-1234-1234-1234567890AB"), new ColumnSet(new string[] { "name", "accountid" }));
                    tracingService.Trace("Account received. Id: {account.Id}");
                }
                catch
                {
                    // handle your exception here
                }
            }
        }
    }
}
