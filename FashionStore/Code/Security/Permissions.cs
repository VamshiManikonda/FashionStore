using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FashionStore.Security
{
    [Flags]
    public enum UserPermission : long
    {
        [Description("Enabled")]
        Enabled = 1,

        [Description("Admin")]
        Admin = 1 << 1,

        [Description("Allow Reports")]
        AllowReports = 1 << 2

        //[Description("Allow Payment")]
        //AllowPayment = 1 << 3,

        //[Description("Allow Order Management")]
        //AllowOrder = 1 << 4,

       
    }
   
}
