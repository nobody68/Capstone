using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ReviewsApp.AWSECommerceService;
using ReviewsApp.Models;

namespace ReviewsApp.Controllers
{
    [Authorize]
    public class FavoritesController : Controller
    {
        private MyDbContext db;
        private UserManager<ApplicationUser> manager;

        public FavoritesController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Favorites
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            var favorites = db.Favorites.Include(f => f.Product);
            return View(favorites.ToList().Where(favorite => favorite.User.Id == currentUser.Id));
        }

        // GET: /ToDo/All
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> All()
        {
            return View(await db.Favorites.ToListAsync());
        }

        // GET: Favorites/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var favorite = await db.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            if (favorite.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(favorite);
        }

        // GET: Favorites/Create
        public ActionResult Create()
        {
            ViewBag.ASIN = new SelectList(db.Products, "ASIN", "Name");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,UserId,ASIN")] Favorite favorite)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                favorite.User = currentUser;
                db.Favorites.Add(favorite);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(favorite);
        }

        // GET: Favorites/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var favorite = await db.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            if (favorite.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            ViewBag.ASIN = new SelectList(db.Products, "ASIN", "Name");

            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,UserId,ASIN")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorite).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ASIN = new SelectList(db.Products, "ASIN", "Name");
            return View(favorite);
        }

        // GET: Favorites/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            var currentUser = await manager.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var favorite = await db.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            if (favorite.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }
            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Favorite favorite = await db.Favorites.FindAsync(id);
            db.Favorites.Remove(favorite);
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

        // POST: Favorites/SaveFavoriteToAccount
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        protected async Task<ActionResult> SaveFavoriteToAccount(string returnUrl)
        {
            //var product = new Product
            //{
            //    ASIN = item.ASIN,
            //    Name = item.ItemAttributes.Title
            //};

            Favorite favorite = new Favorite();
            if (ModelState.IsValid)
            {
                //db.Products.Add(product);
                //favorite.Product = product;
                //favorite.ASIN = item.ASIN;
                favorite.User = manager.FindById(User.Identity.GetUserId());

                db.Favorites.Add(favorite);
                await db.SaveChangesAsync();
            }

            return Redirect(returnUrl);
        }
    }
}
