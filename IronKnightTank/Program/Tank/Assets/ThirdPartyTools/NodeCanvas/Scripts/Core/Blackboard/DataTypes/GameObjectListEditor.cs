#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvasEditor{

	public class GameObjectListEditor : EditorWindow{

		public List<GameObject> targetList;
		public GameObject newGo;

		public void OnGUI(){

			for (int i = 0; i < targetList.Count; i++){

				GameObject go = targetList[i];
				GUILayout.BeginHorizontal("box");
				go = EditorGUILayout.ObjectField("GameObject", go, typeof(GameObject), true) as GameObject;
				if (GUILayout.Button("X", GUILayout.Width(18)))
					targetList.Remove(go);
				GUILayout.EndHorizontal();
			}

			if (targetList.Count != 0)
				NodeCanvas.EditorUtils.BoldSeparator();
			
			newGo = EditorGUILayout.ObjectField("Add GameObject", newGo, typeof(GameObject), true) as GameObject;
			if (newGo){
				targetList.Add(newGo);
				newGo = null;
			}
		}


		public static void Show(List<GameObject> list){
			GameObjectListEditor window= ScriptableObject.CreateInstance(typeof(GameObjectListEditor)) as GameObjectListEditor;
			window.targetList = list;
			window.ShowUtility();
		}
	}
}

#endif