using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Assets.Tools.Script.Debug.Console;
using Assets.Tools.Script.Reflec;
using UnityEngine;

namespace Assets.Extends.EXTools.Debug.Console
{
    using Debug = UnityEngine.Debug;

    public class ObjectAnalyseDisplayer : IObjectAnalyseDisplayer
    {
        //work for
        private FullDebugConsole _debugConsole;
        //保存之前显示的，现在暂时不显示，以后可以退回去的内容
        private Stack<object> _objectList = new Stack<object>();
        private Stack<string> _objectNames = new Stack<string>();
        //当前显示的内容
        private object _currObj = null;
        private string _currName = null;
        //视图用
        private GUIStyle _btnGuiStyle = null;

        protected GUIStyle BtnGuiStyle
        {
            get
            {
                if (_btnGuiStyle == null)
                {
                    _btnGuiStyle = new GUIStyle("button")
                    {
                        normal = { textColor = Color.white },
                        alignment = TextAnchor.MiddleLeft
                    };
                }
                _btnGuiStyle.fontSize = Pixel(20);
                return _btnGuiStyle;
            }
        }

        private List<IObjectDebugAnalyse> _objectAnalyses = new List<IObjectDebugAnalyse>(); 

        public ObjectAnalyseDisplayer(FullDebugConsole debugConsole)
        {
            _debugConsole = debugConsole;
        }

        public void Show()
        {
            OnShowObject(_currObj, _currName);
        }
        public void ShowNewObject(object obj, string objName)
        {
            _objectList.Clear();
            _objectNames.Clear();
            OnShowObject(obj, objName);
        }

        public void RegisterObjectAnalyse(IObjectDebugAnalyse debugAnalyse)
        {
            _objectAnalyses.Add(debugAnalyse);
        }

        private void ShowObjectOnBack(object obj, string objName)
        {
            OnShowObject(obj, objName);
        }

        public void ShowObjectChild(object obj, string objName)
        {
            _objectList.Push(_currObj);
            _objectNames.Push(_currName);
            OnShowObject(obj, objName);
        }
        //---------------------------------------------show object-----------------------------------------------------------//
        /// <summary>
        /// 显示一个Object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objName"></param>
        public void OnShowObject(object obj, string objName)
        {
            GUILayoutBtn(String.Format("==={0}===", objName));
            _currObj = obj;
            _currName = objName;
            for (int i = 0; i < this._objectAnalyses.Count; i++)
            {
                var objectAnalysis = this._objectAnalyses[i];
                if (objectAnalysis.IsActiveBy(obj))
                {
                    objectAnalysis.Show(obj, objName,this);
                    return;
                }
            }
            if (IsBasicType(obj))
            {
                ShowSimpleObject(obj, objName);
            }
            else if (obj is IEnumerable)
            {
                ShowEnumerableObject(obj, objName);
            }
            else if (obj is SimpleMethodReflect)
            {
                ShowMethodObject(obj as SimpleMethodReflect, objName);
            }
            else if (obj is GameObject)
            {
                ShowGameObject(obj as GameObject, objName);
            }
            else
            {
                ShowClassObject(obj, objName);
            }
        }

        private void ShowGameObject(GameObject obj, string objName)
        {
            GUILayoutBtn("---Components---");
            foreach (var component in obj.GetComponents<UnityEngine.Component>())
            {
                ShowClassTypeProperty(component, objName, component.GetType().Name);
            }
            GUILayoutBtn("---object---");
            ShowClassObject(obj, objName);
        }

