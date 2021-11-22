using MediaShareMVC_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MediaShareMVC_Core.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;

namespace MediaShareMVC_Core.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MediaShareMVC_CoreContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private static readonly string bucketName = "awscoremvcap-co1ww7khrdzzoyfbf13jx1bjfc91wuse2a-s3alias";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger , MediaShareMVC_CoreContext context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        public async Task<IActionResult> Index(string Search)
        {
            //string userEmail = HttpContext.User.Identity.Name;
            List<Media> MyImages = new List<Media>();
            if (Search == null)
            {
                string connString = "Server=awsmediasharedb.cijc6laeupag.us-east-2.rds.amazonaws.com,1433;Integrated Security=False;Persist Security Info=True;User ID=admin;Password=#Cmpg323;Database=MediaShareMVC_Core;Trusted_Connection=False;MultipleActiveResultSets=true";
                using (SqlConnection Conn = new SqlConnection(connString))
                {
                    Conn.Open();
                    SqlCommand sqlCom = new SqlCommand("SELECT MediaId ,MediaTitle, MediaName, Email ,MediaPublic FROM Media WHERE MediaPublic='1'", Conn);
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        Media thisImage = new Media();

                        thisImage.MediaId = (int)reader["MediaId"];
                        thisImage.MediaTitle = (string)reader["MediaTitle"];
                        thisImage.MediaName = (string)reader["MediaName"];
                        thisImage.Email = (string)reader["Email"];
                        thisImage.MediaPublic = Convert.ToBoolean(Convert.ToInt32(reader["MediaPublic"]));
                        
                        MyImages.Add(thisImage);
                    }

                    Conn.Close();
                }
            }
            else
            {
                string connString = "Server=awsmediasharedb.cijc6laeupag.us-east-2.rds.amazonaws.com,1433;Integrated Security=False;Persist Security Info=True;User ID=admin;Password=#Cmpg323;Database=MediaShareMVC_Core;Trusted_Connection=False;MultipleActiveResultSets=true";
                using (SqlConnection Conn = new SqlConnection(connString))
                {
                    Conn.Open();
                    SqlCommand sqlCom = new SqlCommand("SELECT MediaId ,MediaTitle, MediaName, Email, MediaPublic FROM Media WHERE MediaPublic='1'" + " AND (MediaTitle LIKE '%" + Search + "%' OR Email LIKE '%" + Search + "%')", Conn);// OR Email LIKE '%" + Search + "%')
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        Media thisImage = new Media();

                        thisImage.MediaId = (int)reader["MediaId"];
                        thisImage.MediaTitle = (string)reader["MediaTitle"];
                        thisImage.MediaName = (string)reader["MediaName"];
                        thisImage.Email = (string)reader["Email"];
                        thisImage.MediaPublic = Convert.ToBoolean(Convert.ToInt32(reader["MediaPublic"]));
                        MyImages.Add(thisImage);
                    }

                    Conn.Close();
                }
            }

            //return View(await _context.Media.ToListAsync());
            await _context.SaveChangesAsync();
            return View(MyImages);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
