// ----------------------------------------------------------------------------
// <author>HuHuiBin</author>
// <date>30/04/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Data
{
    using System;

    /// <summary>
    /// Content类型
    /// </summary>
    public class ContentType
    {
        public const string Any = "any";
        public const string Boolean = "boolean";
        public const string Number = "number";
        public const string String = "string";
        public const string Nil = "nil";

        public const string Action = "ScriptAction";
//        public const string ActionList = "ScriptAction[]";
        //        public const string VariableList = "VariableAction[]";


        public static bool IsVariable(string type)
        {
            return !IsList(type) && type != Action;
        }

        public static string List(string type)
        {
            return type + "[]";
        }

        public static string Unlist(string type)
        {
            if (!IsList(type))
            {
                return type;
            }
            return type.Substring(0, type.Length - 2);
        }


        public static bool IsList(string type)
        {
            return type != null && type.EndsWith("[]");
        }

        public static bool Is(string type, string target)
        {
            if (type == Any)
            {
                return true;
            }
            return type == target;
        }

        public static string GetName(string variableType)
        {
            switch (variableType)
            {
                case Any:
                    return "任何";
                case Boolean:
                    return "真值";
                case Number:
                    return "数字";
                case String:
                    return "字符";
                case Nil:
                    return "空值";
                case Action:
                    return "动作";
                    
            }
            if (IsList(variableType))
            {
                return GetName(Unlist(variableType)) + "列表";
            }
            return variableType;
        }

        public static string GetObjectType(object o)
        {
            if (o is string)
            {
                return String;
            }
            if (o is int)
            {
                return Number;
            }
            if (o is float)
            {
                return Number;
            }
            if (o is bool)
            {
                return Boolean;
            }
            return null;
        }

        public static object FormatValue(string type,string value)
        {
            if (type == ContentType.String)
            {
                return value;
            }
            if (type == ContentType.Boolean)
            {
                return value != null;
            }
            if (type == ContentType.Number)
            {
                try
                {
                    var single = Convert.ToSingle(value);
                    if (Math.Abs((int)single - single) < 0.000001)
                    {
                        return (int)single;
                    }
                    return single;
                }
                catch (Exception)
                {
                }
            }
            return null;
        }
    }
}