using System.Web.Services;

namespace ClientAccountLockingTest
{
    /// <summary>
    /// Summary description for ClientAccountProcessing
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class ClientAccountProcessing : WebService
    {
        [WebMethod]
        public string ProcessClientAccount(int clientAccountId)
        {
            if (StateManager.GetClientAccount(clientAccountId) != null)
            {
                // do something with the locked client accoun here
            }

            return null;
        }
    }
}
