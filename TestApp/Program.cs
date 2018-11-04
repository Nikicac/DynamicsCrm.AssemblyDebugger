using System;
using System.Configuration;
using System.Collections.Generic;
using CrmAssemblyDebugger;
using Microsoft.Xrm.Tooling.Connector;

using Microsoft.Xrm.Sdk;
using CrmAssemblyDebugger.Fakes;
using Microsoft.Crm.Sdk.Messages;
using YourCodeActivities;
using YourPlugins;
using System.Activities;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            CrmServiceClient crmServiceClient = new CrmServiceClient(
                //ConfigurationManager.ConnectionStrings["ConnectionStringName"].ConnectionString 
                "AuthType=Office365; Url=https://yoururl.crm.dynamics.com; UserName=username@yourcomapny.onmicrosoft.com; Password=YourPassword;"
            );
            IOrganizationService os = crmServiceClient;
            WhoAmIRequest req = new WhoAmIRequest();
            WhoAmIResponse resp = (WhoAmIResponse)crmServiceClient.Execute(req);
          
            // run plugin
            SamplePluginExecute(crmServiceClient, resp.UserId);
            //run code activity
            var workflowOutputs = SampleCodeActivityInvoke(crmServiceClient);
        }

        static IDictionary<string, object> SampleCodeActivityInvoke(CrmServiceClient crmServiceClient)
        {
            var myActivity = new TestActivity();
            CodeActivityContextFake context = new CodeActivityContextFake();
            Dictionary<string, object> inputs = new Dictionary<string, object>();
            context = new CodeActivityContextFake();
            context.PrimaryEntityName = "opportunity";
            context.PrimaryEntityId = new Guid("2A2B61B9-1234-ABCD-7891-B3223BFC9624");
            inputs = new Dictionary<string, object>();
            inputs.Add(nameof(myActivity.StringInputOutput), "some string input");
            inputs.Add(nameof(myActivity.IntInputOutput), 858);
            inputs.Add(nameof(myActivity.LookupInputOutput), new EntityReference("account", new Guid()));
            inputs.Add(nameof(myActivity.BoolInputOutput), true);
            inputs.Add(nameof(myActivity.DateInputOutput), new DateTime(2018, 9, 12));
            inputs.Add(nameof(myActivity.DecimalInputOutput), (decimal)10.23);
            inputs.Add(nameof(myActivity.MoneyInputOutput), new Money(10));
 
            return CrmWorkflowInvoker.Invoke(myActivity, crmServiceClient, context, inputs);
        }


        static void SamplePluginExecute(CrmServiceClient crmServiceClient, Guid userId)
        {
            PluginExecutionContextFake pluginContext = new PluginExecutionContextFake();
            pluginContext.PrimaryEntityId = new Guid("12345678-1234-1234-1234-1234567890AB");
            pluginContext.PrimaryEntityName = "account";
            pluginContext.UserId = userId;
            pluginContext.InputParameters = new ParameterCollection();
            // set your target record here
            pluginContext.InputParameters.Add("Target", new Entity("account", new Guid("12345678-1234-1234-1234-1234567890AB")));
            TestPlugin testPlugin = new TestPlugin();
            // if within your plugin you use other stuff of plugincontext, initialize it and put into pluginContext
            // e.g. pluginContext.BusinessUnitId = yourBuId
            PluginExecutor.Execute(testPlugin, crmServiceClient, pluginContext);
        }
    }
}
