using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tier1And2BalanceEnforcement
{
    public class TotalTransaction
    {
        [IdentityPrimaryKey]
        public long tid { get; set; }

        public string CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string TellerNo { get; set; }

        public string Amount { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public string BankTransID { get; set; }

        public string BankSortCode { get; set; }

        public System.Nullable<System.DateTime> TransferDate { get; set; }

        public string Status { get; set; }

        public string ErrMsg { get; set; }

        public System.DateTime EntryDate { get; set; }

        public System.Nullable<System.DateTime> SentDate { get; set; }

        public int TryCount { get; set; }
    }
}
