using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediaShareMVC_Core.Models
{
    public class MediaItemModel
    {
        [Key]
        public int MediaId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string MediaTitle { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string MediaName { get; set; }

        [ForeignKey("AspNetUsers")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        public bool MediaPublic { get; set; }
    }
}
