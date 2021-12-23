using RapidPay.Authorize;
using RapidPay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RapidPay.Controllers
{
    public class PaymentController : Controller
    {
        private RapidPayEntities db = new RapidPayEntities();
        private static System.Timers.Timer timer = new System.Timers.Timer(3600000);
        private static double tax = 0;
        private static Random rand = new Random();

        public static void CardFees()
        {
            tax = tax * (rand.NextDouble() * 2);
            timer.Elapsed += async (sender, arguments) => await timer_Elapsed(sender, arguments);
            timer.Start();
        }
        async static Task timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tax = tax * (rand.NextDouble() * 2);
        }

        [HttpPost]
        [GetAuthorize]
        public ActionResult Pay(string cardNumber, decimal value)
        {
            using (RapidPayEntities db = new RapidPayEntities())
            {
                var result = db.card.SingleOrDefault(b => b.card1 == cardNumber);
                if (result != null)
                {
                    result.balance = result.balance - ((value * (Convert.ToDecimal(tax) / 100)) + value);
                    db.SaveChanges();
                }
            }
            return Json("sold");
        }
    }
}