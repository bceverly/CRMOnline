﻿using System;
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
    [System.Web.Script.Services.ScriptService]
    public class Security : System.Web.Services.WebService
    {

        [WebMethod]
        public string Login(string user_id, string user_pass)
        {
            return "Hello World";
        }
    }
}
