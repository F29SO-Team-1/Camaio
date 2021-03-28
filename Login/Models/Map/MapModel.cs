using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models.Map
{
    public class MapModel
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Error { get; set; }

        public bool HasValidCords()
        {
            return Lat.HasValue && Lng.HasValue;
        }
    }
}
