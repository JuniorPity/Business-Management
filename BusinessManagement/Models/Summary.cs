using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessManagement.Models
{
    public class Summary
    {
        public Dictionary<DateTime, string> events { get; set; }
        public Dictionary<int, string> yearlyEvents { get; set; }
        public double totalHours { get; set; }
        public double totalPay { get; set; }
        public string name { get; set; }
        public double payRate { get; set; }
    }
}