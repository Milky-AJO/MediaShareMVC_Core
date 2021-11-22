using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MediaShareMVC_Core.Models
{
    public class Media
    {
        [Key]
        public int MediaId { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Image Title")]
        public string MediaTitle { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string MediaName { get; set; }

        [ForeignKey("AspNetUsers")]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(10)")]
        [DisplayName("Public Image")]
        public bool MediaPublic { get; set; }
        
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile MediaFile { get; set; }

        //[NotMapped]
        //public string MediaLink { get; set; }
    }
}
