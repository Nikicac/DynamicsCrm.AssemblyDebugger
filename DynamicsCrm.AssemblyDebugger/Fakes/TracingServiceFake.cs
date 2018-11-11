using Microsoft.Xrm.Sdk;
using System;

namespace DynamicsCrm.AssemblyDebugger.Fakes
{
    public class TracingServiceFake : ITracingService
    {
        public ITracingService crmTracingService;
        public void Trace(string format, params object[] args)
        {
            if (crmTracingService == null)
            {
                Console.Write(format, args);
                Console.WriteLine();
            }
            else
                crmTracingService.Trace(format, args);
        }
   }
}
