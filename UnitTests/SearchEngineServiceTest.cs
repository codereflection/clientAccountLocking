using System;
using System.Collections.Generic;
using ClientAccountLockingTest;
using Moq.Mvc;
using Xunit;

namespace UnitTests
{
    class SearchEngineServiceTest
    {
        [Fact]
        public void can_add_a_value_to_the_application_object()
        {
            var mockHttpContext = new HttpContextMock();
            const int clientAccountId = 123;
            mockHttpContext.HttpApplicationState.Setup(x => x.Add("test", clientAccountId));
            mockHttpContext.HttpApplicationState.Setup(x => x["test"]).Returns(clientAccountId);

            mockHttpContext.Object.Application.Add("test", clientAccountId);

            Assert.NotNull(mockHttpContext.Object.Application);
            Assert.Equal(123, mockHttpContext.Object.Application["test"]);

            mockHttpContext.HttpApplicationState.VerifyAll();
        }


        [Fact]
        public void can_call_modify_client_web_method()
        {
            var mockHttpContext = new HttpContextMock();

            var clientAccountIdList = new List<int>();

            mockHttpContext.HttpApplicationState.Setup(x => x["ClientAccounts"]).Returns(clientAccountIdList);

            var ses = new SearchEngineService(mockHttpContext.Object);

            Assert.Equal("Client Account Modification Complete", ses.ModifyClientAccount(0));

            mockHttpContext.HttpApplicationState.VerifyAll();
        }


        [Fact]
        public void throws_exception_when_trying_to_modify_an_account_that_is_already_processing()
        {
            var mockHttpContext = new HttpContextMock();
            const int clientAccountId = 123;
            var clientAccountIdList = new List<int>() { clientAccountId };

            mockHttpContext.HttpApplicationState.Setup(x => x["ClientAccounts"]).Returns((object x) => clientAccountIdList).AtMost(2);

            var ses = new SearchEngineService(mockHttpContext.Object);

            Assert.Throws<ApplicationException>(() => ses.ModifyClientAccount(clientAccountId));

            mockHttpContext.HttpApplicationState.VerifyAll();
        }
    }
}