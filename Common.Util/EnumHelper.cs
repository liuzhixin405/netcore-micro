using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public static class EnumHelper
    {
        public static string GetDescription(this Enum enums)
        {
            FieldInfo field = enums.GetType().GetField(enums.ToString());

            if (field != null)
            {
                DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

                if (attribute != null)
                {
                    return attribute.Description;
                }
            }

            return enums.ToString(); // 如果没有找到描述，返回枚举成员的名称
        }
    }
}
