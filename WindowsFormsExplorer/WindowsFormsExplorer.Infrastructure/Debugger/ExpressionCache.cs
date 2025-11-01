using System;
using System.Collections.Generic;

namespace WindowsFormsExplorer.Infrastructure.Debugger
{
    /// <summary>
    /// Cache per i risultati delle espressioni del debugger
    /// Riduce il numero di chiamate a EnvDTE memorizzando temporaneamente i risultati
    /// </summary>
    public class ExpressionCache
    {
        private readonly Dictionary<string, CacheEntry> _cache;
        private readonly TimeSpan _cacheLifetime;

        public ExpressionCache(TimeSpan cacheLifetime)
        {
            _cache = new Dictionary<string, CacheEntry>();
            _cacheLifetime = cacheLifetime;
        }

        public ExpressionCache() : this(TimeSpan.FromSeconds(30))
        {
        }

        public bool TryGet(string expression, out string value)
        {
            if (_cache.TryGetValue(expression, out CacheEntry entry))
            {
                if (DateTime.UtcNow - entry.Timestamp < _cacheLifetime)
                {
                    value = entry.Value;
                    return true;
                }
                else
                {
                    // Rimuove le entry scadute
                    _cache.Remove(expression);
                }
            }

            value = null;
            return false;
        }

        public void Set(string expression, string value)
        {
            _cache[expression] = new CacheEntry
            {
                Value = value,
                Timestamp = DateTime.UtcNow
            };
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public void Remove(string expression)
        {
            _cache.Remove(expression);
        }

        private class CacheEntry
        {
            public string Value { get; set; }
            public DateTime Timestamp { get; set; }
        }
    }
}

