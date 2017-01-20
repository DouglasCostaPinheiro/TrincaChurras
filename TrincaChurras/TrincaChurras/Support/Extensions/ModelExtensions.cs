using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using TrincaChurras.Support.Attributes;
using TrincaChurras.Support.Models;

namespace TrincaChurras.Support.Extensions
{
    public static class ModelExtensions
    {
        public static string DataTablesSerialize<T>(this IEnumerable<T> pSource, int pDrawCount, Dictionary<string, string>[] order, ClientPagination pClientPagination, Dictionary<string, string> pModelSearch, ServerPagination pServerPagination = null, string[] pColumns = null)
        {
            DataTablesAjaxReturn result = new DataTablesAjaxReturn() {
                draw = ++pDrawCount
            };

            IEnumerable<string> gridColumns = null;
            IEnumerable<PropertyInfo> propertyColumns = null;
            IEnumerable<T> filteredSource = pSource;

            if (pColumns != null)
                gridColumns = pColumns;
            else
                if (Attribute.GetCustomAttribute(typeof(T), typeof(GridColumnOrderAttribute)) != null)
                gridColumns = ((GridColumnOrderAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(GridColumnOrderAttribute))).Order;
            else
                propertyColumns = typeof(T).GetProperties().Where(p => Attribute.GetCustomAttribute(p, typeof(GridIgnoreAttribute)) == null).OrderBy(p => p.MetadataToken);

            if (propertyColumns == null)
                propertyColumns = gridColumns.Select(gc => typeof(T).GetProperty(gc));

            result.recordsTotal = pServerPagination != null ? pServerPagination.RecordsTotal : pSource.Count();

            if (pModelSearch != null)
            {
                foreach (var modelField in pModelSearch.Where(m => !string.IsNullOrWhiteSpace(m.Value)))
                {
                    filteredSource = (filteredSource ?? pSource).Where(i => i.GetType().GetProperty(modelField.Key).Match(i, modelField.Value));
                }
            }

            result.recordsFiltered = filteredSource.Count();

            foreach (var orderConfig in order)
            {
                if (orderConfig["dir"] == "asc")
                    filteredSource = filteredSource.OrderBy(i => propertyColumns.ElementAt(int.Parse(orderConfig["column"])).GetValue(i));
                else
                    filteredSource = filteredSource.OrderByDescending(i => propertyColumns.ElementAt(int.Parse(orderConfig["column"])).GetValue(i));
            }

            if (pServerPagination == null)
                filteredSource = filteredSource.Skip(pClientPagination.start).Take(pClientPagination.length);

            foreach (var item in filteredSource)
            {
                var itemList = new List<object>();
                foreach (var col in propertyColumns)
                {
                    if (col.PropertyType == typeof(bool))
                        itemList.Add(string.Format("<span class='glyphicon glyphicon-thumbs-{0}'></span>", (bool)col.GetValue(item) ? "up" : "down"));
                    else
                        if (Attribute.GetCustomAttribute(col, typeof(DisplayFormatAttribute)) != null)
                            itemList.Add(string.Format(((DisplayFormatAttribute)Attribute.GetCustomAttribute(col, typeof(DisplayFormatAttribute))).DataFormatString, col.GetValue(item)));
                        else
                            itemList.Add(col.GetValue(item));
                }
                result.data.Add(itemList);
            }

            return JsonConvert.SerializeObject(result, new UserFriendlyDateTimeConverter());
        }

        private static bool Match(this PropertyInfo pPropertyInfo, object instance, object valueToMatch)
        {
            if (pPropertyInfo.PropertyType == typeof(DateTime))
            {
                var val = (DateTime)Convert.ChangeType(valueToMatch, typeof(DateTime));
                var propVal = (DateTime)pPropertyInfo.GetValue(instance);

                if (val.TimeOfDay.Milliseconds == 0)
                    return val.Date == propVal.Date;

                return val == propVal;
            }

            if (pPropertyInfo.PropertyType == typeof(string)) {
                return CultureInfo.InvariantCulture.CompareInfo.IndexOf((string)pPropertyInfo.GetValue(instance), (string)valueToMatch, CompareOptions.IgnoreCase) > -1;
            }

            return Convert.ChangeType(valueToMatch, pPropertyInfo.PropertyType).Equals(Convert.ChangeType(pPropertyInfo.GetValue(instance, null), pPropertyInfo.PropertyType));
        }
    }
}