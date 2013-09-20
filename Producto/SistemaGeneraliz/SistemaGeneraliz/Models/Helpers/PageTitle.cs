using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaGeneraliz.Models.Helpers
{
    public class PageTitle
    {
        public string Big { get; set; }
        public string Small { get; set; }

        public PageTitle(string big, string small = "")
        {
            Big = big;
            Small = small;
        }
    }
}