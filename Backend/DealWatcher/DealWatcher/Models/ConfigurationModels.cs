using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DealWatcher.Models
{
    public class ConfigurationViewModel
    {
        [Required]
        public String Key { get; set; }
        [Required]
        public Object Value { get; set; }
        public String Type { get; set; }
    }
}