//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestDockerWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ManageContainer
    {
        public int id { get; set; }
        public string username { get; set; }
        public string containername { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public double cpu { get; set; }
        public string ram { get; set; }
        public string sshkey { get; set; }
        public System.DateTime enddate { get; set; }
        public string status { get; set; }
        public string sshcommand { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
