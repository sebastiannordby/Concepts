using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace ChangeDetection
{
    public static class Extensions
    {
        public static string[] GetPropertyPath<T, K>(this Expression<Func<T, K>> expression)
        {
            MemberExpression me;
            var result = new List<string>();

            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    me = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expression.Body as MemberExpression;
                    break;
            }

            while (me != null)
            {
                string propertyName = me.Member.Name;
                Type propertyType = me.Type;

                result.Add(me.Member.Name);
                me = me.Expression as MemberExpression;
            }

            result.Reverse();

            return result.ToArray();
        }


        public static object GetPropertyValue(this object obj, string propertyPath)
        {
            object propertyValue = null;
            if (propertyPath.IndexOf(".") < 0)
            {
                return obj.GetType().GetProperty(propertyPath).GetValue(obj, null);
            }

            var properties = propertyPath.Split('.').ToList();
            var midPropertyValue = obj;
            while (properties.Count > 0)
            {
                var propertyName = properties.First();
                properties.Remove(propertyName);
                propertyValue = midPropertyValue.GetPropertyValue(propertyName);
                midPropertyValue = propertyValue;
            }
            return propertyValue;
        }

        public static void SetPropertyValue(this object obj, string[] propertyPath, object valueToSet)
        {
            object currObj = obj;
            PropertyInfo test = null;

            for (int i = 0; i < propertyPath.Length; i++)
            {
                var property = propertyPath[i];
                var currObjectType = currObj.GetType();

                if (i == propertyPath.Length - 1)
                {
                    test = currObj.GetType().GetProperty(property);
                }
                else
                {
                    currObj = currObj.GetType().GetProperty(property).GetValue(currObj);
                }
            }

            test.SetValue(currObj, valueToSet);
        }
    }
}
