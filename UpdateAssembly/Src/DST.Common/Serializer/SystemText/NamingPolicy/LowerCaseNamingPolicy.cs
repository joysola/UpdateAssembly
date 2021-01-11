using System.Text.Json;

namespace DST.Common.Helper
{
    /// <summary>
    /// 将属性转成小写
    /// </summary>
    public class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name) => name.ToLower();
    }
}