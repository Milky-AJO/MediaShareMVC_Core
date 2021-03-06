using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaShareMVC_Core.Data;
using MediaShareMVC_Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;

namespace MediaShareMVC_Core.Controllers
{
    [Authorize]
    public class MediaController : Controller
    {
        private readonly MediaShareMVC_CoreContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        private static readonly string bucketName = "awscoremvcap-co1ww7khrdzzoyfbf13jx1bjfc91wuse2a-s3alias";
        //awscoremvc

        public MediaController(MediaShareMVC_CoreContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Medias
        public async Task<IActionResult> Index(string Search)
        {
            string userEmail = HttpContext.User.Identity.Name;
            List<Media> MyImages = new List<Media>();
            if (Search == null)
            {
                string connString = "Server=awsmediasharedb.cijc6laeupag.us-east-2.rds.amazonaws.com,1433;Integrated Security=False;Persist Security Info=True;User ID=admin;Password=#Cmpg323;Database=MediaShareMVC_Core;Trusted_Connection=False;MultipleActiveResultSets=true";
                using (SqlConnection Conn = new SqlConnection(connString))
                {
                    Conn.Open();
                    SqlCommand sqlCom = new SqlCommand("SELECT MediaId ,MediaTitle, MediaName, Email ,MediaPublic FROM Media WHERE Email='" + userEmail + "'", Conn);
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        Media thisImage = new Media();

                        thisImage.MediaId = (int)reader["MediaId"];
                        thisImage.MediaTitle = (string)reader["MediaTitle"];
                        thisImage.MediaName = (string)reader["MediaName"];
                        thisImage.Email = (string)reader["Email"];
                        thisImage.MediaPublic = Convert.ToBoolean(Convert.ToInt32(reader["MediaPublic"]));
                        //thisImage.MediaLink = ;
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
                    SqlCommand sqlCom = new SqlCommand("SELECT MediaId ,MediaTitle, MediaName, Email ,MediaPublic FROM Media WHERE Email='" + userEmail + "'" + " AND MediaTitle LIKE '%" + Search +"%'", Conn);
                    SqlDataReader reader = sqlCom.ExecuteReader();

                    while (reader.Read())
                    {
                        Media thisImage = new Media();

                        thisImage.MediaId = (int)reader["MediaId"];
                        thisImage.MediaTitle = (string)reader["MediaTitle"];
                        thisImage.MediaName = (string)reader["MediaName"];
                        thisImage.Email = (string)reader["Email"];
                        thisImage.MediaPublic = Convert.ToBoolean(Convert.ToInt32(reader["MediaPublic"]));
                        //thisImage.MediaLink = ;
                        MyImages.Add(thisImage);
                    }

                    Conn.Close();
                }
            }

            //return View(await _context.Media.ToListAsync());
            await _context.SaveChangesAsync();
            return View(MyImages);
        }

        // GET: Medias/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Media = await _context.Media
                .FirstOrDefaultAsync(m => m.MediaId == id);
            if (Media == null)
            {
                return NotFound();
            }

            return View(Media);
        }

        // GET: Medias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MediaId,MediaTitle,MediaFile,Email,MediaPublic")] Media Media)
        {
            var s3Client = new AmazonS3Client("AKIARC6VCC5JCWYLCANP", "tr3wtrmS2Jbshv3TVkA9CSXuDpRTq9Lhb87iTZVz", Amazon.RegionEndpoint.USEast2);

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(Media.MediaFile.FileName);
                string fileExtension = Path.GetExtension(Media.MediaFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                Media.MediaName = fileName;
                Media.Email = HttpContext.User.Identity.Name;



                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await Media.MediaFile.CopyToAsync(fileStream);
                }

                _context.Add(Media);
                await _context.SaveChangesAsync();

                PutObjectRequest putRequest = new PutObjectRequest
                {
                    
                    BucketName = bucketName,
                    Key = "Images/" + fileName,
                    FilePath = path,
                    ContentType = "text/plain"
                };

                PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);

                if (System.IO.File.Exists(path))
                {

                    System.IO.File.Delete(path);

                }

                return RedirectToAction(nameof(Index));
            }
            return View(Media);
        }

        // GET: Medias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Media = await _context.Media.FindAsync(id);
            if (Media == null)
            {
                return NotFound();
            }
            return View(Media);
        }

        // POST: Medias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MediaId,MediaTitle,MediaName,Email,MediaPublic")] Media Media)
        {
            if (id != Media.MediaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Media);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MediaExists(Media.MediaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(Media);
        }

        // GET: Medias/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Media = await _context.Media
                .FirstOrDefaultAsync(m => m.MediaId == id);
            if (Media == null)
            {
                return NotFound();
            }

            return View(Media);
        }

        // POST: Medias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Media = await _context.Media.FindAsync(id);
            _context.Media.Remove(Media);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MediaExists(int id)
        {
            return _context.Media.Any(e => e.MediaId == id);
        }
    }
}
