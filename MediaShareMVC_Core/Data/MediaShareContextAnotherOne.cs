using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MediaShareMVC_Core.Models;

namespace MediaShareMVC_Core.Data
{
    public class MediaShareContextAnotherOne : DbContext
    {
        public MediaShareContextAnotherOne (DbContextOptions<MediaShareContextAnotherOne> options)
            : base(options)
        {
        }

        public DbSet<MediaShareMVC_Core.Models.Media> Media { get; set; }
    }
}
