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

        [WebMethod]
        public string ModifyClientAccount(int clientAccountId)
        {
            using (var clientAccountLock = new ClientAccountLock(clientAccountId))
            {
                System.Threading.Thread.Sleep(10000);
            }

            return "Client Account Modification Complete";
        }

    }
}
