using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MediaShareMVC_Core.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the WebAppUser class
    public class WebAppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string FName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string LName { get; set; }
    }
}
