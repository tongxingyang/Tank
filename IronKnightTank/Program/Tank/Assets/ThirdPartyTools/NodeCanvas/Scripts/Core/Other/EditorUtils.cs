using System;

#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using NodeCanvas.Variables;

namespace NodeCanvas{

	//Have some commonly stuff used across most inspectors and helper functions. Keep outside of Editor folder since many runtime classes have editor GUIs in #if UNITY_EDITOR
	static public class EditorUtils{

		private static Texture2D tex;

		public static Color lightOrange = new Color(1, 0.9f, 0.4f);
		public static Color lightBlue   = new Color(0.8f,0.8f,1);
		public static Color lightRed    = new Color(1,0.5f,0.5f, 0.8f);

		//a cool label :-P
		public static void CoolLabel(string text){

			GUI.skin.label.richText = true;
			GUI.color = lightOrange;
			GUILayout.Label("<b><size=16>" + text + "</size></b>");
			GUI.color = Color.white;
		}

		//a thin separator
		public static void Separator(){
			
			GUI.backgroundColor = Color.black;
			GUILayout.Box("", GUILayout.MaxWidth(Screen.width), GUILayout.Height(2));
			GUI.backgroundColor = Color.white;
		}

		//A thick separator similar to ngui. Thanks
		public static void BoldSeparator(){

			if (tex == null)
				tex = new Texture2D(1,1);

			Rect lastRect= GUILayoutUtility.GetLastRect();

			GUILayout.Space(14);
			GUI.color = new Color(0, 0, 0, 0.25f);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 4), tex);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 1), tex);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 9, Screen.width, 1), tex);
			GUI.color = Color.white;
		}

		public static void TitledSeparator(string title){

			TitledSeparator(title, false);
		}

		//Combines the rest functions for a header style label
		public static void TitledSeparator(string title, bool startOfInspector){

			if (!startOfInspector)
				BoldSeparator();
			else
				EditorGUILayout.Space();

			CoolLabel(title + " ▼");
			Separator();
		}

		//Just a fancy ending for inspectors
		public static void EndOfInspector(){

			if (tex == null)
				tex = new Texture2D(1,1);
			
			Rect lastRect= GUILayoutUtility.GetLastRect();

			GUILayout.Space(8);
			GUI.color = new Color(0, 0, 0, 0.4f);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 4), tex);
			GUI.DrawTexture(new Rect(0, lastRect.yMax + 4, Screen.width, 1), tex);
			GUI.color = Color.white;
		}

		//a Custom titlebar for tasks. Returns if the task is folded or not
		public static bool TaskTitlebar(Task task){
			
			GUI.backgroundColor = new Color(0.8f,0.8f,1f,1f);
			GUILayout.BeginHorizontal("box");
			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("X", GUILayout.Width(18)))
				UnityEngine.Object.DestroyImmediate(task, true);

			GUILayout.Label("<b>" + (task.unfolded? "▼ " :"► ") + task.taskName + "</b>" + (task.unfolded? "" : "\n<i><size=10>(" + task.taskInfo + ")</size></i>") );
			GUILayout.EndHorizontal();

			Rect titleRect = GUILayoutUtility.GetLastRect();
			var e = Event.current;

			if (e.button == 0 && e.type == EventType.MouseDrag && titleRect.Contains(e.mousePosition)){
				DragAndDrop.PrepareStartDrag();
				DragAndDrop.objectReferences = new UnityEngine.Object[]{task};
				DragAndDrop.StartDrag("Custom Drag");
				e.Use();
			}

			if (e.type == EventType.MouseUp)
				DragAndDrop.PrepareStartDrag();

			if (e.type == EventType.ContextClick && titleRect.Contains(e.mousePosition)){
				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("Open Script"), false, delegate{AssetDatabase.OpenAsset(MonoScript.FromMonoBehaviour(task)) ;} );
				menu.AddItem(new GUIContent("Delete"), false, delegate{UnityEngine.Object.DestroyImmediate(task, true);} );
				menu.ShowAsContext();
				e.Use();
			}

			if (e.button == 0 && e.type == EventType.MouseUp && titleRect.Contains(e.mousePosition)){
				task.unfolded = !task.unfolded;
				e.Use();
			}

			return task.unfolded;
		}


		//Automatic editor gui for arbitrary objects
		public static void ShowAutoEditorGUI(object o){

			foreach (FieldInfo field in o.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)){

				if (field.GetCustomAttributes(typeof(HideInInspector), true ).FirstOrDefault() as HideInInspector != null)
					continue;

				if (field.GetCustomAttributes(typeof(BeginGroupAttribute), true ).FirstOrDefault() as BeginGroupAttribute != null)
					GUILayout.BeginVertical("box");

				field.SetValue(o, GenericField(field.Name, field.GetValue(o), field.FieldType, field) );
				GUI.backgroundColor = Color.white;

				if (field.GetCustomAttributes(typeof(EndGroupAttribute), true ).FirstOrDefault() as EndGroupAttribute != null)
					GUILayout.EndVertical();
			}

			foreach (PropertyInfo prop in o.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)){

				if (!prop.CanRead || !prop.CanWrite)
					continue;

				prop.SetValue(o, GenericField(prop.Name, prop.GetValue(o, null), prop.PropertyType, prop), null);
				GUI.backgroundColor = Color.white;				
			}
		}

		//For generic automatic editors. Passing a fieldinfo will also check for the attributes
		public static object GenericField(string name, object value, System.Type t, MemberInfo member = null){

			name = CamelCaseToWords(name);
			System.Type type = null;
			if (member != null)
				type = member.MemberType == MemberTypes.Field? (member as FieldInfo).FieldType : (member as PropertyInfo).PropertyType;

			if (member != null && member.GetCustomAttributes(typeof(RequiredFieldAttribute), true).FirstOrDefault() as RequiredFieldAttribute != null){
				if (type == typeof(string) && string.IsNullOrEmpty((string)value) ||
					type == typeof(BBString) && string.IsNullOrEmpty( (value as BBString).value ) ||
					typeof(BBValue).IsAssignableFrom(type) && (value as BBValue).isNull ||
					typeof(UnityEngine.Object).IsAssignableFrom(type) && (value as UnityEngine.Object) == null)
				{
					GUI.backgroundColor = lightRed;
				}
			} 


			if (t == typeof(string)){
				if (member != null){
					if (member.GetCustomAttributes(typeof(TagFieldAttribute), true).FirstOrDefault() as TagFieldAttribute != null)
						return EditorGUILayout.TagField(name, (string)value);
				}

				return EditorGUILayout.TextField(name, (string)value);
			}

			if (t == typeof(bool))
				return EditorGUILayout.Toggle(name, (bool)value);

			if (t == typeof(int)){
				if (member != null && member.GetCustomAttributes(typeof(LayerFieldAttribute), true).FirstOrDefault() as LayerFieldAttribute != null)
					return EditorGUILayout.LayerField(name, (int)value);

				return EditorGUILayout.IntField(name, (int)value);
			}

			if (t == typeof(float)){
				if (member != null){
					SliderFieldAttribute sField = member.GetCustomAttributes(typeof(SliderFieldAttribute), true).FirstOrDefault() as SliderFieldAttribute;
					if (sField != null)
						return EditorGUILayout.Slider(name, (float)value, sField.left, sField.right);
				}

				return EditorGUILayout.FloatField(name, (float)value);
			}

			if (t == typeof(Vector2))
				return EditorGUILayout.Vector2Field(name, (Vector2)value);

			if (t == typeof(Vector3))
				return EditorGUILayout.Vector3Field(name, (Vector3)value);

			if (t == typeof(Color))
				return EditorGUILayout.ColorField(name, (Color)value);

			if (typeof(System.Enum).IsAssignableFrom(t))
				return EditorGUILayout.EnumPopup(name, (System.Enum)value);

			if (t == typeof(Rect))
				return EditorGUILayout.RectField(name, (Rect)value);

			if (typeof(BBValue).IsAssignableFrom(t))
				return BBValueField(name, (BBValue)value);

			if (t == typeof(Component))
				return ComponentField(name, (Component)value);

			if (typeof(UnityEngine.Object).IsAssignableFrom(t))
				return EditorGUILayout.ObjectField(name, (UnityEngine.Object)value, t, true);

			if (t == typeof(List<GameObject>)){
				GUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel(name);
				if (GUILayout.Button( (value as List<GameObject>).Count.ToString() + " GameObject(s)") )
					NodeCanvasEditor.GameObjectListEditor.Show((List<GameObject>)value);
				GUILayout.EndHorizontal();
				return value;
			}

			if (t == typeof(AnimationCurve))
				return EditorGUILayout.CurveField(name, (AnimationCurve)value);


			GUILayout.Label("Field '" + name + "' Type '" + TypeName(t) + "' not yet supported for Auto Editor");
			return value;
		}

		//Convert camelCase to words as the name implies.
		public static string CamelCaseToWords(string s){

			s = s.Substring(0, 1).ToUpper() + s.Substring(1);
			return Regex.Replace(s, @"(\B[A-Z])", @" $1");
		}

		//a special object field for the BBValue class to let user choose either a real value or enter a string to read data from a Blackboard
		public static BBValue BBValueField(string prefix, BBValue bbValue){

			if (bbValue == null){
				EditorGUILayout.LabelField(prefix, "NULL");
				return null;
			}

			bool isContained = false;

			EditorGUILayout.BeginHorizontal();
			if (!bbValue.blackboardOnly && !bbValue.useBlackboard){

				FieldInfo field = bbValue.GetType().GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic);
				field.SetValue(bbValue, GenericField(prefix, field.GetValue(bbValue), field.FieldType, field) );

			} else {

				GUI.color = new Color(0.9f,0.9f,1f,1f);
				if (bbValue.bb){

					List<string> dataNames = bbValue.bb.RetrieveDataNames(bbValue.dataType).ToList();

					if (dataNames.Contains(bbValue.dataName) || string.IsNullOrEmpty(bbValue.dataName) ){

						GUI.backgroundColor = new Color(0.95f,0.95f,1f,1f);
						bbValue.dataName = StringPopup(prefix, bbValue.dataName, dataNames, false, true);
						isContained = true;

					} else {

						GUI.backgroundColor = new Color(1f,0.7f,0.7f,1f);
						bbValue.dataName = EditorGUILayout.TextField(prefix + " (" + TypeName(bbValue.dataType) + ")", bbValue.dataName);
					}	

				} else {

					bbValue.dataName = EditorGUILayout.TextField(prefix + " (" + TypeName(bbValue.dataType) + ")", bbValue.dataName);
				}

			}

			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;

			if (!bbValue.blackboardOnly)
				bbValue.useBlackboard = EditorGUILayout.Toggle(bbValue.useBlackboard, EditorStyles.radioButton, GUILayout.Width(18));

			EditorGUILayout.EndHorizontal();
		
			if (bbValue.bb && bbValue.useBlackboard && !bbValue.blackboardOnly){	
				GUI.backgroundColor = new Color(0.8f,0.8f,1f,0.5f);
				GUI.color = new Color(1f,1f,1f,0.5f);
				GUILayout.BeginHorizontal("textfield");

				if (string.IsNullOrEmpty(bbValue.dataName)){
					GUILayout.Label("Select a '" + TypeName(bbValue.dataType) + "' Blackboard Variable");
				} else if (isContained){
					GUILayout.Label("<i>" + bbValue.dataName + " = " + (bbValue.objectValue != null? bbValue.objectValue.ToString() : "NULL") + "</i>");
				} else {
					GUILayout.Label("Variable name does not exist in blackboard");
				}
				
				GUILayout.EndHorizontal();
				GUILayout.Space(2);
				GUI.backgroundColor = Color.white;
				GUI.color = Color.white;
			}

			return bbValue;
		}


		public static bool BooleanField(string prefix, bool value){

			GUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel(prefix);
			if (GUILayout.Button(value? "True" : "False", GUILayout.Height(15)))
				value = !value;
			GUILayout.EndHorizontal();
			return value;
		}

		public static Component ComponentField(Component comp){
			return ComponentField(string.Empty, comp);
		}

		//an editor field where if the component is null simply shows an object field, but if its not, shows a dropdown popup to select the specific component
		//from within the gameobject
		public static Component ComponentField(string prefix, Component comp, bool allowNone = true){

			if (!comp){

				if (prefix != string.Empty){

					comp = EditorGUILayout.ObjectField(prefix, comp, typeof(Component), true, GUILayout.ExpandWidth(true)) as Component;

				} else {

					comp = EditorGUILayout.ObjectField(comp, typeof(Component), true, GUILayout.ExpandWidth(true)) as Component;
				}

				return comp;
			}

			List<Component> allComp = new List<Component>(comp.GetComponents(typeof(Component)));
			List<string> compNames = new List<string>();

			foreach (Component c in allComp.ToArray()){
				
				if (c == null)
					continue;

				if (c.hideFlags == HideFlags.HideInInspector){
					allComp.Remove(c);
					continue;
				}

				compNames.Add(TypeName(c.GetType()) + " @ " + c.gameObject.name);
			}

			if (allowNone)
				compNames.Add("|NONE|");

			int index;
			if (prefix != string.Empty)
				index = EditorGUILayout.Popup(prefix, allComp.IndexOf(comp), compNames.ToArray(), GUILayout.ExpandWidth(true));
			else
				index = EditorGUILayout.Popup(allComp.IndexOf(comp), compNames.ToArray(), GUILayout.ExpandWidth(true));
			
			if (allowNone && index == compNames.Count - 1)
				return null;

			return allComp[index];
		}


		public static string StringPopup(string selected, List<string> options, bool showWarning = true, bool allowNone = false){
			return StringPopup(string.Empty, selected, options, showWarning, allowNone);
		}

		//a popup that it's based on the string rather than the index
		public static string StringPopup(string prefix, string selected, List<string> options, bool showWarning = true, bool allowNone = false){

			EditorGUILayout.BeginVertical();
			if (options.Count == 0 && showWarning){
				EditorGUILayout.HelpBox("There are no options to select for '" + prefix + "'", MessageType.Warning);
				EditorGUILayout.EndVertical();
				return null;
			}

			if (allowNone)
				options.Insert(0, "|NONE|");

			int index;

			if (options.Contains(selected))	index = options.IndexOf(selected);
			else index = allowNone? 0 : -1;

			if (!string.IsNullOrEmpty(prefix)) index = EditorGUILayout.Popup(prefix, index, options.ToArray());
			else index = EditorGUILayout.Popup(index, options.ToArray());

			if (index == -1 || (allowNone && index == 0)){

				if (showWarning){
					if (!string.IsNullOrEmpty(selected))
						EditorGUILayout.HelpBox("The previous selection '" + selected + "' has been deleted or changed. Please select another", MessageType.Warning);
					else
						EditorGUILayout.HelpBox("Please make a selection", MessageType.Warning);
				}
			}

			EditorGUILayout.EndVertical();
			if (allowNone)
				return index == 0? string.Empty : options[index];

			return index == -1? string.Empty : options[index];
		}

		//Used from task to show up the selection of the agent
		public static bool AgentField(ref Component agent, Type type, Component ownerAgent, bool isOverride){

			EditorGUILayout.BeginHorizontal("Box");
			GUI.color = new Color(1f,1f,1f, isOverride? 1f : 0.5f);

			if (isOverride){

				if (type == typeof(Component))
					agent = ComponentField("Override Agent", agent);
				else
					agent = EditorGUILayout.ObjectField("Override Agent", agent, type, true) as Component;
			
			} else {

				bool isMissing = (ownerAgent != null && ownerAgent.GetComponent(type) == null);
				string typeString = isMissing? "<color=#ff5f5f>" + TypeName(type) + "</color>": TypeName(type);

				GUILayout.Label("Owner Agent (" + typeString + ")", GUILayout.Height(15));
				GUI.color = new Color(1f,1f,1f,1f);
				agent = EditorGUILayout.ObjectField(agent, type, true, GUILayout.Width(22)) as Component;
			}

			GUI.backgroundColor = isOverride? new Color(1, 0.5f, 0.5f):Color.white;
			GUI.color = new Color(1f,1f,1f,1f);
			
			if (isOverride && GUILayout.Button("X", GUILayout.Width(25), GUILayout.Height(15))){
				agent = null;
				isOverride = false;
			}

			GUI.backgroundColor = Color.white;
			GUI.color = Color.white;
			EditorGUILayout.EndHorizontal();

			return isOverride;
		}


		//Shows a button that when click pops a context menu witha list of components deriving the type specified. When something is selected the callback is called
		//Passing null target gameObject creates a new one
		public static void ShowComponentSelectionButton(GameObject target, Type ofType, Action<Component> callback, bool hideComponent = true){

			GUI.backgroundColor = lightBlue;

			if (GUILayout.Button("Add " + TypeName(ofType))){

				Action<Type> ContextAction = delegate (Type script){

					if (!target)
						target = new GameObject(ofType.ToString());

					var newScript = target.AddComponent(script);
					if (hideComponent)
						newScript.hideFlags = HideFlags.HideInInspector;
					
					callback(newScript);
				};

				ShowComponentSelectionMenu(ofType, ContextAction);
			}

			GUI.backgroundColor = Color.white;
		}

		//shows a context menu with a list of components of specified type directly
		public static void ShowComponentSelectionMenu(Type ofType, Action<Type> callback){

				GenericMenu.MenuFunction2 Selected = delegate(object selectedType){
					callback((Type)selectedType);
				};

				GenericMenu menu= new GenericMenu();

				var scriptInfos = GetAllScriptsOfTypeCategorized(ofType);

				foreach (ScriptInfo script in scriptInfos){
					if (string.IsNullOrEmpty(script.category))
						menu.AddItem(new GUIContent(script.name), false, Selected, script.type);
				}

				menu.AddSeparator("/");

				foreach (ScriptInfo script in scriptInfos){
					if (!string.IsNullOrEmpty(script.category))
						menu.AddItem(new GUIContent( script.category + "/" + script.name), false, Selected, script.type);
				}

				menu.ShowAsContext();
		}


		//Get all scripts of a type excluding the base type and those with the Exclude attribute, from within the project categorized as a list of ScriptInfo
		public static List<ScriptInfo> GetAllScriptsOfTypeCategorized(Type baseType){

			if (!typeof(Component).IsAssignableFrom(baseType)){
				Debug.LogError("Can't Get Scripts of type which do not derive from Component type");
				return null;
			}

			List<MonoScript> allScripts = new List<MonoScript>();
			List<ScriptInfo> allRequestedScripts = new List<ScriptInfo>();

			foreach (string path in AssetDatabase.GetAllAssetPaths()){

				if (path.EndsWith(".js") || path.EndsWith(".cs") || path.EndsWith(".boo"))
					allScripts.Add(AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript)) as MonoScript);
			}

			foreach (MonoScript monoScript in allScripts){

				if (monoScript == null)
					continue;
				
				Type subType= monoScript.GetClass();
				if (baseType.IsAssignableFrom(subType) && subType.GetCustomAttributes(typeof(ExcludeFromGettingAttribute), false).FirstOrDefault() as ExcludeFromGettingAttribute == null){

					if (subType == baseType)
						continue;

					if (subType.IsAbstract)
						continue;
				
					string scriptName = CamelCaseToWords( TypeName(subType) );
					string scriptCategory = string.Empty;

					var nameAttribute = subType.GetCustomAttributes(typeof(ScriptNameAttribute), false).FirstOrDefault() as ScriptNameAttribute;
					if (nameAttribute != null)
						scriptName = nameAttribute.name;

					var categoryAttribute = subType.GetCustomAttributes(typeof(ScriptCategoryAttribute), true).FirstOrDefault() as ScriptCategoryAttribute;
					if (categoryAttribute != null)
						scriptCategory = categoryAttribute.category;

					allRequestedScripts.Add(new ScriptInfo(subType, scriptName, scriptCategory));
				}
			}

			allRequestedScripts = allRequestedScripts.OrderBy(script => script.name).ToList();
			allRequestedScripts = allRequestedScripts.OrderBy(script => script.category).ToList();

			return allRequestedScripts;
		}

		//Get non categorized list of types deriving base type provided
		public static List<System.Type> GetAllScriptsOfType(System.Type type){
			
			List<System.Type> scripts = new List<System.Type>();
			List<ScriptInfo> categorizedScripts = GetAllScriptsOfTypeCategorized(type);
			categorizedScripts = categorizedScripts.OrderBy(script => script.name).ToList();

			foreach (ScriptInfo sInfo in categorizedScripts)
				scripts.Add(sInfo.type);

			return scripts;
		}


		//get the right text for a type name
		public static string TypeName(Type t){

			string s = t.Name;
			
			if (s == "Single") s = "Float";
			if (s == "Int32") s = "Int";

			if (t.IsGenericParameter)
				s = "T";

			if (t.IsGenericType){
				
				Type[] args= t.GetGenericArguments();
				
				if (args.Length != 0){
				
					s = s.Replace("`" + args.Length.ToString(), "");

					s += "<";
					for (int i= 0; i < args.Length; i++)
						s += (i == 0? "":", ") + TypeName(args[i]);
					s += ">";
				}
			}

			return Strip(s, ".");
		}

		//strips anything before the specified character. Mostly use to strip namespaces
		public static string Strip(string text, string before){

			int index= text.LastIndexOf(before);
			if (index >= 0)
				return text.Substring(index + 1);
			return text;
		}

		//Get the names of all available methods on a type
		public static List<string> GetAvailableMethods(System.Type type, System.Type paramType, System.Type returnType, bool propertiesOnly = false, bool includeCastable = false){

			//search methods for selected
			List<MethodInfo> methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public).ToList();
			List<string> methodNames = new List<string>();
			ParameterInfo[] parameters;

			foreach (MethodInfo method in methods){

				if (propertiesOnly && !method.IsSpecialName)
					continue;

				if (method.IsGenericMethod)
					continue;

				if (returnType == null || !returnType.IsAssignableFrom(method.ReturnType))
					continue;

				parameters = method.GetParameters();
				if (parameters.Length > 1)
					continue;

				if (parameters.Length == 0 && paramType != null)
					continue;

				if (parameters.Length != 0 && paramType == null)
					continue;

				if (includeCastable){

					if (parameters.Length != 0 && !CanConvert(paramType, parameters[0].ParameterType) ){
						continue;
					}

				} else {

					if (parameters.Length != 0 && !paramType.IsAssignableFrom(parameters[0].ParameterType) ){
						continue;
					}					
				}

				methodNames.Add(method.Name);
			}

			return methodNames;
		}

		public static List<string> GetAvailableEvents(System.Type type){

			var eventNames = new List<string>();
			foreach(EventInfo e in type.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){

				var m = e.EventHandlerType.GetMethod("Invoke");

				if (m.GetParameters().Length != 0)
					continue;

				if (m.ReturnType != typeof(void) )
					continue;

				eventNames.Add(e.Name);
			}
			return eventNames;
		}

		//Determines if a type can be casted to another
		public static bool CanConvert(Type fromType, Type toType) {
		    try
		    {
		        Expression.Convert(Expression.Parameter(fromType, null), toType);
		        return true;
		    }
		    catch
		    {
		        return false;
		    }
		}

		//all scene names (added in build settings)
		public static List<string> GetSceneNames(){

			List<string> allSceneNames = new List<string>();

			foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes){

				if (scene.enabled){

					string name= scene.path.Substring(scene.path.LastIndexOf("/") + 1);
					name = name.Substring(0,name.Length-6);
					allSceneNames.Add(name);
				}
			}

			return allSceneNames;
		}


	    public static void CreateAsset(Type o){

	        ScriptableObject asset= ScriptableObject.CreateInstance(o);
	        if (!asset){
	        	Debug.LogWarning("NULL");
	        	return;
	        }
	        string path= AssetDatabase.GetAssetPath (Selection.activeObject);
	        if (path == "")
	        {
	            path = "Assets";
	        }
	        else if (Path.GetExtension (path) != "")
	        {
	            path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), "");
	        }

	        string assetPathAndName= AssetDatabase.GenerateUniqueAssetPath (path + "/New " + o.ToString() + ".asset");

	        AssetDatabase.CreateAsset (asset, assetPathAndName);

	        AssetDatabase.SaveAssets ();
	        EditorUtility.FocusProjectWindow ();
	        Selection.activeObject = asset;
	    }


		//for when getting scripts
		public class ScriptInfo{

			public Type type;
			public string name;
			public string category;

			public ScriptInfo(Type type, string name, string category){
				this.type = type;
				this.name = name;
				this.category = category;
			}
		}
	}
}

