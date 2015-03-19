using System;
using System.ComponentModel.DataAnnotations;

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