using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace Tier1And2BalanceEnforcement
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class IdentityPrimaryKeyAttribute : Attribute
    {

    }
}

