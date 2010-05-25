using System;
using System.Collections.Generic;

namespace ClientAccountLockingTest
{
    public static class StateManager
    {
        public static Dictionary<int, LockedClientAccount> _clientAccounts;

        public static void LockClientAccount(LockedClientAccount clientAccount)
        {
            if (clientAccount == null)
                throw new ArgumentNullException("clientAccount", "clientAccount cannot be null");

            if (_clientAccounts == null)
                _clientAccounts = new Dictionary<int, LockedClientAccount>();

            if (_clientAccounts.ContainsKey(clientAccount.ClientAccountId))
                throw new ArgumentException(string.Format("ClientAccountID {0} is already locked", clientAccount.ClientAccountId), "clientAccount");

            _clientAccounts.Add(clientAccount.ClientAccountId, clientAccount);
        }

        public static bool UnlockClientAccount(int clientAccountId)
        {
            if (_clientAccounts == null || _clientAccounts.Count == 0)
                return true;

            return _clientAccounts.Remove(clientAccountId);
        }

        public static LockedClientAccount GetClientAccount(int clientAccountId)
        {
            if (_clientAccounts == null)
                return null;

            if (_clientAccounts.ContainsKey(clientAccountId))
                return _clientAccounts[clientAccountId];
            
            return null;
        }

        public static void ResetStateManager()
        {
            _clientAccounts = new Dictionary<int, LockedClientAccount>();
        }

        public static bool IsClientAccountIDLocked(int clientAccountId)
        {
            if (_clientAccounts == null)
                return false;

            return _clientAccounts.ContainsKey(clientAccountId);
            
        }
    }
}