using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Jose;
using RapidPay.Models;

namespace RapidPay.Controllers
{
    public class LoginController : Controller
    {
        [HttpPost]
        public string GetAuth(byte[] credential)
        {
            try
            {
                string cred = Encoding.UTF8.GetString(credential);
                var payload = new Dictionary<string, object>()
            {
                {"user", cred.Split(':')[0] },
                {"password", cred.Split(':')[1] },
                {"exp", Authorize.GetAuthorize.GetExp(DateTime.UtcNow.AddMinutes(20)) }
            };
                return JWT.Encode(payload, Encoding.UTF8.GetBytes("Pay@xlt"), JwsAlgorithm.HS256);
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}