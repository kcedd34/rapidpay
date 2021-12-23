using Jose;
using RapidPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace RapidPay.Authorize
{
    public class GetAuthorize : ActionFilterAttribute, IAuthenticationFilter
    {
        public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public void OnAuthentication(AuthenticationContext filterContext)
        {

        }
        
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            Authorize.Auth jwtAuth = Newtonsoft.Json.JsonConvert.DeserializeObject<Authorize.Auth>(JWT.Decode(HttpContext.Current.Request.Headers.GetValues("Authorization").FirstOrDefault().Replace("Bearer ", ""), Encoding.UTF8.GetBytes("Pay@xlt")));
            using (RapidPayEntities inv = new RapidPayEntities())
            {
                try
                {
                    Authorize.Auth jwt = Newtonsoft.Json.JsonConvert.DeserializeObject<Authorize.Auth>(JWT.Decode(HttpContext.Current.Request.Headers.GetValues("Authorization").FirstOrDefault().Replace("Bearer ", ""), Encoding.UTF8.GetBytes("Pay@xlt")));

                    if (inv.user.Where(x => x.email == jwtAuth.user && x.password == jwtAuth.password).Count() > 0 && Convert.ToInt64(jwtAuth.exp) > GetExp(DateTime.UtcNow))
                    {
                        
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                    }
                }
                catch (System.Exception)
                {
                    filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary(new { controller = "Error", action = "AccessDenied" }));
                }
            }
        }

        public static long GetExp(DateTime utcNow)
        {
            var tokenValidFor = TimeSpan.FromDays(200);
            var expiry = utcNow.Add(tokenValidFor);
            return GetEpochDateTimeAsInt(expiry);
        }

        public static long GetEpochDateTimeAsInt(DateTime datetime)
        {
            DateTime dateTime = datetime;
            if (datetime.Kind != DateTimeKind.Utc)
                dateTime = datetime.ToUniversalTime();
            if (dateTime.ToUniversalTime() <= UnixEpoch)
                return 0;
            return (long)(dateTime - UnixEpoch).TotalSeconds;
        }
    }
}