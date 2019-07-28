using System;
using System.Collections.Generic;
using FullSerializer;

namespace ParadoxNotion.Serialization
{
    ///Serializes/Deserializes to/from JSON with 'FullSerializer'
    public static class JSON
    {
        private static readonly fsSerializer serializer = new fsSerializer {Config = {SerializeEnumsAsInteger = true} };
        private static bool init;

        ///Serialize to json
        public static string Serialize(Type type, object value, bool pretyJson = false, List<UnityEngine.Object> objectReferences = null)
        {
            if (!init)
            {
                serializer.AddConverter(new fsUnityObjectConverter());
                init = true;
            }

            //set the objectReferences context
            if (objectReferences != null) serializer.Context.Set(objectReferences);

            //serialize the data
            fsData data;
            serializer.TrySerialize(type, value, out data).AssertSuccess();

            //print data to json
            if (pretyJson)
                return fsJsonPrinter.PrettyJson(data);
            return fsJsonPrinter.CompressedJson(data);
        }

        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="pretyJson">if set to <c>true</c> [prety json].</param>
        /// <param name="objectReferences">The object references.</param>
        /// <returns></returns>
        public static string Serialize<T>(object value, bool pretyJson = false, List<UnityEngine.Object> objectReferences = null)
        {
            return Serialize(typeof(T), value, pretyJson, objectReferences);
        }

        ///Deserialize generic
        public static T Deserialize<T>(string serializedState, List<UnityEngine.Object> objectReferences = null)
        {
            return (T)Deserialize(typeof(T), serializedState, objectReferences);
        }

        ///Deserialize from json
        public static object Deserialize(Type type, string serializedState, List<UnityEngine.Object> objectReferences = null)
        {

            if (!init)
            {
                serializer.AddConverter(new fsUnityObjectConverter());
                init = true;
            }

            if (objectReferences != null)
                serializer.Context.Set(objectReferences);

            //parse the JSON data
            var data = fsJsonParser.Parse(serializedState);

            //deserialize the data
            object deserialized = null;
            serializer.TryDeserialize(data, type, ref deserialized).AssertSuccess();

            return deserialized;
        }
    }
}