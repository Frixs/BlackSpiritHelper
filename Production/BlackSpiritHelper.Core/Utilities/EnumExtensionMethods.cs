﻿using System;
using System.Linq;
using System.Reflection;

namespace BlackSpiritHelper.Core
{
    public static class EnumExtensionMethods
    {
        /// <summary>
        /// Get Description of the enum if exists.
        /// </summary>
        /// <param name="GenericEnum"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
    }
}
