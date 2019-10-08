using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Solar.Models
{
    public partial class ESPRoot
    {
        [JsonProperty("Panel")]
        public PanelClass ESPData { get; set; }

        [JsonProperty("V")]
        public double[] V { get; set; }

        [JsonProperty("I")]
        public double[] I { get; set; }
    }

    public partial class PanelClass
    {
        [JsonProperty("IR")]
        public double Ir { get; set; }

        [JsonProperty("T")]
        public double T { get; set; }
    }
}
