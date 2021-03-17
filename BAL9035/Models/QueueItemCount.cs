using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BAL9035.Models
{
    public class QueueItemCount
    {
        public int QueueDefinitionId { get; set; }
        public object OutputData { get; set; }
        public object AnalyticsData { get; set; }
        public string Status { get; set; }
        public string ReviewStatus { get; set; }
        public object ReviewerUserId { get; set; }
        public string Key { get; set; }
        public object Reference { get; set; }
        public object ProcessingExceptionType { get; set; }
        public object DueDate { get; set; }
        public object RiskSlaDate { get; set; }
        public string Priority { get; set; }
        public object DeferDate { get; set; }
        public object StartProcessing { get; set; }
        public object EndProcessing { get; set; }
        public int SecondsInPreviousAttempts { get; set; }
        public object AncestorId { get; set; }
        public int RetryNumber { get; set; }
        public string SpecificData { get; set; }
        public object Progress { get; set; }
        public string RowVersion { get; set; }
        public int Id { get; set; }
        public object ProcessingException { get; set; }
        public SpecificContent SpecificContent { get; set; }
        public object Output { get; set; }
        public object Analytics { get; set; }
    }
}