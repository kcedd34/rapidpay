using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using RapidPay.Authorize;
using RapidPay.Models;

namespace RapidPay.Controllers
{
    public class CardsController : Controller
    {
        private RapidPayEntities db = new RapidPayEntities();


        // GET: cards
        [GetAuthorize]
        [HttpPost]
        public ActionResult List()
        {
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(db.card.ToList()));
        }

        [HttpPost]
        [GetAuthorize]
        public ActionResult Get(string cardNumber)
        {
            return Json(db.card.Where(x => x.card1 == cardNumber).Sum(x => x.balance).ToString()); ;
        }


        // GET: cards/Create
        [GetAuthorize]
        public ActionResult Create()
        {
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();
            card cardNumber = Newtonsoft.Json.JsonConvert.DeserializeObject<card>(json);
            if (db.card.Where(x => x.card1 == cardNumber.card1).Count() > 0)
            {
                return Json("this card already exists");
            }
            else
            {
                db.card.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<card>(json));
                db.SaveChanges();
                return Json("created");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
