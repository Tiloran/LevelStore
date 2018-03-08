using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LevelStore.Infrastructure
{
    public static class SessionExtension
    {
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);
            T result = default(T);
            if (sessionData != null)
            {
                result = JsonConvert.DeserializeObject<T>(sessionData);
            }
            return result;
        }
    }
}
