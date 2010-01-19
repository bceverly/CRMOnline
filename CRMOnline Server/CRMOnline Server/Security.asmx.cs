using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace CRMOnline_Server
{
    /// <summary>
    /// Summary description for Security
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Security : System.Web.Services.WebService
    {

        [WebMethod]
        public string Login(string user_id, string user_pass)
        {
            using (CRMOnlineDataContext dc = new CRMOnlineDataContext(string.Format("Data Source={0};Initial Catalog=CRMOnline;User ID={1};Password={2}", "localhost", "sa", "abc123%")))
            {
                var users = from u in dc.user_accesses
                            where u.user_id == user_id && u.user_pass == user_pass
                            select u;

                List<user_access> retVal = users.ToList();
                if (retVal.Count > 0)
                {
                    // Successful login.  Create a UUID and update the last_accessed_datetime column
                    Guid retUUID = Guid.NewGuid();
                    user_access theRow = retVal.First();
                    theRow.session_guid = retUUID;
                    theRow.last_access_datetime = DateTime.Now;
                    dc.SubmitChanges();

                    // Return the UUID
                    return retUUID.ToString();
                }
            }
            return "";
        }
    }
}
