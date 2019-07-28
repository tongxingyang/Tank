#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas{

	public enum NodeStates {
		
		Failure  = 0,
		Success  = 1,
		Running  = 2,
		Resting  = 3,
		Error    = 4,
	}

	///The base class for all nodes that can live in NodeCanvas
	abstract public class NodeBase : MonoBehaviour{

		[SerializeField]
		private List<ConnectionBase> _inConnections = new List<ConnectionBase>();
		[SerializeField]
		private List<ConnectionBase> _outConnections = new List<ConnectionBase>();
		[SerializeField]
		private NodeGraphContainer _graph;

		private NodeStates _nodeState = NodeStates.Resting;
		private float _elapsedTime;
		private int _ID;
		private bool isChecked;

		////
		////

		///The title name of the node shown in the window
		virtual public string nodeName{
			get
			{

				ScriptNameAttribute nameAttribute = this.GetType().GetCustomAttributes(typeof(ScriptNameAttribute), false).FirstOrDefault() as ScriptNameAttribute;
				if (nameAttribute != null){
					return nameAttribute.name;
				} else {
					return this.GetType().Name;
				}
			}
		}

		///The description shown at the bottom of nodecanvas as info
		virtual public string nodeDescription{
			get {return "Override 'nodeDescription' to show information here..";}
		}

		///The numer of possible inputs. -1 for infinite
		virtual public int maxInConnections{
			get {return -1;}
		}

		///The numer of possible outputs. -1 for infinite
		virtual public int maxOutConnections{
			get {return -1;}
		}

		///The output connection Type this node has
		virtual public System.Type outConnectionType{
			get {return typeof(ConditionalConnection);}
		}

		virtual public bool allowAsPrime{
			get {return true;}
		}

		public List<ConnectionBase> inConnections{
			get {return _inConnections;}
			protected set {_inConnections = value;}
		}

		public List<ConnectionBase> outConnections{
			get {return _outConnections;}
			protected set {_outConnections = value;}
		}

		///The graph this node belongs to
		public NodeGraphContainer graph{
			get {return _graph;}
			private set {_graph = value;}
		}

		///The current state of the node
		public NodeStates nodeState{
			get {return _nodeState;}
			protected set {_nodeState = value;}
		}

		///The elapsed time this node runs
		public float elapsedTime{
			get {return _elapsedTime;}
			protected set {_elapsedTime = value;}
		}

		//The node's ID
		public int ID{
			get {return _ID;}
			private set {_ID = value;}
		}

		///The agent of the graph this node belongs to
		protected Component graphAgent{
			get {return graph != null? graph.agent : null;}
		}

		///The blackboard of the graph this node belongs to
		protected Blackboard graphBlackboard{
			get {return graph != null? graph.blackboard : null;}
		}

		/////////////////////
		/////////////////////
		/////////////////////

		public static NodeBase Create(NodeGraphContainer ownerGraph, System.Type nodeType){

			NodeBase newNode = new GameObject(nodeType.ToString()).AddComponent(nodeType) as NodeBase;
			newNode.graph = ownerGraph;
			newNode.transform.parent = ownerGraph.nodesRoot;
			newNode.transform.localPosition = Vector3.zero;
			newNode.UpdateNodeBBFields(ownerGraph.blackboard);
			newNode.OnCreate();
			return newNode;
		}

		///Called when the node is created
		virtual protected void OnCreate(){
			
		}

		public NodeStates Execute(){
			return Execute(graphAgent, graphBlackboard);
		}

		public NodeStates Execute(Component agent){
			return Execute(agent, graphBlackboard);
		}

		///The main execution function of the node
		public NodeStates Execute(Component agent, Blackboard blackboard){
			
			if (isChecked)
				return Error("Infinite Loop Detected. Node ID: " + ID +"  will not execute to prevent stack overflow.", gameObject);
				
			isChecked = true;
			nodeState = OnExecute(agent, blackboard);
			isChecked = false;
			
			return nodeState;
		}

		protected NodeStates Error(string log){
			return Error(log, null);
		}

		///Just a little helper function to log errors more easily
		protected NodeStates Error(string log, GameObject go){
			Debug.LogError("<b>Graph Error:</b> '" + log + "' On node '" + nodeName + "' ID " + ID + " | On graph '" + graph.graphName + "'", go);
			return NodeStates.Error;
		}


		///Derived nodes override this to do their stuff here
		virtual protected NodeStates OnExecute(Component agent, Blackboard blackboard){
			
			return OnExecute(agent);
		}

		virtual protected NodeStates OnExecute(Component agent){

			return OnExecute();
		}

		virtual protected NodeStates OnExecute(){

			return nodeState;
		}

		///Called from the graph when it starts to initialize values
		public void Init(){
			OnInit();
		}

		///Override. Called when node gets initialized. Use to init values
		virtual protected void OnInit(){

		}

		public void ResetNode(){
			ResetNode(true);
		}

		///Recursively reset the node and child nodes
		public void ResetNode(bool recursively){

			if (nodeState == NodeStates.Resting)
				return;

			if (isChecked)
				return;

			OnReset();
			nodeState = NodeStates.Resting;
			elapsedTime = 0;

			isChecked = true;
			for (int i = 0; i < outConnections.Count; i++)
				outConnections[i].ResetConnection(recursively);
			isChecked = false;
		}

		///Called when the node gets reseted
		virtual protected void OnReset(){

		}

		//Nodes can start coroutine through MonoManager for even when they are disabled
		new protected Coroutine StartCoroutine(IEnumerator routine){
			
			return MonoManager.current.StartCoroutine(routine);
		}

		//Set the target blackboard for all BBValues found on node. Done when creating node as well as graphBlackboard set to new value.
		public void UpdateNodeBBFields(Blackboard bb){

			foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){
				if ((field.GetValue(this) as BBValue) != null)
					(field.GetValue(this) as BBValue).bb = bb;

				if (typeof(BBValueSet) == field.FieldType)
					(field.GetValue(this) as BBValueSet).bb = bb;
			}
		}

		///Updates the node ID as well as its GO name and outConnection names. This is called in the editor update for convenience, as well as whenever a change is made in the node graph and from the node graph.
		public int AssignIDToGraph(NodeGraphContainer toNodeGraph, int lastID){

			if (isChecked)
				return lastID;

			isChecked = true;

			lastID++;
			ID = lastID;

			if (gameObject.name != ID + "_" + nodeName)
				gameObject.name = ID + "_" + nodeName;

			//parent node for clarity to the nodes original graph
			if (gameObject.transform.parent != graph.nodesRoot)
				gameObject.transform.parent = graph.nodesRoot;
			
			for ( int i= 0; i < outConnections.Count; i++){

				ConnectionBase connection= outConnections[i];
				lastID = connection.targetNode.AssignIDToGraph(toNodeGraph, lastID);

				if (connection.gameObject.name != connection.sourceNode.ID + "_" + connection.targetNode.ID + " Connection")
					connection.gameObject.name = connection.sourceNode.ID + "_" + connection.targetNode.ID + " Connection";

				//also parent connection for clarity
				if (connection.gameObject.transform.parent != this.gameObject.transform)
					connection.gameObject.transform.parent = this.gameObject.transform;
			}

			return lastID;
		}

		public void ResetRecursion(){

			if (!isChecked)
				return;

			isChecked = false;
			
			for (int i = 0; i < outConnections.Count; i++)
				outConnections[i].targetNode.ResetRecursion();
		}

		///Fetch all child nodes of the node
		protected List<NodeBase> FetchAllChildNodes(bool includeThis){

			List<NodeBase> childList = new List<NodeBase>();

			foreach (ConnectionBase connection in outConnections){
				childList.Add(connection.targetNode);
				childList.AddRange(connection.targetNode.FetchAllChildNodes(false));
			}

			if (includeThis)
				childList.Add(this);

			return childList;
		}


		///Called when a port is connected
		virtual public void OnPortConnected(int portIndex){

		}

		///Called when a port is disconnected but before it actually does
		virtual public void OnPortDisconnected(int portIndex){

		}

		public void MoveToGraph(NodeGraphContainer newGraph){

			graph.allNodes.Remove(this);
			newGraph.allNodes.Add(this);
			graph = newGraph;
			
			transform.parent = graph.nodesRoot;
			transform.localPosition = Vector3.zero;
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[HideInInspector]
		public Rect nodeRect = new Rect(100,300,100,40);

		[SerializeField]
		protected string nodeComment = string.Empty;

		private bool inResizeMode;
		private Texture2D _icon;

		public static Vector2 minSize = new Vector2(100, 40);
		private static Port clickedPort;

		public bool inIconMode{
			get {return icon != null && graph.iconMode;}
		}

		public Texture2D icon{
			get
			{
				if (_icon == null) _icon = Resources.Load(this.GetType().Name) as Texture2D;
				return _icon;			
			}
		}

		public static Color successColor{
			get {return new Color(0.4f, 0.7f, 0.2f);}
		}

		public static Color failureColor{
			get {return new Color(1.0f, 0.4f, 0.4f);}
		}

		public static Color runningColor{
			get {return Color.yellow;}
		}

		public static Color restingColor{
			get {return new Color(0.5f ,0.5f, 0.8f, 0.8f);}
		}

		////////////////


		//The main function for drawing a node's gui.Fires off others.
		public void ShowNodeGUI(){

			if (graph && graph.primeNode == this)
				GUI.Box(new Rect(nodeRect.x, nodeRect.y - 20, nodeRect.width, 20), "<b>START</b>");

			DrawNodeWindow();
			DrawNodeComments();
			DrawNodeConnections();

			EditorUtility.SetDirty(this);
		}

		private void DrawNodeComments(){

			if (graph && graph.showComments){

				var commentsRect = new Rect();

				if (maxOutConnections == 0){
					var height = new GUIStyle("textArea").CalcHeight(new GUIContent(nodeComment), nodeRect.width);
					commentsRect = new Rect(nodeRect.x, nodeRect.yMax + 5, nodeRect.width, height);
				} else {
					commentsRect = new Rect(nodeRect.xMax + 5, nodeRect.yMin, 130, nodeRect.height);
				}

				if (!string.IsNullOrEmpty(nodeComment)){
					GUI.color = new Color(1,1,1,0.6f);
					GUI.backgroundColor = new Color(1f,1f,1f,0.2f);
					GUI.Box(commentsRect, nodeComment, new GUIStyle("textArea"));
					GUI.backgroundColor = Color.white;
					GUI.color = Color.white;
				}
			}
		}

		//Simple method that most importantly creates the node window + it put some color for debug in the graph editor.
		private void DrawNodeWindow(){

			if (NodeGraphContainer.currentSelection == this)
				GUI.color = new Color(0.95f, 0.95f, 1);

			GUI.Box(nodeRect, "", new GUIStyle("windowShadow") );

			if (inIconMode){

				nodeRect = 
				    GUILayout.Window(ID, nodeRect, NodeWindowGUI, string.Empty, "compactWindow");

			} else {
			
				nodeRect =
				    GUILayout.Window (ID, nodeRect, NodeWindowGUI, nodeName, "window");
			}

			GUI.color = Color.white;

			if (Application.isPlaying){

				if (nodeState == NodeStates.Success)
					GUI.color = successColor;
				if (nodeState == NodeStates.Running)
					GUI.color = runningColor;
				if (nodeState == NodeStates.Failure)
					GUI.color = failureColor;
				if (nodeState == NodeStates.Resting)
					GUI.color = restingColor;

				GUI.Box(nodeRect, "", new GUIStyle("windowHighlight"));
				
			} else {
				
				if (NodeGraphContainer.currentSelection == this){
					GUI.color = new Color(0.5f,0.5f,0.8f,0.8f);
					GUI.Box(nodeRect, "", new GUIStyle("windowHighlight"));
				}
			}

			GUI.color = Color.white;
		}

		//This is the callback function of the GUILayout.window. Everything here is INSIDE the node Window.
		private void NodeWindowGUI(int ID){

			if (this == null)
				return;

			if (inIconMode){
				GUI.backgroundColor = new Color(0,0,0,0.05f);
				GUILayout.Box(icon);
				GUI.backgroundColor = Color.white;
			}

			Event e = Event.current;

			if (e.button == 0 && e.type == EventType.MouseDown)
				NodeGraphContainer.currentSelection = this;

			if (NodeGraphContainer.currentSelection == this && e.keyCode == KeyCode.Delete && e.type == EventType.KeyDown){
				graph.RemoveNode(this);
				return;
			}

		    Rect scaleNodeRect= new Rect(nodeRect.width-10,nodeRect.height-10, 8, 8);
		    GUI.Box(scaleNodeRect, "", "nodeScaleBtn");

		    if (e.button == 0 && e.type == EventType.MouseDown && scaleNodeRect.Contains(e.mousePosition))
		    	inResizeMode = true;

		    if (inResizeMode && e.type == EventType.MouseUp)
	    		inResizeMode = false;

	    	if (inResizeMode){
		    	nodeRect.width = Mathf.Max(e.mousePosition.x+10, minSize.x);
		    	nodeRect.height = Mathf.Max(e.mousePosition.y+10, minSize.y);
		    }

	        if (e.button == 0 && e.type == EventType.MouseDrag && e.control)
	        	PanNode(e.delta, true);


	        Rect markRect = new Rect(5, 5, 15, 15);
	        if (nodeState == NodeStates.Success){
	        	GUI.color = successColor;
	        	GUI.Box(markRect, "", new GUIStyle("checkMark"));

	        } else if (nodeState == NodeStates.Running){
	        	GUI.Box(markRect, "", new GUIStyle("clockMark"));

	        } else if (nodeState == NodeStates.Failure){
	        	GUI.color = failureColor;
	        	GUI.Box(markRect, "", new GUIStyle("xMark"));
	        }


	        GUI.color = Color.white;
	        GUI.skin = null;
	        GUI.skin.label.richText = true;

			OnNodeGUI();

		    if (e.button == 1 && e.type == EventType.MouseDown){

	            GenericMenu menu = new GenericMenu();

	            if (graph.primeNode != this && allowAsPrime)
		            menu.AddItem (new GUIContent ("Make Start"), false, ContextMakePrime);

				menu.AddItem (new GUIContent ("Duplicate"), false, ContextDuplicate);

	            OnContextMenu(menu);

				menu.AddSeparator("--------");
	            menu.AddItem (new GUIContent ("Delete Node"), false, ContextDelete);
	            menu.ShowAsContext();
	            e.Use();
		    } 

		    if (!inResizeMode){

		    	GUI.DragWindow();
		    }
		}

		//Editor. Override to show controls within the node window
		virtual protected void OnNodeGUI(){

			GUILayout.Label("", GUILayout.Height(1));
		}

		public void ShowNodeInspectorGUI(){

			OnNodeInspectorGUI();
		}

		//Editor. Override to show controls within the inline inspector or leave it to show an automatic editor
		virtual protected void OnNodeInspectorGUI(){

			EditorUtils.ShowAutoEditorGUI(this);
		}

		//Editor. Override to add more entries to the right click context menu of the node
		virtual protected void OnContextMenu(GenericMenu menu){

		}

		//Callback function from the context menu above.
		private void ContextDelete(){

			graph.RemoveNode(this);
		}

		//Callback function from the context menu above.
		private void ContextMakePrime(){

			graph.primeNode = this;
		}

		private void ContextDuplicate(){

			Duplicate();
		}

		//Duplicate node
		public NodeBase Duplicate(){
			var newNode = Instantiate(this, this.transform.position, this.transform.rotation) as NodeBase;
			newNode.transform.parent = this.transform.parent;
			graph.allNodes.Add(newNode);
			newNode.inConnections.Clear();
			newNode.outConnections.Clear();
			newNode.nodeRect.center += new Vector2(50,50);
			return newNode;
		}

		//Draw the connections line from this node, to all of its children. This is the default. Override in each system's base node class.
		virtual protected void DrawNodeConnections(){

			Event e = Event.current;

			//Receive connections
			if (clickedPort != null && e.type == EventType.MouseUp){

				var port = clickedPort;
				if (ID == graph.allNodes.Count)
					clickedPort = null;

				if (nodeRect.Contains(e.mousePosition)){
					if (graph.ConnectNode(port.parent, this, port.portIndex) != null)
						clickedPort = null;
				}
			}

			if (maxOutConnections == 0)
				return;

			 var nodeOutputBox = new Rect(nodeRect.x, nodeRect.yMax - 4, nodeRect.width, 12);
			 GUI.Box(nodeOutputBox, "", new GUIStyle("nodePortContainer"));
			
			 if (outConnections.Count < maxOutConnections || maxOutConnections == -1){
   
			 	for (int i = 0; i < outConnections.Count + 1; i++){
			 		
			 		Rect portRect = new Rect(0, 0, 10, 10);
			 		portRect.center = new Vector2(((nodeRect.width / (outConnections.Count + 1)) * (i + 0.5f)) + nodeRect.xMin, nodeRect.yMax + 6);
			 		GUI.Box(portRect, "", "nodePortEmpty");
   
			 		if (e.button == 0 && e.type == EventType.MouseDown && portRect.Contains(e.mousePosition))
			 			clickedPort = new Port(i, this, portRect.center);
			 	}
			 }

		    
			//draw the new connection line if in link mode
			if (clickedPort != null && clickedPort.parent == this)
				Handles.DrawBezier(clickedPort.pos, e.mousePosition, clickedPort.pos, e.mousePosition, new Color(0.5f,0.5f,0.8f,0.8f), null, 2);

            //draw all connected lines
            for (int connectionIndex = 0; connectionIndex < outConnections.Count; connectionIndex++)
            {

                var connection = outConnections[connectionIndex];
                if (connection != null)
                {

                    var sourcePos = new Vector2(((nodeRect.width / (outConnections.Count + 1)) * (connectionIndex + 1)) + nodeRect.xMin, nodeRect.yMax + 6);
                    var targetPos = new Vector2(connection.targetNode.nodeRect.center.x, connection.targetNode.nodeRect.y);

                    Rect connectedPortRect = new Rect(0, 0, 12, 12);
                    connectedPortRect.center = sourcePos;
                    GUI.Box(connectedPortRect, "", "nodePortConnected");
                    connection.DrawConnectionGUI(sourcePos, targetPos);

                    //On right click disconnect connection from the source.
                    if (e.button == 1 && e.type == EventType.MouseDown && connectedPortRect.Contains(e.mousePosition))
                    {
                        graph.RemoveConnection(connection);
                        e.Use();
                        return;
                    }
                }
            }
        }

		//EDITOR. Class for the nodeports
		class Port{

			public int portIndex;
			public NodeBase parent;
			public Vector2 pos;

			public Port(int index, NodeBase parent, Vector2 pos){
				this.portIndex = index;
				this.parent = parent;
				this.pos = pos;
			}
		}

		//As the name implies, function to pan the node recursively if need be. Called from the graph or from this
		public void PanNode(Vector2 delta, bool panChildren){

			float newX = nodeRect.center.x;
			float newY = nodeRect.center.y;
			newX += delta.x;
			newY += delta.y;
			nodeRect.center = new Vector2(newX, newY);

			if (panChildren){

				for (int i= 0; i < outConnections.Count; i++){
					NodeBase node= outConnections[i].targetNode;
					if (node.ID > this.ID)					
						node.PanNode(delta, true);
				}
			}
		}

		#endif
	}
}