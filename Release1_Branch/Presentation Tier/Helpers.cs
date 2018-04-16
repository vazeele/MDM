using SmartBiz.MDMAPI.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using Service = SmartBiz.MDM.Presentation.ServiceReference;
using System.Globalization;
using System.Collections.ObjectModel;
using System.Text;
using System.Diagnostics;
namespace SmartBiz.MDM.Presentation
{
    class Helper
    {
        public static bool IsValid(DependencyObject obj)
        {
            return !Validation.GetHasError(obj) &&
                LogicalTreeHelper.GetChildren(obj)
                .OfType<DependencyObject>()
                .All(IsValid);
        }
   
    
        public static Service.MdmServiceClient getServiceClient(){
              var client = new Service.MdmServiceClient();
              return client;
        }
        public static List<string> getItemSource(Type type, String PropertyName)
        {
            return (type.GetMember(PropertyName)[0].GetCustomAttribute(typeof(ValuesAttribute)) as ValuesAttribute).values;
        }
        public static string FormatHeaders(string text, bool preserveAcronyms)
        {
          
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                         i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');               

                newText.Append(text[i]);
            }
            newText.Replace('_', ' ');
            newText.Replace("Crd", "Credit");
            newText.Replace("Dbt", "Debit");
            newText.Replace("FIN", String.Empty);            
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string str_newText = textInfo.ToTitleCase(textInfo.ToLower(newText.ToString()));
       
            return str_newText;

        }
    }

    public class IndexConverter : IValueConverter
    {


        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null){
                if (value.GetType() == typeof(int))
                    return (int)value - 1;
                else if (value.GetType() == typeof(short))
                    return (short)value - 1;
                else
                    throw new InvalidCastException();
            }
            else
                return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType() == typeof(int))
                    return (int)value + 1;
                else if (value.GetType() == typeof(short))
                    return (short)value + 1;
                else
                    throw new InvalidCastException();
            }
            else
                return DependencyProperty.UnsetValue;
        }
    }
    public class AttributeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter as string != null)
            {
                var property = ((IEnumerable<object>)value).First().GetType().GetProperty((string)parameter);
                if (property != null)
                {
                    var attribute = property.GetCustomAttributes(typeof(ValuesAttribute), false).OfType<ValuesAttribute>().FirstOrDefault();
                    if (attribute != null)
                        return attribute.values.Select((display, index) =>
                            new KeyValuePair<int, string>(index, display)
                            ).ToArray();
                }
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class KeyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && parameter as string != null)
            {
                var property = ((IEnumerable<object>)value).First().GetType().GetProperty((string)parameter);
                if (property != null)
                {
                    return false;
                }
                else return true;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
  
}
