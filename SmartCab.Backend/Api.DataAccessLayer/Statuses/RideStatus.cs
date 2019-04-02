using System;
using System.Collections.Generic;
using System.Text;

namespace Api.DataAccessLayer.Statuses
{
    public enum RideStatus
    {
        LookingForMatch,
        WaitingForAccept,
        Accepted,
        Debited,
        Expired
    }
}