#endif



namespace NodeCanvas{

	//To exclude a class from when GetAllScriptsOfType
	[AttributeUsage(AttributeTargets.Class)]
	public class ExcludeFromGettingAttribute : Attribute{

	}

	//For friendly names
	[AttributeUsage(AttributeTargets.Class)]
	public class ScriptNameAttribute : Attribute{

		public string name;

		public ScriptNameAttribute(string name){

			this.name = name;
		}
	}

	//To categorize scripts
	[AttributeUsage(AttributeTargets.Class)]
	public class ScriptCategoryAttribute : Attribute{

		public string category;

		public ScriptCategoryAttribute(string category){

			this.category = category;
		}
	}


	//makes the int field show as layerfield
	[AttributeUsage(AttributeTargets.Field)]
	public class LayerFieldAttribute : Attribute{

	}

	//makes the string field show as tagfield
	[AttributeUsage(AttributeTargets.Field)]
	public class TagFieldAttribute : Attribute{

	}

	//makes the float field show as slider
	[AttributeUsage(AttributeTargets.Field)]
	public class SliderFieldAttribute : Attribute{

		public float left;
		public float right;

		public SliderFieldAttribute(float left, float right){
			this.left = left;
			this.right = right;
		}
	}

	//Helper attribute. Designates that the field is required not to be null or string.empty
	[AttributeUsage(AttributeTargets.Field)]
	public class RequiredFieldAttribute : Attribute{

	}

	//When auto editor layouting, starts a vertical box group
	[AttributeUsage(AttributeTargets.Field)]
	public class BeginGroupAttribute : Attribute{

	}

	//When auto editor layouting, ends a vertical box group
	[AttributeUsage(AttributeTargets.Field)]
	public class EndGroupAttribute : Attribute{

	}
}