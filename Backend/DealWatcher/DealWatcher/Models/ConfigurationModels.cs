using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DealWatcher.Models
{
    public class ConfigurationViewModel
    {
        public int Id { get; set; }
        public String Key { get; set; }
        public Object Value { get; set; }
    }

    public class ConfigurationBindingModel
    {
        public int Id { get; set; }
        public String Key { get; set; }
        public Object Value { get; set; }
        public int TypeId { get; set; }
    }
    
}