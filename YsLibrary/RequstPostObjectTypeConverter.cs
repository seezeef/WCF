using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;
using System.Configuration;

namespace YsLibrary
{
    public class RequstPostObjectTypeConverter<T> : TypeConverter
    {
        public static string Token { get { return ConfigurationManager.AppSettings.Get("Token"); } }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ||
                base.CanConvertFrom(context, sourceType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {


            string strValue = value as string;

            if (strValue != null)
            {



                T requstObject = JsonConvert.DeserializeObject<T>(strValue);
                return requstObject;

            }

            return base.ConvertFrom(context, culture, value);


        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                T requstObject = (T)value;
                return requstObject;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }

}
