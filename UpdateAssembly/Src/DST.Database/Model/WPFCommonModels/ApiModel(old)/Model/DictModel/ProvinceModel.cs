using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DST.Common.Model
{
    public class ProvinceModel
    {
        /// <summary>
        /// 省份id
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; set; }
        /// <summary>
        /// 省份id
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
