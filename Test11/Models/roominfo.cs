//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Test11.Models
{
    using System;
    using System.Collections.Generic;
    public enum rStatus1
    {
        To_Do = 0,
        In_Processing = 1,
        Done = 2
    }
    public partial class roominfo
    {
        //public IEnumerable<roominfo> rId { get; set; }
        public int rId { get; set; }
        public int rStatus { get; set; }
        public int hId { get; set; }
    }
}
