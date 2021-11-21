using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MediaShareMVC_Core.Data;
using MediaShareMVC_Core.Models;

namespace MediaShareMVC_Core.Controllers
{
    public class IndexModel : PageModel
    {
        private readonly MediaShareMVC_Core.Data.MediaShareContextAnotherOne _context;

        public IndexModel(MediaShareMVC_Core.Data.MediaShareContextAnotherOne context)
        {
            _context = context;
        }

        public IList<Media> Media { get;set; }

        public async Task OnGetAsync()
        {
            Media = await _context.Media.ToListAsync();
        }
    }
}
