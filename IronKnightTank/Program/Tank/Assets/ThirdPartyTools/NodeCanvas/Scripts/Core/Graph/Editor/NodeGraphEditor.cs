using UnityEngine;
using System.Collections;
using UnityEditor;
using NodeCanvas;

namespace NodeCanvasEditor{

	public class NodeGraphEditor : EditorWindow{

		public NodeGraphContainer targetGraph;
		private int targetGraphID;
		private NodeGraphContainer currentGraph;
		private Rect canvas= new Rect(0, 0, 2000, 2000);
		private Rect bottomRect;
		private GUISkin guiSkin;
		private Vector2 scrollPos= Vector2.zero;
		private float topMargin = 20;
		private float bottomMargin = 5;

		public void OnEnable(){
			
			title = "NodeCanvas";
			if (EditorGUIUtility.isProSkin){
				guiSkin = Resources.Load("NodeCanvasSkin") as GUISkin;
			} else {
				guiSkin = Resources.Load("NodeCanvasSkinLight") as GUISkin;
			}
			if (targetGraph)
				targetGraph.SendDefaults();
		}

		public void OnInspectorUpdate(){

			if (EditorUtility.InstanceIDToObject(targetGraphID) is NodeGraphContainer){
				targetGraph = EditorUtility.InstanceIDToObject(targetGraphID) as NodeGraphContainer;
				Repaint();
			}
		}

		public void OnGUI(){

			if (EditorApplication.isCompiling){
				ShowNotification(new GUIContent("Compiling Please Wait..."));
				return;			
			}


			if (targetGraph == null){
				ShowNotification(new GUIContent("Please select a GameObject with a Graph Owner"));
				return;
			}

	        if (PrefabUtility.GetPrefabType(targetGraph) == PrefabType.Prefab){
	            ShowNotification(new GUIContent("Editing is not allowed when prefab asset is selected for safety. Please place the prefab in a scene, edit and apply it"));
	            return;
	        }

	       	RemoveNotification();
			GUI.skin = guiSkin;

			Event e = Event.current;

			//Canvas Scroll pan
			if (e.button == 0 && e.isMouse && e.type == EventType.MouseDrag && e.alt)
				scrollPos += e.delta * 2;

			currentGraph = targetGraph.CurrentlyShowingGraph();
			NodeGraphContainer.scrollOffset = scrollPos;

			//Get and set canvas limits for the nodes
			Vector2 canvasLimits= currentGraph.GetCanvasLimits();
			canvas.width = canvasLimits.x;
			canvas.height = canvasLimits.y;

			Rect actualCanvas= new Rect(5, topMargin, position.width - 10, position.height - (topMargin + bottomMargin));
			GUI.Box(actualCanvas, "NodeCanvas v1.4.0", "canvasBG");

			//Begin windows and ScrollView for the nodes.
			scrollPos = GUI.BeginScrollView (actualCanvas, scrollPos, canvas);
			BeginWindows();
            currentGraph.ShowNodeGraphWindows();
            EndWindows();
			GUI.EndScrollView();
			//End windows and scrollview for the nodes.

			currentGraph.ShowInlineInspectorGUI();
			currentGraph.ShowBlackboardGUI();

            currentGraph.ShowNodeGraphControls();
            //TopMID for hierarchy
            GUILayout.Space(5);
			targetGraph.ShowNodeGraphHierarchy();
			//END

			GUI.Box(actualCanvas,"", "canvasBorders");
			Repaint();
			GUI.skin = null;
			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;
		}

		//Change viewing graph
		public void OnSelectionChange(){
			
			if (Selection.activeGameObject != null){
				var lastWindow = EditorWindow.focusedWindow;
				var owner = Selection.activeGameObject.GetComponent<GraphOwner>();
				if (owner != null && owner.graph != null)
					OpenWindow(owner.graph, owner, owner.blackboard);
				if (lastWindow)
					EditorWindow.GetWindow(lastWindow.GetType());
			}
		}

	    //For opening the window from gui button in the nodegraph's Inspector.
	    public static void OpenWindow(NodeGraphContainer targetGraph){

	    	OpenWindow(targetGraph, targetGraph.agent, targetGraph.blackboard);
	    }

	    public static void OpenWindow(NodeGraphContainer targetGraph, Component agent, Blackboard blackboard) {

	        NodeGraphEditor window = GetWindow(typeof(NodeGraphEditor)) as NodeGraphEditor;
	        targetGraph.agent = agent;
	        targetGraph.blackboard = blackboard;
	        targetGraph.SendDefaults();
	        window.targetGraphID = targetGraph.GetInstanceID();
	        window.targetGraph = targetGraph;
	        window.targetGraph.nestedGraphView = null;
	        window.targetGraph.UpdateNodeIDsInGraph();
	        NodeGraphContainer.currentSelection = null;
	    }
	}
}