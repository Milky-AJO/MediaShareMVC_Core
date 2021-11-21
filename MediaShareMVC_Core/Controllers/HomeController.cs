using MediaShareMVC_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;

namespace MediaShareMVC_Core.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult PersonalImages()
        {
            string userEmail = HttpContext.User.Identity.Name;

            List<Media> MyImages = new List<Media>();
            string connString = "Server=awsmediasharedb.cijc6laeupag.us-east-2.rds.amazonaws.com,1433;Integrated Security=False;Persist Security Info=True;User ID=admin;Password=#Cmpg323;Database=MediaShareMVC_Core;Trusted_Connection=False;MultipleActiveResultSets=true";
            using (SqlConnection Conn = new SqlConnection(connString))
            {
                Conn.Open();
                SqlCommand sqlCom = new SqlCommand("SELECT MediaTitle, MediaName, MediaPublic FROM Media WHERE Email='" + userEmail + "'", Conn);
                SqlDataReader reader = sqlCom.ExecuteReader();

                while (reader.Read())
                {
                    Media thisImage = new Media();
                    thisImage.MediaTitle = (string)reader["MediaTitle"];
                    thisImage.MediaName = (string) reader["MediaName"];
                    MyImages.Add(thisImage);
                }

                Conn.Close();
            }

            return View(MyImages);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
