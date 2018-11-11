﻿using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicsCrm.AssemblyDebugger.Fakes
{
    public class PluginExecutionContextFake : IPluginExecutionContext
    {
        public int Stage { get; set; }

        public IPluginExecutionContext ParentContext { get; set; }

        public int Mode { get; set; }

        public int IsolationMode { get; set; }

        public int Depth { get; set; }

        public string MessageName { get; set; }

        public string PrimaryEntityName { get; set; }

        public Guid? RequestId { get; set; }

        public string SecondaryEntityName { get; set; }

        public ParameterCollection InputParameters { get; set; }

        public ParameterCollection OutputParameters { get; set; }

        public ParameterCollection SharedVariables { get; set; }

        public Guid UserId { get; set; }

        public Guid InitiatingUserId { get; set; }

        public Guid BusinessUnitId { get; set; }

        public Guid OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public Guid PrimaryEntityId { get; set; }

        public EntityImageCollection PreEntityImages { get; set; }

        public EntityImageCollection PostEntityImages { get; set; }

        public EntityReference OwningExtension { get; set; }

        public Guid CorrelationId { get; set; }

        public bool IsExecutingOffline { get; set; }

        public bool IsOfflinePlayback { get; set; }

        public bool IsInTransaction { get; set; }

        public Guid OperationId { get; set; }

        public DateTime OperationCreatedOn { get; set; }
    }
}
