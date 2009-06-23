using System;
namespace WIT.Common.ServiceRunner.Common
{
    [Serializable]
    public abstract class SchedulableServiceBase : MarshalByRefObject, ISchedulableService
    {
        #region ISchedulableService Members

        public abstract void Execute();

        #endregion
    }
}
