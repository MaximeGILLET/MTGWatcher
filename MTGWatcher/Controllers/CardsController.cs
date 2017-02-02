using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MTGWatcher.DAL;
using MTGWatcher.Models;
using Newtonsoft.Json;
using System.IO;
using System.Data.Entity.Validation;
using System.Linq.Dynamic;

namespace MTGWatcher.Controllers
{
    public class CardsController : Controller
    {
        private MTGWatcherContext db = new MTGWatcherContext();
        private static float RefreshSetsProgress { get; set; }
        // GET: Cards
        public ActionResult Index()
        {

            var model = new CardListViewModel();
            model.Results = db.Cards.ToList();
            model.ResultCount = model.Results.Count;
            model.PageSize = 25;
            model.Page = 1;
            model.SortField = "Name";
            model.SortDir = "DESC";
            model.Filter = "";

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(CardListViewModel model,string cardSearch)
        {

            if (model.Page == 0)
            {
                model.Results = db.Cards.ToList();
                model.ResultCount = model.Results.Count;
                model.PageSize = 25;
                model.Page = 1;
                model.SortField = "Name";
                model.SortDir = "DESC";
                model.Filter = "";
            }

            var filterCards = new List<Card>();
            //var results = FilterCards(model);
            if (!string.IsNullOrEmpty(cardSearch))
            {
                filterCards = db.Cards.Where(o => o.Name.Contains(cardSearch) || o.Text.Contains(cardSearch)).ToList();

                if (filterCards.Count == 1)
                    return RedirectToAction("Details", new { id = filterCards.FirstOrDefault().CardId });
            }

            model.ResultCount = db.Cards.Count();
            model.PageSize = 25;

            if (string.IsNullOrEmpty(model.SortField) || string.IsNullOrEmpty(model.SortDir))
                model.Results = db.Cards.OrderBy("Name", "DESC").Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();
            else
                model.Results = db.Cards.OrderBy(model.SortField, model.SortDir).Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();

            return View(model);
        }

        // GET: Cards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            if(string.IsNullOrEmpty(card.ImageUrl)) card.ImageUrl = CardImageUrlFind(card.Name);

            var product = JsonConvert.DeserializeObject<Product>(RequestHelper.mkmRequest("https://www.mkmapi.eu/ws/v2.0/products/find?search="+Url.Encode(card.Name)+"&exact=true&idGame=1&idLanguage=1"));

            return View(card);
        }

        // GET: Cards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CardId,Name,Cmc")] Card card)
        {
            if (ModelState.IsValid)
            {
                db.Cards.Add(card);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(card);
        }

        // GET: Cards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CardId,Name,Cmc")] Card card)
        {
            if (ModelState.IsValid)
            {
                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(card);
        }

        // GET: Cards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Card card = db.Cards.Find(id);
            db.Cards.Remove(card);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult RefreshCards()
        {
            try
            {
                var cards = JsonConvert.DeserializeObject<Dictionary<string, JsonCard>>(System.IO.File.ReadAllText(@"C:\Users\Néné\Documents\ProjectWeb\MTGWatcher\MTGWatcher\MTGWatcher\StaticJson\AllCards.json"));
                var dbCards = cards.Select(x => new Card(x.Value.name)).ToList();
                db.Cards.AddRange(dbCards);
                db.SaveChanges();

            }
            catch (Exception e)
            {
              
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RefreshSets()
        {

            try
            {
                var sets = JsonConvert.DeserializeObject<Dictionary<string, JsonSet>>(System.IO.File.ReadAllText(@"C:\Users\m.gillet\Documents\GitHub\MTGWatcher\MTGWatcher\StaticJson\AllSets.json"));
                RefreshSetsProgress = 0;
                float i = 0;
                foreach (var item in sets)
                {
                    i++;
                    RefreshSetsProgress=  i/(float)sets.Count;
                    db.Sets.Add(new SetModel() { Name = item.Value.name , Code = item.Value.code,ReleaseDate = item.Value.releaseDate});
                    db.SaveChanges();
                    foreach (var card in item.Value.cards)
                    {
                        
                        
                        card.SetId = db.Sets.Where(x => x.Code == item.Value.code).FirstOrDefault().SetId;
                        db.Cards.Add(card);
                    }
                }

                db.SaveChanges();
                
            }
            catch (Exception e)
            {
                
            }

            return RedirectToAction("Index");
        }

        public JsonResult RefreshProgress()
        {
            
            return Json(new { message="OK" ,data = RefreshSetsProgress}, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Autocomplete(string term)
        {
            var filteredItems = db.Cards.Where(item => item.Name.Contains(term));
            var nameList = filteredItems.Select(x => x.Name).ToList();
            return Json(nameList, JsonRequestBehavior.AllowGet);
        }

        private string CardImageUrlFind(string cardName)
        {
            var urlMultiverse = string.Format("http://gatherer.wizards.com/pages/search/default.aspx?name=+[{0}]",Url.Encode(cardName));
            var request = WebRequest.Create(urlMultiverse);
            var resp = request.GetResponse();
            var urlResp = resp.ResponseUri.ToString();
            var index = urlResp.IndexOf("multiverseid=");
            if (index == -1) return null;
            int multiverseid;
            if (!int.TryParse(urlResp.Substring(index + 13, urlResp.Length - index - 13), out multiverseid)) return null;
            string urlImage =string.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", multiverseid);
            resp.Dispose();
            return string.IsNullOrEmpty(urlImage)?null:urlImage;
        }
    }
}
