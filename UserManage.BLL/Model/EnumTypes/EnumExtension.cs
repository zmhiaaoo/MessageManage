using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace MessageManage.BLL
{
    public static class EnumExtension
    {
        
        public static string GetDisplayName (this System.Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] menInfo = type.GetMember(en.ToString());
            if (menInfo!=null&&menInfo.Length>0)
            {
                object[] attrs = menInfo[0].GetCustomAttributes(typeof(DisplayAttribute), true);
                if (attrs!=null&&attrs.Length>0)
                {
                    return ((DisplayAttribute)attrs[0]).Name;
                }
            }
            return en.ToString();
        }
    }
}
