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
    
    public partial class ProductCode
    {
        public int TypeId { get; set; }
        public int ProductId { get; set; }
        public string Code { get; set; }
    
        public virtual ProductCodeType ProductCodeType { get; set; }
        public virtual Product Product { get; set; }
    }
}