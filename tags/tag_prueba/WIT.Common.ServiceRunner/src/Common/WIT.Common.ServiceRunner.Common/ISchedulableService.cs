
using System;
namespace WIT.Common.ServiceRunner
{
    public interface ISchedulableService
    {
        void Execute(DateTime? lastExecution);
    }
}
