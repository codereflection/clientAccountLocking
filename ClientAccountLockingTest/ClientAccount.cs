using System;

namespace ClientAccountLockingTest
{
    public class LockedClientAccount
    {
        public int ClientAccountId { get; set; }
        public string ClientName { get; set; }
        public DateTime ProcessingStarted { get; set; }
        public string MethodCalled { get; set; }
    }
}
