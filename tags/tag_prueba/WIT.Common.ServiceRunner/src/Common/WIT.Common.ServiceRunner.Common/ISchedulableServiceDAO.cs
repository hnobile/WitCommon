using System.Collections.Generic;

namespace WIT.Common.ServiceRunner.DAO
{
    public interface ISchedulableServiceDAO
    {
        List<SchedulableServiceInfo> GetAll();
        
        void SaveServiceState(SchedulableServiceInfo serviceInfo);
    }
}
