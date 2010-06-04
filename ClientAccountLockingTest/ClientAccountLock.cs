using System;

namespace ClientAccountLockingTest
{
    public class ClientAccountLock : LockedClientAccount, IDisposable
    {

        public ClientAccountLock()
        {
            throw new NotImplementedException("Not implemented by design. Parameterless constructor only exists for seralization compatibility.");
        }

        /// <summary>
        /// Initializes a new instance of the ClientAccountLock class.
        /// </summary>
        public ClientAccountLock(int id)
        {
            var stackTrace = new System.Diagnostics.StackTrace();

            var callingMethod = stackTrace.GetFrame(1).GetMethod().Name;

            base.ClientAccountId = id;
            base.MethodCalled = callingMethod;

            StateManager.LockClientAccount(this);
        }

        public void Dispose()
        {
            StateManager.UnlockClientAccount(base.ClientAccountId);
        }
    }
}
