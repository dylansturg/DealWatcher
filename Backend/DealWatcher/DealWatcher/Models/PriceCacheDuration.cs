//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DealWatcher.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class PriceCacheDuration
    {
        public int SellerId { get; set; }
        public System.TimeSpan CacheLifetime { get; set; }
    
        public virtual Seller Seller { get; set; }
    }
}
