
namespace WIT.Common.ServiceRunner
{
    public class SchedulableServiceInfo
    {
        public string Name {get;set;}

        public string Assembly {get;set;}

        public System.Nullable<System.DateTime> LastExecution { get; set; }

        /// <summary>
        /// The interval in minutes to wait before each service execution
        /// </summary>
        public System.Nullable<long> ExecutionInterval { get; set; }
    }
}
