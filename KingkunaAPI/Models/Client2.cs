using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KingkunaAPI.Models {
    public partial class Client2 {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public String CreateAt { get; set; }
        public String HireDate { get; set; }
        public String CancelDate { get; set; }
        public int DemoDays { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public double Amount { get; set; }
    }
}