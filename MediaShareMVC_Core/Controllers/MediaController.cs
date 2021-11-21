using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MediaShareMVC_Core.Data;
using MediaShareMVC_Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace MediaShareMVC_Core.Controllers
{
    public class MediaController : Controller
    {
        private readonly MediaShareMVC_CoreContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MediaController(MediaShareMVC_CoreContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Medias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Media.ToListAsync());
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
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(Media.MediaFile.FileName);
                string fileExtension = Path.GetExtension(Media.MediaFile.FileName);
                Media.MediaName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    await Media.MediaFile.CopyToAsync(fileStream);
                }

                _context.Add(Media);
                await _context.SaveChangesAsync();
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
