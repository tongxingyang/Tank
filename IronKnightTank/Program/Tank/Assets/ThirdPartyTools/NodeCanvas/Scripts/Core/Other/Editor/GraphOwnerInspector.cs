using UnityEditor;
using UnityEngine;
using System.Collections;
using NodeCanvas;

namespace NodeCanvasEditor{

	public class GraphOwnerInspector : Editor {

		GraphOwner owner{
			get{return target as GraphOwner;}
		}

		void OnDestroy(){
			if (owner == null){
				if (owner.graph != null && EditorUtility.DisplayDialog("Removing Owner...", "Do you also want to delete the Owner's assigned Graph?", "DO IT", "Keep it")){
					DestroyImmediate(owner.graph.gameObject);
				}
			}
		}

		public override void OnInspectorGUI(){

			if (owner.graph == null){
				
				EditorGUILayout.HelpBox("Owner Needs graph. Assign or Create a new one", MessageType.Info);
				if (GUILayout.Button("CREATE NEW")){
				
					if (owner.graph == null){
						owner.graph = new GameObject("Behaviour").AddComponent(owner.graphType) as NodeGraphContainer;
						owner.graph.transform.parent = owner.transform;
						owner.graph.transform.localPosition = Vector3.zero;
					}

					owner.graph.agent = owner;
				}

				OnSpecifics();
				return;
			}

			GUILayout.Space(10);

			owner.graph.graphName = EditorGUILayout.TextField("Graph Name", owner.graph.graphName);
			owner.graph.graphComments = GUILayout.TextArea(owner.graph.graphComments, GUILayout.Height(50));

			GUILayout.BeginHorizontal();

			GUI.backgroundColor = EditorUtils.lightBlue;
			if (GUILayout.Button("OPEN"))
				NodeGraphEditor.OpenWindow(owner.graph, owner, owner.blackboard);
		
			GUI.backgroundColor = Color.white;
			if (GUILayout.Button("S", GUILayout.Width(20)))
				Selection.activeObject = owner.graph;

			GUI.backgroundColor = EditorUtils.lightRed;
			if (GUILayout.Button("X", GUILayout.Width(20))){
				if (EditorUtility.DisplayDialog("Remove Assignment", "Delete assigned graph as well?", "DO IT!", "Keep it!"))
					DestroyImmediate(owner.graph.gameObject);
				else
					owner.graph = null;
			}

			GUILayout.EndHorizontal();
			GUI.backgroundColor = Color.white;

			owner.blackboard = (Blackboard)EditorGUILayout.ObjectField("Blackboard", owner.blackboard, typeof(Blackboard), true);
			owner.executeOnStart = EditorGUILayout.Toggle("Execute On Start", owner.executeOnStart);

			OnExtraOptions();

			EditorUtils.EndOfInspector();

			if (GUI.changed)
				EditorUtility.SetDirty(owner);

		}

		virtual protected void OnSpecifics(){

		}

		virtual protected void OnExtraOptions(){
			
		}
	}
}