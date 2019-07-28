using UnityEngine;
using System.Collections;

namespace NodeCanvas.Variables{

	[AddComponentMenu("")]
	public class ComponentData : Data{

		public Component value;

		public override System.Type dataType{
			get {return typeof(Component);}
		}

		public override void SetValue(System.Object value){
			this.value = (Component)value;
		}

		public override System.Object GetValue(){
			return value;
		}

		public override System.Object GetSerialized(){

			if (value == null)
				return null;

			GameObject obj = value.gameObject;

			if (obj == null)
				return null;

			string path= "/" + obj.name;
			while (obj.transform.parent != null){
				obj = obj.transform.parent.gameObject;
				path = "/" + obj.name + path;
			}
			
			return new SerializedComponent(path, value.GetType());
		}

		public override void SetSerialized(System.Object obj){

			SerializedComponent serComponent = obj as SerializedComponent;
			if (obj == null){
				value = null;
				return;
			}

			GameObject go = GameObject.Find(serComponent.path);
			if (!go){
				Debug.LogWarning("ComponentData Failed to load. The component's gameobject was not found in the scene. Path '" + serComponent.path + "'");
				return;
			}

			value = go.GetComponent(serComponent.trueType);
			if (value == null)
				Debug.LogWarning("ComponentData Failed to load. GameObject was found but the component of type '" + serComponent.trueType.ToString() + "' itself was not. Path '" + serComponent.path + "'");
		}


		[System.Serializable]
		private class SerializedComponent{

			public string path;
			public System.Type trueType;

			public SerializedComponent(string path, System.Type type){
				this.path = path;
				this.trueType = type;
			}
		}


		//////////////////////////
		///////EDITOR/////////////
		//////////////////////////
		#if UNITY_EDITOR

		public override void ShowDataGUI(){

			base.ShowDataGUI();
			value = UnityEditor.EditorGUILayout.ObjectField(value, typeof(Component), true, GUILayout.MaxWidth(100), GUILayout.ExpandWidth(true)) as Component;
		}

		#endif
	}
}