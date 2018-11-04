namespace YourCodeActivities
{
    using System;
    using System.Activities;
    using System.ServiceModel;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    public sealed class TestActivity : CodeActivity
    {
        [Input("String Input")]
        public InOutArgument<string> StringInputOutput  { get; set; }

        [Input("Lookup Input")]
        [ReferenceTarget("account")]
        public InOutArgument<EntityReference> LookupInputOutput  { get; set; }

        [Input("Bool input")]
        public InOutArgument<bool> BoolInputOutput  { get; set; }

        [Input("Int input")]
        public InOutArgument<int> IntInputOutput  { get; set; }

        [Input("Decimal input")]
        public InOutArgument<decimal> DecimalInputOutput  { get; set; }


        [Input("Money input")]
        public InOutArgument<Money> MoneyInputOutput  { get; set; }

        [Input("DateTime input")]
        public InOutArgument<DateTime> DateInputOutput  { get; set; }

        protected override void Execute(CodeActivityContext executionContext)
        {
            ITracingService tracingService = executionContext.GetExtension<ITracingService>();

            if (tracingService == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve tracing service.");
            }

            tracingService.Trace("Entered TestActivity.Execute(), Activity Instance Id: {0}, Workflow Instance Id: {1}",
                executionContext.ActivityInstanceId,
                executionContext.WorkflowInstanceId);

            // Create the context
            IWorkflowContext context = executionContext.GetExtension<IWorkflowContext>();

            if (context == null)
            {
                throw new InvalidPluginExecutionException("Failed to retrieve workflow context.");
            }

            tracingService.Trace("TestActivity.Execute(), Correlation Id: {0}, Initiating User: {1}",
                context.CorrelationId,
                context.InitiatingUserId);

            IOrganizationServiceFactory serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            IOrganizationService service = serviceFactory.CreateOrganizationService(context.UserId);

            try
            {
                Guid primaryEntityId = context.PrimaryEntityId;
                string str = StringInputOutput.Get<string>(executionContext);
                int intVar = IntInputOutput.Get<int>(executionContext);
                decimal decVar = DecimalInputOutput.Get<decimal>(executionContext);
                EntityReference erVar = LookupInputOutput.Get<EntityReference>(executionContext);
                bool boolVar = BoolInputOutput.Get<bool>(executionContext);
                Money moneyVar = MoneyInputOutput.Get<Money>(executionContext);
                //
                // do your activity stuff here
                //
                
                // now prepare outputs
                StringInputOutput.Set(executionContext, "returned string");
                IntInputOutput.Set(executionContext, 3);
                DecimalInputOutput.Set(executionContext, (decimal)2.11);
                LookupInputOutput.Set(executionContext, new EntityReference("account", new Guid("12345a12-1234-1234-1234-1234567890ab")));
                BoolInputOutput.Set(executionContext, true);
                MoneyInputOutput.Set(executionContext, new Money((decimal)2.45));


            }
            catch (FaultException<OrganizationServiceFault> e)
            {
                tracingService.Trace("Exception: {0}", e.ToString());
                // Handle the exception.
                throw;
            }

            tracingService.Trace("Exiting TestActivity.Execute(), Correlation Id: {0}", context.CorrelationId);
        }
    }
}