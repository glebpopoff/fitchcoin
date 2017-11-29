﻿using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace FitchCoinEngine.Util
{
    public static class ObjectSerializer
    {
        public static string Serialize(this object obj)
        {
            var serializer = new DataContractJsonSerializer(obj.GetType());
            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }
    }
}
