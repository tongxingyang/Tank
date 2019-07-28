// ----------------------------------------------------------------------------
//  <copyright file="CustomConvert.cs" company="上海序曲网络科技有限公司">
//  Copyright (C) 2015 上海序曲网络科技有限公司
//  All rights are reserved. Reproduction or transmission in whole or in part, in
//  any form or by any means, electronic, mechanical or otherwise, is prohibited 
//  without the prior written consent of the copyright owner.
//  </copyright>
//  <author>Ben</author>
//  <date>2016/07/16 17:57</date>
// ----------------------------------------------------------------------------

#if !NO_UNITY
using System;
using System.Collections;
using FullSerializer.Internal;

namespace FullSerializer.Extend.Converters
{
    public class CustomConvert : fsConverter
    {
        public override bool CanProcess(Type type)
        {
            if (type.Resolve().IsArray || typeof(ICollection).IsAssignableFrom(type))
            {
                return false;
            }

            return true;
        }

        public override fsResult TrySerialize(object instance, out fsData serialized, Type storageType)
        {
            serialized = fsData.CreateDictionary();
            var result = fsResult.Success;

            fsMetaType metaType = fsMetaType.Get(Serializer.Config, instance.GetType());
            metaType.EmitAotData();

            for (int i = 0; i < metaType.Properties.Length; ++i)
            {
                fsMetaProperty property = metaType.Properties[i];
                if (property.CanRead == false) continue;
                //+根据字段的null属性来确定是否需要序列化出去
                //+把object序列化成原始值
                //+@Ben
                var obj = property.Read(instance);
                Type type = property.StorageType;
                if (fsPortableReflection.HasAttribute<fsSkipNullAttribute>(property.MemberInfo) && null == obj) continue;
                if (fsPortableReflection.HasAttribute<fsConvertObjectAttribute>(property.MemberInfo)) type = obj.GetType();

                fsData serializedData;
                var itemResult = Serializer.TrySerialize(type, property.OverrideConverterType, obj, out serializedData);
                result.AddMessages(itemResult);
                if (itemResult.Failed)
                {
                    continue;
                }

                serialized.AsDictionary[property.JsonName] = serializedData;
            }

            return result;
        }

        public override fsResult TryDeserialize(fsData data, ref object instance, Type storageType)
        {
            var result = fsResult.Success;

            // Verify that we actually have an Object
            if ((result += CheckType(data, fsDataType.Object)).Failed)
            {
                return result;
            }

            fsMetaType metaType = fsMetaType.Get(Serializer.Config, storageType);
            metaType.EmitAotData();

            for (int i = 0; i < metaType.Properties.Length; ++i)
            {
                fsMetaProperty property = metaType.Properties[i];
                if (property.CanWrite == false) continue;

                fsData propertyData;
                if (data.AsDictionary.TryGetValue(property.JsonName, out propertyData))
                {
                    object deserializedValue = null;

                    // We have to read in the existing value, since we need to support partial
                    // deserialization. However, this is bad for perf.
                    // TODO: Find a way to avoid this call when we are not doing a partial deserialization
                    //       Maybe through a new property, ie, Serializer.IsPartialSerialization, which just
                    //       gets set when starting a new serialization? We cannot pipe the information
                    //       through CreateInstance unfortunately.
                    if (property.CanRead)
                    {
                        deserializedValue = property.Read(instance);
                    }
                    //+把原始值反序列化成object，直接序列化会有类型转换异常（内部并没有抛出）
                    //+@Ben
                    Type type = property.StorageType;
                    fsConvertObjectAttribute convertAttr = fsPortableReflection.GetAttribute<fsConvertObjectAttribute>(property.MemberInfo);
                    if (null != convertAttr) type = convertAttr.ValueType;
                    var itemResult = Serializer.TryDeserialize(propertyData, type, property.OverrideConverterType, ref deserializedValue);
                    result.AddMessages(itemResult);
                    if (itemResult.Failed) continue;

                    property.Write(instance, deserializedValue);
                }
            }

            return result;
        }

        public override object CreateInstance(fsData data, Type storageType)
        {
            fsMetaType metaType = fsMetaType.Get(Serializer.Config, storageType);
            return metaType.CreateInstance();
        }
    }
}
#endif