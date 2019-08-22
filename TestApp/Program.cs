using System;
using System.Configuration;
using System.Collections.Generic;
using DynamicsCrm.AssemblyDebugger;
using DynamicsCrm.AssemblyDebugger.Fakes;
using Microsoft.Xrm.Tooling.Connector;

using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using YourCodeActivities;
using YourPlugins;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var cs = "AuthType=Office365; Url=https://bestbest4.crm4.dynamics.com; UserName=nikica@bestbest4.onmicrosoft.com; Password=SomePass123;";
            //var cs = "AuthType=Office365; Url=https://yoururl.crm.dynamics.com; UserName=username@yourcomapny.onmicrosoft.com; Password=YourPassword;";
            var client = new CrmServiceClient(cs);
            var req = new WhoAmIRequest();
            var resp = (WhoAmIResponse)client.Execute(req);
            // run plugin
            SamplePluginExecute(client, resp.UserId);
            //run code activity
            var workflowOutputs = SampleCodeActivityExecute(client, resp.UserId);
            // sample plugin secure
            SamplePluginNoTrustExecute(client, resp.UserId);
        }

        static IDictionary<string, object> SampleCodeActivityExecute(CrmServiceClient client, Guid userId)
        {
            var myActivity = new TestActivity();
            var context = new CodeActivityContextFake();
            context = new CodeActivityContextFake();
            context.InitiatingUserId = userId;
            context.UserId = userId;
            context.PrimaryEntityName = "opportunity";
            context.PrimaryEntityId = new Guid("2A2B61B9-1234-ABCD-7891-B3223BFC9624");
            var inputs = new Dictionary<string, object>();
            inputs.Add(nameof(myActivity.StringInputOutput), "some string input");
            inputs.Add(nameof(myActivity.IntInputOutput), 858);
            inputs.Add(nameof(myActivity.LookupInputOutput), new EntityReference("account", new Guid()));
            inputs.Add(nameof(myActivity.BoolInputOutput), true);
            inputs.Add(nameof(myActivity.DateInputOutput), new DateTime(2018, 9, 12));
            inputs.Add(nameof(myActivity.DecimalInputOutput), (decimal)10.23);
            inputs.Add(nameof(myActivity.MoneyInputOutput), new Money(10));
 
            return CodeActivityExecutor.Execute(myActivity, client, context, inputs);
        }


        static void SamplePluginExecute(CrmServiceClient client, Guid userId)
        {
            var pluginContext = new PluginExecutionContextFake();
            // if within your plugin you use other stuff of plugincontext, initialize it and put appropriate
            // e.g. pluginContext.BusinessUnitId = yourBuId
            //pluginContext.PreEntityImages.Add("name", someEntity);
            pluginContext.PrimaryEntityId = new Guid("12345678-1234-1234-1234-1234567890AB");
            pluginContext.PrimaryEntityName = "account";
            pluginContext.UserId = userId;
            pluginContext.InitiatingUserId = userId;
             // set your target record lie this
            var e = new Entity("account", new Guid("12345678-1234-1234-1234-1234567890AB"));
            e.Attributes["name"] = "someName";
            pluginContext.InputParameters.Add("Target", e);
            var testPlugin = new TestPlugin();
            // sample for plugin constructors requiering secure and unsecure config:
            // TestPlugin testPlugin = new TestPlugin(yourSecureConfig, yourUnsecureConfig); //- both parameters can be null or empty string

            PluginExecutor.Execute(testPlugin, client, pluginContext);
        }


        // use this sample if you don't trust Nuget packages and don't want to give client to Nuget Packages
        static void SamplePluginNoTrustExecute(CrmServiceClient client, Guid userId)
        {
            var pluginContext = new PluginExecutionContextFake();
            pluginContext.PrimaryEntityId = new Guid("12345678-1234-1234-1234-1234567890AB");
            pluginContext.PrimaryEntityName = "account";
            pluginContext.UserId = userId;
            pluginContext.InputParameters.Add("Target", new Entity("account", new Guid("12345678-1234-1234-1234-1234567890AB")));

            // workaround - client goes directly to your plugin. PluginExecutor can't touch it. 
            var testPlugin = new TestPluginNoTrust();
            var serviceFactory = new ServiceFactoryFake();
            serviceFactory.service = client.OrganizationServiceProxy;
            testPlugin.SetOrganizationServiceFactory( serviceFactory);
            // end of workaround

            PluginExecutor.Execute(testPlugin, null, pluginContext);
        }
    }
}
