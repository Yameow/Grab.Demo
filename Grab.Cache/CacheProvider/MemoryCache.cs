using Grab.Interface.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace Grab.CacheProvider.Cache
{
    public class MemoryCache : ICache
    {
        private const string REGION_NAME = "$#MemoryCache#$";
        private const int _DefaultCacheTime = 30;
        private readonly static object s_lock = new object();

        protected ObjectCache Cache
        {
            get
            {
                return System.Runtime.Caching.MemoryCache.Default;
            }
        }

        public IEnumerable<KeyValuePair<string, object>> Entries
        {
            get
            {
                return Cache
                    .Where(e => e.Key.StartsWith(REGION_NAME))
                    .Select(k => new KeyValuePair<string, object>(k.Key.Substring(REGION_NAME.Length), k.Value));
            }
        }

        public T Get<T>(string key, Func<T> baseMethod)
        {
            return Get<T>(key, baseMethod, _DefaultCacheTime);
        }

        public T Get<T>(string key, Func<T> baseMethod, int cacheTime)
        {
            key = BuildKey(key);

            if (Cache.Contains(key))
            {
                return (T)Cache.Get(key);
            }
            else
            {
                lock (s_lock)
                {
                    if (!Cache.Contains(key))
                    {
                        var value = baseMethod();
                        if (value != null)
                        {
                            var cacheItem = new CacheItem(key, value);
                            CacheItemPolicy policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) }; ;
                            Cache.Add(cacheItem, policy);
                        }
                        return value;
                    }
                }
                return (T)Cache.Get(key);
            }
        }

        public bool Contains(string key)
        {
            return Cache.Contains(BuildKey(key));
        }

        public void Remove(string key)
        {
            Cache.Remove(BuildKey(key));
        }

        private string BuildKey(string key)
        {
            return string.IsNullOrEmpty(key) ? null : REGION_NAME + key;
        }
    }
}
