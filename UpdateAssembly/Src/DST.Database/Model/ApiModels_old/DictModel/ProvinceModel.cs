using System.Text.Json.Serialization;

namespace DST.Database.Model.ApiModels_old
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