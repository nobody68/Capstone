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

        public ActionResult SearchResults(AmazonSearch searchParameters)
        {
            AWSECommerceServicePortTypeClient amazonClient = new AWSECommerceServicePortTypeClient();

            // prepare an ItemSearch request
            ItemSearchRequest request = new ItemSearchRequest();
            request.SearchIndex = searchParameters.ProductType.ToString();
            request.Title = searchParameters.SearchText;
            request.ResponseGroup = new string[] { "Small", "Reviews" };

            ItemSearch itemSearch = new ItemSearch();
            itemSearch.Request = new ItemSearchRequest[] { request };
            itemSearch.AWSAccessKeyId = ConfigurationManager.AppSettings["AKIAJQRB4ENL4MVWZTTQ"];
            itemSearch.AssociateTag = "donncord-20";

            // send the ItemSearch request
            ItemSearchResponse response = amazonClient.ItemSearch(itemSearch);

            return View(response.Items[0]);
        }
    }
}