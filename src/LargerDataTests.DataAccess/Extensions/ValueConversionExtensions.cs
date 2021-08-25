using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;

namespace LargerDataTests.DataAccess.Extensions
{
    /// <summary>
    /// Extensions for value convertors.
    /// </summary>
    public static class ValueConversionExtensions
    {
        /// <summary>
        /// Add a JSON value convertor.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="propertyBuilder">Property builder.</param>
        public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var converter = new ValueConverter<T, string>(
                obj => JsonConvert.SerializeObject(obj, jsonSerializerSettings),
                json => JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings));

            var comparer = new ValueComparer<T>(
                equalsExpression: (l, r) => JsonConvert.SerializeObject(l, jsonSerializerSettings) == JsonConvert.SerializeObject(r, jsonSerializerSettings),
                hashCodeExpression: v => v == null ? 0 : JsonConvert.SerializeObject(v, jsonSerializerSettings).GetHashCode(),
                snapshotExpression: v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v, jsonSerializerSettings), jsonSerializerSettings));

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }
    }
}
