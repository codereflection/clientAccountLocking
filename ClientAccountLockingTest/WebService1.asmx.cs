using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ClientAccountLockingTest
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MyWebService : WebService
    {
        readonly HttpContextBase _httpContext;

        public MyWebService()
        {
            _httpContext = new HttpContextWrapper(HttpContext.Current);
        }

        public MyWebService(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }


        private bool IsClientAccountLocked(int clientAccountId)
        {
            if (_httpContext.Application["ClientAccounts"] == null)
            {
                return false;
            }
            var lockList = (List<int>)_httpContext.Application["ClientAccounts"];

            return lockList.Contains(clientAccountId);
        }


        public bool LockClientAccount(int clientAccountId)
        {
            if (_httpContext.Application["ClientAccounts"] == null)
            {
                _httpContext.Application.Add("ClientAccounts", new List<int>());
            }
            var lockList = (List<int>)_httpContext.Application["ClientAccounts"];

            lockList.Add(clientAccountId);

            return true;
        }


        private void UnLockClientAccount(int clientAccountId)
        {
            var lockList = (List<int>)_httpContext.Application["ClientAccounts"];

            lockList.Remove(clientAccountId);
        }

        [WebMethod]
        public string ModifyClientAccount(int clientAccountId)
        {
            if (IsClientAccountLocked(clientAccountId) == true)
            {
                throw new ApplicationException(String.Format("ClientAccountID {0} is currently in use", clientAccountId));
            }

            LockClientAccount(clientAccountId);

            System.Threading.Thread.Sleep(10000);

            UnLockClientAccount(clientAccountId);

            return "Client Account Modification Complete";
        }

    }
}
