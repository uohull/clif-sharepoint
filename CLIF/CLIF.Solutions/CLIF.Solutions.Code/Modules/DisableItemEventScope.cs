using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CLIF.Solutions.Code
{
    public class DisabledItemEventsScope : SPItemEventReceiver, IDisposable
    {
        public DisabledItemEventsScope()
        {
            base.DisableEventFiring();
        }
        #region IDisposable Members

        public void Dispose()
        {
            base.EnableEventFiring();
        }

        #endregion
    }
}
