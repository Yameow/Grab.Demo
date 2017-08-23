using Grab.CacheProvider.Cache;
using Grab.Interface.Cache;
using System.Configuration;

namespace Grab.Common
{
    public static class ConfigHelper
    {
        private static readonly ICache cache = new MemoryCache();

        /// <summary>
        /// 得到AppSettings中的配置字符串信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConfigString(this string key)
        {
            string cacheKey = "AppSettings_" + key;

            return cache.Get<string>(cacheKey, () =>
            {
                var obj = ConfigurationManager.AppSettings[key];
                if (obj == null)
                {
                    return string.Empty;
                    //throw new Exception("获取配置信息失败");
                }
                return obj.ToString();
            }, 60 * 24 * 180);
        }

        /// <summary>
        /// 得到AppSettings中的配置Bool信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool GetConfigBool(this string key, bool defaultVal = false)
        {
            bool.TryParse(GetConfigString(key), out defaultVal);
            return defaultVal;
        }

        /// <summary>
        /// 得到AppSettings中的配置Decimal信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static decimal GetConfigDecimal(this string key, decimal defaultVal = 0)
        {
            decimal.TryParse(GetConfigString(key), out defaultVal);
            return defaultVal;
        }

        /// <summary>
        /// 得到AppSettings中的配置int信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetConfigInt(this string key, int defaultVal = 0)
        {
            int.TryParse(GetConfigString(key), out defaultVal);
            return defaultVal;
        }

        /// <summary>
        /// 得到AppSettings中的配置long信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static long GetConfigLong(this string key, long defaultVal = 0)
        {
            long.TryParse(GetConfigString(key), out defaultVal);
            return defaultVal;
        }

    }
}
