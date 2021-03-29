using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Login.Models.Map
{
    public class MapModel
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Url { get; set; }

    }

    public class MapModelList
    {
        public IEnumerable<MapModel> MapCordsList { get; set; }
    }
}
