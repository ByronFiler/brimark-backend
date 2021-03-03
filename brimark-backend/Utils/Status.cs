using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace brimark_backend.Utils
{
    public enum Status
    {

        // General ()
        OK,
        DATABASE_FAILURE,

        // Activate Account
        ALREADY_ACTIVATED,
        NO_MATCHING_ACCOUNT

    }
}
