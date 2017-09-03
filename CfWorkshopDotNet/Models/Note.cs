using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CfWorkshopDotNet.Models
{
    public class Note
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
    }
}