        /// <summary>
        /// 显示可Enumerable类型的Object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objName"></param>
        private void ShowEnumerableObject(object obj, string objName)
        {
            GUILayoutBtn("---IEnumerable---");
            if (obj is List<NameableObject>)
            {
                var nameableObjects = obj as List<NameableObject>;
                foreach (var nameableObject in nameableObjects)
                {
                    ShowProperty(nameableObject.Value, objName, nameableObject.Name);
                }
            }
            else if (obj is IDictionary)
            {
                IDictionary id = obj as IDictionary;
                foreach (var key in id.Keys)
                {
                    object value = id[key];
                    if (key != null)
                    {
                        ShowProperty(key, objName, "key");
                    }
                    else
                    {
                        ShowProperty("null", objName, "key");
                    }
                    if (value != null)
                    {
                        ShowProperty(value, objName, "value");
                    }
                    else
                    {
                        ShowProperty("null", objName, "value");
                    }
                }
            }
            else
            {
                IEnumerable iv = obj as IEnumerable;
                foreach (var element in iv)
                {
                    if (element != null)
                    {
                        ShowProperty(element, objName, "element");
                    }
                    else
                    {
                        ShowProperty("null", objName, "element");
                    }
                }
            }
            GUILayoutBtn("---object---");
            ShowClassObject(obj, objName);
        }
        /// <summary>
        /// 显示简单类型object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objName"></param>
        private void ShowSimpleObject(object obj, string objName)
        {
            ShowBasicTypeProperty(obj, objName, objName);
        }
        /// <summary>
        /// 显示一般的复杂类型object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="objName"></param>
        private void ShowClassObject(object obj, string objName)
        {
            try
            {
                Type t = obj.GetType();
                foreach (PropertyInfo pi in t.GetProperties())
                {
                    try
                    {
                        ShowProperty(pi.GetValue(obj, null), objName, pi.Name);
                    }
                    catch (Exception)
                    {
                    }

                }
                foreach (FieldInfo fieldInfo in t.GetFields())
                {
                    try
                    {
                        ShowProperty(fieldInfo.GetValue(obj), objName, fieldInfo.Name);
                    }
                    catch (Exception)
                    {
                    }
                }
                //------------------------显示空方法-----------------------//
                MethodInfo[] methodInfos = t.GetMethods();
                foreach (var methodInfo in methodInfos)
                {
                    try
                    {
                        ShowMethodProperty(obj, objName, methodInfo);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception e)
            {
                GUILayoutBtn("Exception,at show class");
            }
        }
        /// <summary>
        /// 显示SimpleMethodReflect类型的Object
        /// </summary>
        /// <param name="debugMethodReflect"></param>
        /// <param name="objName"></param>
        private void ShowMethodObject(SimpleMethodReflect debugMethodReflect, string objName)
        {
            for (int i = 0; i < debugMethodReflect.parameters.Count; i++)
            {
                debugMethodReflect.parameters[i] = _debugConsole.GUILayoutTextField(debugMethodReflect.parameters[i]);
            }
            if (GUILayoutBtn("Call " + debugMethodReflect.method.Name))
            {
                object call = debugMethodReflect.Call();
                ShowObjectChild(call, objName + " return");
            }
        }
        //---------------------------------------------show property-----------------------------------------------------------//
        /// <summary>
        /// 显示对象的方法
        /// </summary>
        /// <param name="v"></param>
        /// <param name="methodInfo"></param>
        public void ShowMethodProperty(object v, string objName, MethodInfo methodInfo)
        {
            try
            {
                if (!methodInfo.Name.StartsWith("get_"))
                {
                    if (SimpleMethodReflect.IsSimpleMethod(methodInfo, v))
                    {
                        if (GUILayoutBtn("Method --> " + methodInfo.Name))
                        {
                            ShowObjectChild(new SimpleMethodReflect(methodInfo, v), String.Format("{0}.{1}",objName , methodInfo.Name));
                        }
                    }
                }
            }
            catch (Exception)
            {
                GUILayoutBtn("Exception,at show method");
            }

        }
        /// <summary>
        /// 显示对象的属性
        /// </summary>
        /// <param name="v"></param>
        /// <param name="propertyName"></param>
        public void ShowProperty(object v, string objName, string propertyName)
        {
            try
            {
                if (IsBasicType(v))
                {
                    ShowBasicTypeProperty(v, objName, propertyName);
                }
                else if (v is Transform)
                {
                    ShowTransformTypeProperty(v as Transform, objName, propertyName);
                }
                else
                {
                    ShowClassTypeProperty(v, objName, propertyName);
                }
            }
            catch
            {
                GUILayoutBtn("Exception,at show property");
            }
        }

        /// <summary>
        /// 显示对象的复杂类型属性
        /// </summary>
        /// <param name="v"></param>
        /// <param name="propertyName"></param>
        private void ShowClassTypeProperty(object v, string objName, string propertyName)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("{0}  {1}", v.GetType().Name, propertyName);
            if (GUILayoutBtn(s.ToString()))
            {
                ShowObjectChild(v, String.Format("{0}.{1}", objName, propertyName));
            }
        }
        /// <summary>
        /// 显示对象的简单类型属性
        /// </summary>
        /// <param name="v"></param>
        /// <param name="propertyName"></param>
        private void ShowBasicTypeProperty(object v, object objName, string propertyName)
        {
            try
            {
                StringBuilder s = new StringBuilder();
                if (v == null)
                {
                    s.AppendFormat("{0}  {1}", propertyName, "null");
                }
                else if (v is Type)
                {
                    s.AppendFormat("{0}  {1}  {2}", "Type", propertyName, v);
                }
                else
                {
                    Type type = v.GetType();
                    if (type.IsEnum)
                    {
                        s.AppendFormat("{0}  {1}  {2}  {3}", "Enum", type.Name, propertyName, v);
                    }
                    else if (type.IsValueType || type == typeof(string))
                    {
                        s.AppendFormat("{0}  {1}  {2}", type.Name, propertyName, v);
                    }
                    else
                    {
                        s.AppendFormat("{0}  {1}  {2}", type.Name, propertyName, v);
                    }
                }
                if (GUILayoutBtn(s.ToString()))
                {
                    Debug.Log(s.ToString());
                }
            }
            catch
            {
                GUILayoutBtn("Exception,at show property");
            }
        }

        private void ShowTransformTypeProperty(Transform v, string objName, string propertyName)
        {
            StringBuilder s = new StringBuilder();
            s.AppendFormat("{0}  {1}  {2}  {3}", "Transform", propertyName, v.gameObject.name, v.gameObject.activeSelf ? "activeTrue" : "activeFalse");
            if (GUILayoutBtn(s.ToString()))
            {
                ShowObjectChild(v, objName + "." + propertyName);
            }
        }


        /// <summary>
        /// 对象是简单类型
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsBasicType(object v)
        {
            if (v == null) return true;
            Type type = v.GetType();
            return (type.IsValueType && !(v is Vector3) && !(v is Vector2) && !(v is Vector4) && !(v is Quaternion)) ||
                type.IsEnum ||
                v is string ||
                v is Type;
        }
        //----------------------------------------------------------------------------------------------------------
        public bool GUILayoutBtn(string str)
        {
            return GUILayout.Button(str, BtnGuiStyle, GUILayout.Height(Pixel(30)));
        }

        public bool Back()
        {
            if (_objectList.Count == 0) return true;
            ShowObjectOnBack(_objectList.Pop(), _objectNames.Pop());
            return false;
        }

        public int Pixel(int value)
        {
            return _debugConsole.GetPixelValue(value);
        }
    }
}