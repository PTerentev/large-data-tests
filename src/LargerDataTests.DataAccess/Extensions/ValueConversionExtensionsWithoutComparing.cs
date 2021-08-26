using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace LargerDataTests.DataAccess.Extensions
{
    /// <summary>
    /// Extensions for value convertors.
    /// </summary>
    public static class ValueConversionExtensionsWithoutComparing
    {
        /// <summary>
        /// Add a JSON value convertor.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="propertyBuilder">Property builder.</param>
        public static PropertyBuilder<T> HasNewtonsoftJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            var converter = new ValueConverter<T, string>(
                obj => JsonConvert.SerializeObject(obj, jsonSerializerSettings),
                json => JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings));

            //var comparer = new ValueComparer<T>(
            //    equalsExpression: (l, r) => JsonConvert.SerializeObject(l, jsonSerializerSettings) == JsonConvert.SerializeObject(r, jsonSerializerSettings),
            //    hashCodeExpression: v => v == null ? 0 : JsonConvert.SerializeObject(v, jsonSerializerSettings).GetHashCode(),
            //    snapshotExpression: v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v, jsonSerializerSettings), jsonSerializerSettings));

            propertyBuilder.HasConversion(converter);
            //propertyBuilder.Metadata.SetValueConverter(converter);
            //propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }

        public static PropertyBuilder<T> HasSystemJsonConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            var jsonSerializerSettings = new System.Text.Json.JsonSerializerOptions()
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            };

            var converter = new ValueConverter<T, string>(
                obj => System.Text.Json.JsonSerializer.Serialize(obj, jsonSerializerSettings),
                json => System.Text.Json.JsonSerializer.Deserialize<T>(json, jsonSerializerSettings));

            //var comparer = new ValueComparer<T>(
            //    equalsExpression: (l, r) => System.Text.Json.JsonSerializer.Serialize(l, jsonSerializerSettings) == System.Text.Json.JsonSerializer.Serialize(r, jsonSerializerSettings),
            //    hashCodeExpression: v => v == null ? 0 : System.Text.Json.JsonSerializer.Serialize(v, jsonSerializerSettings).GetHashCode(),
            //    snapshotExpression: v => System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.Serialize(v, jsonSerializerSettings), jsonSerializerSettings));

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            //propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }

        public static PropertyBuilder<T> HasSystemJsonByteArrayConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            var jsonSerializerSettings = new System.Text.Json.JsonSerializerOptions()
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString,
            };

            var converter = new ValueConverter<T, byte[]>(
                obj => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(obj, jsonSerializerSettings),
                json => System.Text.Json.JsonSerializer.Deserialize<T>(json, jsonSerializerSettings));

            //var comparer = new ValueComparer<T>(
            //    equalsExpression: (l, r) => System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(l, jsonSerializerSettings) == System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(r, jsonSerializerSettings),
            //    hashCodeExpression: v => v == null ? 0 : System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(v, jsonSerializerSettings).GetHashCode(),
            //    snapshotExpression: v => System.Text.Json.JsonSerializer.Deserialize<T>(System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(v, jsonSerializerSettings), jsonSerializerSettings));

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            //propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }

        public static PropertyBuilder<T> HasSystemJsonCompressedConversion<T>(this PropertyBuilder<T> propertyBuilder)
        {
            var jsonSerializerSettings = new System.Text.Json.JsonSerializerOptions()
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            var converter = new ValueConverter<T, string>(
                obj => CompressedJsonSerializer.Serialize(obj, jsonSerializerSettings),
                json => CompressedJsonSerializer.Deserialize<T>(json, jsonSerializerSettings));

            //var comparer = new ValueComparer<T>(
            //    equalsExpression: (l, r) => CompressedJsonSerializer.Serialize(l, jsonSerializerSettings) == CompressedJsonSerializer.Serialize(r, jsonSerializerSettings),
            //    hashCodeExpression: v => v == null ? 0 : CompressedJsonSerializer.Serialize(v, jsonSerializerSettings).GetHashCode(),
            //    snapshotExpression: v => CompressedJsonSerializer.Deserialize<T>(CompressedJsonSerializer.Serialize(v, jsonSerializerSettings), jsonSerializerSettings));

            propertyBuilder.HasConversion(converter);
            propertyBuilder.Metadata.SetValueConverter(converter);
            //propertyBuilder.Metadata.SetValueComparer(comparer);

            return propertyBuilder;
        }

        private static class CompressedJsonSerializer
        {
            public static string Serialize<T>(T value, System.Text.Json.JsonSerializerOptions options)
            {
                var uncompressedString = System.Text.Json.JsonSerializer.Serialize(value, options);

                byte[] compressedBytes;
                using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
                {
                    using (var compressedStream = new MemoryStream())
                    {
                        // setting the leaveOpen parameter to true to ensure that compressedStream will not be closed when compressorStream is disposed
                        // this allows compressorStream to close and flush its buffers to compressedStream and guarantees that compressedStream.ToArray() can be called afterward
                        // although MSDN documentation states that ToArray() can be called on a closed MemoryStream, I don't want to rely on that very odd behavior should it ever change
                        using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                        {
                            uncompressedStream.CopyTo(compressorStream);
                        }

                        // call compressedStream.ToArray() after the enclosing DeflateStream has closed and flushed its buffer to compressedStream
                        compressedBytes = compressedStream.ToArray();
                    }
                }

                return Convert.ToBase64String(compressedBytes);
            }

            public static T Deserialize<T>(string value, System.Text.Json.JsonSerializerOptions options)
            {
                byte[] decompressedBytes;

                var compressedStream = new MemoryStream(Convert.FromBase64String(value));

                using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        decompressorStream.CopyTo(decompressedStream);

                        decompressedBytes = decompressedStream.ToArray();
                    }
                }

                var uncompressedString = Encoding.UTF8.GetString(decompressedBytes);
                return System.Text.Json.JsonSerializer.Deserialize<T>(uncompressedString, options);
            }
        }
    }
}
