using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaShareMVC_Core.Models;

namespace MediaShareMVC_Core.Data
{
    public class MediaShareMVC_CoreContext : DbContext
    {
        public MediaShareMVC_CoreContext (DbContextOptions<MediaShareMVC_CoreContext> options)
            : base(options)
        {
        }

        public DbSet<MediaShareMVC_Core.Models.Media> Media { get; set; }
    }
}
