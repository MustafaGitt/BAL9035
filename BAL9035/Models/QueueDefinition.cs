using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class QueueDefinition
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MaxNumberOfRetries { get; set; }
        public bool AcceptAutomaticallyRetry { get; set; }
        public bool EnforceUniqueReference { get; set; }
        public object SpecificDataJsonSchema { get; set; }
        public object OutputDataJsonSchema { get; set; }
        public object AnalyticsDataJsonSchema { get; set; }
        public DateTime CreationTime { get; set; }
        public object ProcessScheduleId { get; set; }
        public int SlaInMinutes { get; set; }
        public int RiskSlaInMinutes { get; set; }
        public object ReleaseId { get; set; }
        public int Id { get; set; }
    }
}