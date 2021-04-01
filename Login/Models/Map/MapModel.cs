using System.Collections.Generic;

namespace Login.Models.Map
{
    public class MapModel
    {
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string Url { get; set; }

    }

    public class MapModelList
    {
        public string Username { get; set; }
        public IEnumerable<MapModel> MapCordsList { get; set; }

    }

}
