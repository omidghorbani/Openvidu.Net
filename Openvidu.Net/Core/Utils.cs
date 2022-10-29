using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Openvidu.Net.Core
{
    public static class Utils
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
    }
}
