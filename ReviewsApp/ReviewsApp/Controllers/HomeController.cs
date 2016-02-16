using ReviewsApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ReviewsApp.Code;
using ReviewsApp.AWSECommerceService;

namespace ReviewsApp.Controllers
{
    public class HomeController : Controller
    {

        private MyDbContext db;
        private UserManager<ApplicationUser> manager;

        public HomeController()
        {
            db = new MyDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        public ActionResult Index()
        {
            return View();
        }

        // Only Authenticated users can access thier profile
        [Authorize]
        public ActionResult Profile()
        {
            // Instantiate the ASP.NET Identity system
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new MyDbContext()));
            
            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId()); 
            
            // Recover the profile information about the logged in user
            ViewBag.HomeTown = currentUser.HomeTown;
            ViewBag.FirstName = currentUser.MyUserInfo.FirstName;

            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProductSearchResults(AmazonSearch searchParameters)
        {
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

            // prepare an ItemSearch request
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = searchParameters.ProductType.ToString();
            request.Title = searchParameters.SearchText;
            request.ResponseGroup = new string[] { "Medium", "Reviews", "Images", "ItemAttributes" };

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { request };
            itemSearch.AWSAccessKeyId = ConfigurationManager.AppSettings["AKIAJQRB4ENL4MVWZTTQ"];
            itemSearch.AssociateTag = "donncord-20";

            // send the ItemSearch request
            ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

            return View(response.Items[0]);
        }

        //[HttpPost]
        public ActionResult ReviewSearchResults(string ASIN, string iFrameUrl)
        //public ActionResult ReviewSearchResults(ReviewSearch reviewSearch)
        {
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();
            ItemLookup itemLookup = new ItemLookup()
            {
                AssociateTag = "donncord-20"
            };
            itemLookup.AWSAccessKeyId = "AKIAJQRB4ENL4MVWZTTQ";

            ItemLookupRequest itemLookupRequest = new ItemLookupRequest();
            itemLookupRequest.IdTypeSpecified = true;
            itemLookupRequest.IdType = ItemLookupRequestIdType.ASIN;
            itemLookupRequest.ItemId = new String[] { ASIN };
            itemLookupRequest.ResponseGroup = new String[] { "Medium", "Reviews", "Images", "ItemAttributes" };
            itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };

            ItemLookupResponse response = amazonClient.ItemLookup(itemLookup);

            var items = (response.Items[0]);
            var item = items.Item[0];
            //foreach (var item in response.Items[0])

            var product = new Product
            {
                ASIN = item.ASIN
            };

            var reviews = new List<Review>();

            reviews.Add(
                new Review
                {
                    ASIN = product.ASIN,
                    Link = item.CustomerReviews.IFrameURL
                });
            product.Reviews = reviews;

            return View(product);
        }

        public ActionResult SaveReviewToFavorites(string ASIN, string returnUrl)
        {
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();
            ItemLookup itemLookup = new ItemLookup()
            {
                AssociateTag = "donncord-20"
            };
            itemLookup.AWSAccessKeyId = "AKIAJQRB4ENL4MVWZTTQ";

            ItemLookupRequest itemLookupRequest = new ItemLookupRequest();
            itemLookupRequest.IdTypeSpecified = true;
            itemLookupRequest.IdType = ItemLookupRequestIdType.ASIN;
            itemLookupRequest.ItemId = new String[] { ASIN };
            itemLookupRequest.ResponseGroup = new String[] { "Medium", "Reviews", "Images", "ItemAttributes" };
            itemLookup.Request = new ItemLookupRequest[] { itemLookupRequest };

            ItemLookupResponse response = amazonClient.ItemLookup(itemLookup);

            var items = (response.Items[0]);
            var item = items.Item[0];
            //foreach (var item in response.Items[0])

            var product = new Product
            {
                ASIN = item.ASIN,
                Name = item.ItemAttributes.Title
            };

            var review = new Review
            {
                ASIN = product.ASIN,
                Link = item.CustomerReviews.IFrameURL,
            };

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();

                var storedProduct = db.Products.SingleOrDefault(x => x.ASIN == ASIN);
                review.ProductId = storedProduct.Id;
                

                //review.Product = storedProduct;

                db.Reviews.Add(review);
                db.SaveChanges();
                return Redirect(returnUrl);
            }


            return Redirect(returnUrl);
        }
    }
}