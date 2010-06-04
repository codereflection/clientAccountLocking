using System;
using System.Reflection;
using ClientAccountLockingTest;
using Xunit;

namespace UnitTests
{
    public class StateManagerTests
    {

        public class when_asked_to_store_a_clientaccount
        {
            [Fact]
            public void the_clientaccount_is_stored_and_retrievable()
            {
                StateManager.ResetStateManager();

                var clientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test" };

                Assert.DoesNotThrow(() => StateManager.LockClientAccount(clientAccount));

                Assert.DoesNotThrow(() => StateManager.GetClientAccount(clientAccount.ClientAccountId));
            }


            [Fact]
            public void can_return_that_clientaccount_after_it_has_been_stored()
            {
                StateManager.ResetStateManager();

                var clientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test" };

                Assert.DoesNotThrow(() => StateManager.LockClientAccount(clientAccount));

                var result = StateManager.GetClientAccount(clientAccount.ClientAccountId);

                Assert.Equal(clientAccount.ClientAccountId, result.ClientAccountId);
                Assert.Equal(clientAccount.ClientName, result.ClientName);
            }

            [Fact]
            public void and_that_clientaccount_is_null_an_exception_is_thrown()
            {
                StateManager.ResetStateManager();

                LockedClientAccount clientAccount = null;

                Assert.Throws<ArgumentNullException>(() => StateManager.LockClientAccount(clientAccount));
            }

            [Fact]
            public void the_method_called_should_be_retrievable_from_the_clientaccount_object()
            {
                StateManager.ResetStateManager();

                var methodName = MethodBase.GetCurrentMethod().Name;
                Console.WriteLine("Thread ID: {0}", System.Threading.Thread.CurrentThread.ManagedThreadId);

                var clientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test", MethodCalled = methodName };

                StateManager.LockClientAccount(clientAccount);

                Assert.Equal(methodName, StateManager.GetClientAccount(123).MethodCalled);
            }
        }

        public class when_asked_to_retrieve_a_clientaccount_that_has_not_been_stored
        {
            [Fact]
            public void null_is_returned()
            {
                StateManager.ResetStateManager();

                const int clientAccountId = 123;

                var result = StateManager.GetClientAccount(clientAccountId);

                Assert.Null(result);
            }
        }

        public class when_asked_the_lock_state_of_a_clientaccount_by_clientaccountid
        {
            [Fact]
            public void true_will_be_returned_for_a_client_account_that_is_locked()
            {
                StateManager.ResetStateManager();

                var clientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test", MethodCalled = "Testmethod" };

                StateManager.LockClientAccount(clientAccount);

                Assert.True(StateManager.IsClientAccountIDLocked(123));
            }

            [Fact]
            public void false_will_be_returned_for_a_client_account_that_is_not_locked()
            {
                StateManager.ResetStateManager();

                Assert.False(StateManager.IsClientAccountIDLocked(123));
            }

            [Fact]
            public void false_will_be_returned_when_the_statemanagers_local_state_has_not_be_initialized()
            {
                ResetStateManagersPrivateListToNull();

                Assert.False(StateManager.IsClientAccountIDLocked(123));
            }

            private static void ResetStateManagersPrivateListToNull()
            {
                var fieldInfo = typeof(StateManager).GetField("_clientAccounts");

                fieldInfo.SetValue(null, null);
            }
        }

        public class when_asked_to_unlock_a_clientaccount_that_has_been_locked
        {
            [Fact]
            public void the_clientaccount_will_be_unlocked_and_no_longer_retrievable()
            {
                StateManager.ResetStateManager();

                var clientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test" };

                StateManager.LockClientAccount(clientAccount);

                Assert.NotNull(StateManager.GetClientAccount(clientAccount.ClientAccountId));

                Assert.True(StateManager.UnlockClientAccount(clientAccount.ClientAccountId));

                Assert.Null(StateManager.GetClientAccount(clientAccount.ClientAccountId));
            }
        }

        public class when_asked_to_unlock_a_clientaccount_that_has_not_been_locked
        {
            [Fact]
            public void return_true_without_exception()
            {
                StateManager.ResetStateManager();

                var clientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test" };

                var result = false;

                Assert.DoesNotThrow(() => result = StateManager.UnlockClientAccount(clientAccount.ClientAccountId));

                Assert.True(result);
            }
        }

        public class when_asked_to_local_a_clientaccount_that_has_already_been_locked
        {
            [Fact]
            public void an_exception_will_be_thrown()
            {
                StateManager.ResetStateManager();

                var lockedClientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test" };

                Assert.DoesNotThrow(() => StateManager.LockClientAccount(lockedClientAccount));

                Assert.DoesNotThrow(() => StateManager.GetClientAccount(lockedClientAccount.ClientAccountId));

                var newLockClientAccount = new LockedClientAccount { ClientAccountId = 123, ClientName = "Test" };

                ArgumentException ex = null;

                ex = Assert.Throws<ArgumentException>(() => StateManager.LockClientAccount(newLockClientAccount));

                Assert.True(ex.Message.IndexOf(string.Format("ClientAccountID {0} is already locked", newLockClientAccount.ClientAccountId)) > -1);
            }
        }

    }

    public class ClientAccountLockTests
    {
        public class when_locking_a_clientaccount_with_the_using_statement
        {
            [Fact]
            public void the_client_account_should_be_locked_during_and_unlocked_after()
            {
                StateManager.ResetStateManager();

                using (var lockedClientAccount = new ClientAccountLock(123))
                {
                    Assert.NotNull(lockedClientAccount);

                    Assert.True(StateManager.IsClientAccountIDLocked(lockedClientAccount.ClientAccountId));

                    var newLock = new LockedClientAccount { ClientAccountId = 123, MethodCalled = "Test" };

                    Assert.Throws<ArgumentException>(() => StateManager.LockClientAccount(newLock));
                }

                Assert.False(StateManager.IsClientAccountIDLocked(123));
            }
        }


        public class when_trying_to_lock_a_client_account_that_has_been_locked
        {
            [Fact]
            public void an_exception_should_be_throw()
            {
                StateManager.ResetStateManager();

                var lockedClientAccount = new LockedClientAccount { ClientAccountId = 123, MethodCalled = "test" };

                StateManager.LockClientAccount(lockedClientAccount);

                var newClientAccountLockRequest = new LockedClientAccount { ClientAccountId = 123, MethodCalled = "test" };

                Assert.Throws<ArgumentException>(() => StateManager.LockClientAccount(newClientAccountLockRequest));
            }
        }
    }
}


