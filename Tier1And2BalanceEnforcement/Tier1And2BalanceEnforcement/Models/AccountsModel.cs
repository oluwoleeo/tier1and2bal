namespace Tier1And2BalanceEnforcement
{
    public class Migrated
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string SourceScheme { get; set; }
        public string TargetScheme { get; set; }
        public string MovedDate { get; set; }
        public string AccountBalance { get; set; }
        public string BranchSOL { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class FailedToMigrate
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string SourceScheme { get; set; }
        public string TargetScheme { get; set; }
        public string AccountBalance { get; set; }
        public string BranchSOL { get; set; }
        public string Reason { get; set; }
    }

    public class ToBeNotified
    {
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public string BranchSOL { get; set; }
        public string SchemeCode { get; set; }
        public string AccountBalance { get; set; }
        public string Percentage { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string RedactedAccountNumber { get; set; }
    }

    public class EmailAlertDetails
    {
        public string CustomerAccountNumber { get; set; }
        public string CustomerAlertEmail { get; set; }
    }

    public class PhoneAlertDetails
    {
        public string CustomerAccountNumber { get; set; }
        public string CustomerAlertPhone { get; set; }
    }
}
