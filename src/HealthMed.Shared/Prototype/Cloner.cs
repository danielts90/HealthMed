using System.Reflection;
using System.Text.Json;

namespace HealthMed.Shared.Prototype
{
    public static class Cloner
    {
        public static T DeepClone<T>(this T source) where T : IPrototype<T>
        {
            if (source is IPrototype<T> prototype)
            {
                return prototype.DeepClone();
            }
            var json = JsonSerializer.Serialize(source);
            return JsonSerializer.Deserialize<T>(json);

        }
        public static T ShallowClone<T>(this T source) where T : IPrototype<T>
        {
            if (source is IPrototype<T> prototype)
            {
                return prototype.ShallowClone();
            }

            var method = typeof(object).GetMethod("MemberwiseClone",
                       BindingFlags.NonPublic | BindingFlags.Instance);

            return (T)method.Invoke(source, null);
        }
    }
}
