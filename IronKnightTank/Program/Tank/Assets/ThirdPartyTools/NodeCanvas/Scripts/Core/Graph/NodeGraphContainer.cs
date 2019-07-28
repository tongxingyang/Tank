#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NodeCanvas{

	///This is the base and main class of NodeCanvas and graphs. All graph Systems are deriving from this.
	abstract public class NodeGraphContainer : MonoBehaviour, ITaskDefaults {

		public string graphName = string.Empty;
		
		[SerializeField]
		private NodeBase _primeNode;
		[SerializeField]
		private List<NodeBase> _allNodes = new List<NodeBase>();
		[SerializeField]
		private Component _agent;
		[SerializeField]
		private Blackboard _blackboard;
		[HideInInspector]
		public Transform _nodesRoot;
		private bool _isRunning;
		private bool _isPaused;

		private System.Action FinishCallback;

		/////
		/////

		virtual public System.Type baseNodeType{
			get {return typeof(NodeBase);}
		}

		public NodeBase primeNode{
			get {return _primeNode;}
			set
			{
				if (value && value.allowAsPrime == false){
					Debug.Log("Node '" + value.nodeName + "' can't be set as Start");
					return;
				}
				_primeNode = value;
			}
		}

		public List<NodeBase> allNodes{
			get {return _allNodes;}
			private set {_allNodes = value;}
		}

		///The agent currently assigned to the graph
		public Component agent{
			get {return _agent;}
			set
			{
				if (_agent != value){
					_agent = value;
					SendDefaults();
				}
				_agent = value;
			}
		}

		///The blackboard currently assigned to the graph
		public Blackboard blackboard{
			get {return _blackboard;}
			set
			{
				if (_blackboard != value){
					_blackboard = value;
					SendDefaults();
					UpdateAllNodeBBFields();
				}
				_blackboard = value;
			}
		}

		//Is the graph now running?
		public bool isRunning{
			get {return _isRunning;}
			private set {_isRunning = value;}
		}

		//Is the graph paused?
		public bool isPaused{
			get {return _isPaused;}
			private set {_isPaused = value;}
		}

		public Transform nodesRoot{
			get
			{
				if (_nodesRoot == null)
					_nodesRoot = new GameObject("__ALLNODES__").transform;

				if (_nodesRoot.parent != this.transform)
					_nodesRoot.parent = this.transform;

				_nodesRoot.hideFlags = HideFlags.HideInHierarchy;
				_nodesRoot.localPosition = Vector3.zero;
				return _nodesRoot;			
			}
		}

		virtual protected bool allowNullAgent{
			get {return false;}
		}

		///////
		///////

		//To ensure that if it doesn't exist on applicaiton quit, we dont get a leaking GO
		void Awake(){
			MonoManager.Create();
		}

		//Sets all graph's Tasks' owner (which is this)
		public void SendDefaults(){

			foreach (Task task in nodesRoot.GetComponentsInChildren<Task>(true))
				task.SetOwnerDefaults(this);
		}

		//Update all graph node's BBFields
		private void UpdateAllNodeBBFields(){

			foreach (NodeBase node in allNodes)
				node.UpdateNodeBBFields(blackboard);
		}

		///Sends a OnCustomEvent message to the tasks that needs them
		public void SendEvent(string eventName){

			if (!string.IsNullOrEmpty(eventName))
				agent.gameObject.SendMessage("OnCustomEvent", eventName, SendMessageOptions.DontRequireReceiver);
		}

		new public void SendMessage(string name){
			SendMessage(name, null);
		}

		///Similar to Unity SendMessage + it forwards the message in all of the graph's tasks.
		new public void SendMessage(string name, System.Object argument){

			gameObject.SendMessage(name, argument, SendMessageOptions.DontRequireReceiver);

			bool received = false;
			foreach (Task c in nodesRoot.GetComponentsInChildren<Task>(true)){
				MethodInfo method = c.GetType().GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				if (method != null){
					if (method.GetParameters().Length == 0){
						method.Invoke(c, null);
					}
					else
					if (method.GetParameters().Length == 1){
						method.Invoke(c, new System.Object[] {argument} );
					}

					received = true;
				}
			}

			if (!received)
				Debug.LogError("Message '" + name + "', was not received by any graph task");
		}

		public void StartGraph(){
			StartGraph(this.agent, this.blackboard, null);
		}

		///Start the graph with the already assigned agent and blackboard
		///optionaly providing a callback for when it is finished
		public void StartGraph(System.Action callback){
			StartGraph(this.agent, this.blackboard, callback);
		}

		public void StartGraph(Component agent){
			StartGraph(agent, this.blackboard, null);
		}

		public void StartGraph(Component agent, System.Action callback){
			StartGraph(agent, this.blackboard, callback);
		}

		public void StartGraph(Component agent, Blackboard blackboard){
			StartGraph(agent, blackboard, null);
		}

		///Start the graph providing the agent and blackboard.
		///Optionally provide a callback for when the graph stops or ends
		public void StartGraph(Component agent, Blackboard blackboard, System.Action callback){

			if (isRunning){
				Debug.LogWarning("Graph allready Active");
				return;
			}

			if (primeNode == null){
				Debug.LogWarning("You tried to Start a Graph that has no Start Node.", gameObject);
				return;
			}

			if (agent == null && allowNullAgent == false){
				Debug.LogWarning("You've tried to start a graph with null Agent.");
				return;
			}
			
			if (blackboard == null && agent != null){
				Debug.Log("Graph started with null blackboard. Looking for blackboard on agent '" + agent.gameObject + "'", agent.gameObject);
				blackboard = agent.GetComponent<Blackboard>();
			}

			this.blackboard = blackboard;
			this.agent = agent;
			this.FinishCallback = callback;

			if (!isPaused){
				foreach (NodeBase node in allNodes)
					node.Init();
			}

			isRunning = true;
			isPaused = false;
			UpdateNodeIDsInGraph();
			OnGraphStarted();

			if (isRunning)
				MonoManager.current.AddMethod(OnGraphUpdate);
		}

		///Override for graph specific stuff to run when the graph is started
		virtual protected void OnGraphStarted(){

		}

		///Override for graph specific per frame logic. Called every frame if the graph is actively running
		virtual protected void OnGraphUpdate(){

		}

		///Stops the graph with option to reset nodes when doing so. True by default
		public void StopGraph(){

			MonoManager.current.RemoveMethod(OnGraphUpdate);
			isRunning = false;
			isPaused = false;

			foreach(NodeBase node in allNodes)
				node.ResetNode(false);

			OnGraphStoped();
			
			if (FinishCallback != null)
				FinishCallback();
			FinishCallback = null;
		}

		///Override for graph specific stuff to run when the graph is stoped
		virtual protected void OnGraphStoped(){

		}

		//Pauses the graph. TODO: Complete implementation
		public void PauseGraph(){

			MonoManager.current.RemoveMethod(OnGraphUpdate);
			isRunning = false;
			isPaused = true;
			OnGraphPaused();
		}

		//Called when the graph is paused
		virtual protected void OnGraphPaused(){

		}

		void OnDestroy(){
			MonoManager.current.RemoveMethod(OnGraphUpdate);
		}

		///Get a node by it's ID, null if not found
		public NodeBase FetchNodeByID(int searchID){

			if (searchID <= allNodes.Count && searchID >= 0)	
				return allNodes[searchID - 1];

			return null;
		}

		///Add a new node to this graph
		public NodeBase AddNewNode(System.Type nodeType){

			if (!baseNodeType.IsAssignableFrom(nodeType)){
				Debug.Log(nodeType + " can't be assigned to " + this.GetType() + " graph");
				return null;
			}

			NodeBase newNode = NodeBase.Create(this, nodeType);

			allNodes.Add(newNode);

			if (primeNode == null)
				primeNode = newNode;

			UpdateNodeIDsInGraph();

			return newNode;
		}

		///Disconnects and then removes a node from this graph
		public void RemoveNode(NodeBase nodeToDelete){

			foreach (ConnectionBase outConnection in nodeToDelete.outConnections.ToArray())
				RemoveConnection(outConnection);

			foreach (ConnectionBase inConnection in nodeToDelete.inConnections.ToArray())
				RemoveConnection(inConnection);

			allNodes.Remove(nodeToDelete);
			DestroyImmediate(nodeToDelete.gameObject, true);

			if (nodeToDelete == primeNode)
				primeNode = FetchNodeByID(1);

			UpdateNodeIDsInGraph();
		}

		///Disconnects and then removes a node from this graph by ID
		public void RemoveNode(int id){

			RemoveNode(FetchNodeByID(id));
		}

		
		///Connect two nodes together to the next available port of the source node
		public ConnectionBase ConnectNode(NodeBase sourceNode, NodeBase targetNode){

			return ConnectNode(sourceNode, targetNode, sourceNode.outConnections.Count);
		}

		///Connect two nodes together to a specific port index of the source node
		public ConnectionBase ConnectNode(NodeBase sourceNode, NodeBase targetNode, int indexToInsert){

			if (targetNode == sourceNode){
				Debug.LogWarning("Node can't connect to itself");
				return null;
			}

			if (sourceNode.outConnections.Count >= sourceNode.maxOutConnections && sourceNode.maxOutConnections != -1){
				Debug.LogWarning("Source node can have no more out connections.");
				return null;
			}

			if (targetNode == primeNode && targetNode.maxInConnections == 1){
				Debug.LogWarning("Target node can have no more connections");
				return null;
			}

			if (targetNode.maxInConnections <= targetNode.inConnections.Count && targetNode.maxInConnections != -1){
				Debug.LogWarning("Target node can have no more connections");
				return null;
			}

			ConnectionBase newConnection = ConnectionBase.Create(sourceNode, targetNode, indexToInsert);
			UpdateNodeIDsInGraph();
			sourceNode.OnPortConnected(indexToInsert);
			return newConnection;
		}

		///Removes a connection
		public void RemoveConnection(ConnectionBase connection){

			connection.sourceNode.OnPortDisconnected(connection.sourceNode.outConnections.IndexOf(connection));

			if (Application.isPlaying)
				connection.ResetConnection();

			connection.sourceNode.outConnections.Remove(connection);
			connection.targetNode.inConnections.Remove(connection);
			DestroyImmediate(connection.gameObject, true);
			UpdateNodeIDsInGraph();
		}

		///Update the IDs of the nodes in the graph as well as the GO names of the nodes. Is automatically called whenever a change happens in the graph by the adding removing connecting etc.
		public void UpdateNodeIDsInGraph(){

			int lastID= 0;

			//start with the prime node
			if (primeNode != null)
				lastID = primeNode.AssignIDToGraph(this, lastID);

			//then set remaining nodes that are not connected
			foreach (NodeBase node in allNodes){
				
				if (node.inConnections.Count == 0)
					lastID = node.AssignIDToGraph(this, lastID);
			}

			allNodes = allNodes.OrderBy(node => node.ID).ToList();

			//reset the check
			foreach (NodeBase node in allNodes)
				node.ResetRecursion();
		}

		///Clears the whole graph
		public void ClearGraph(){

			allNodes.Clear();
			primeNode = null;

			DestroyImmediate(nodesRoot.gameObject, true);
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		public string graphComments = string.Empty;
		public bool showComments = true;
		private NodeGraphContainer _nestedGraphView;
		private Rect blackboardRect = new Rect(15, 55, 0, 0);
		private Rect inspectorRect = new Rect(15, 55, 0, 0);
		
		private static float snapCellSize = 28;
		private static UnityEngine.Object _currentSelection;
		//used in PanNodes since e.button == 2 seems bugged
		private static bool middleButtonDown;
		public static Vector2 scrollOffset;

		//
		public bool showNodeInfo{
			get {return EditorPrefs.GetBool("NodeCanvas_showNodeInfo");}
			set {EditorPrefs.SetBool("NodeCanvas_showNodeInfo", value);}
		}

		public bool iconMode{
			get {return EditorPrefs.GetBool("NodeCanvas_iconMode");}
			set {EditorPrefs.SetBool("NodeCanvas_iconMode", value);}
		}

		private bool doSnap{
			get{return EditorPrefs.GetBool("NodeCanvas_doSnap");}
			set{EditorPrefs.SetBool("NodeCanvas_doSnap", value);}
		}

		private bool showBlackboard{
			get {return EditorPrefs.GetBool("NodeCanvas_showBlackboard");}
			set {EditorPrefs.SetBool("NodeCanvas_showBlackboard", value);}
		}

		private bool autoConnect{
			get {return EditorPrefs.GetBool("NodeCanvas_autoConnect");}
			set {EditorPrefs.SetBool("NodeCanvas_autoConnect", value);}
		}

		//

		public static UnityEngine.Object currentSelection{
			get {return _currentSelection;}
			set {GUIUtility.keyboardControl = 0; _currentSelection = value;}
		}

		public NodeGraphContainer nestedGraphView{
			get {return _nestedGraphView;}
			set
			{
				if (value)
					value.nestedGraphView = null;
				_nestedGraphView = value;
				currentSelection = null;
				if (_nestedGraphView != null){
					_nestedGraphView.agent = this.agent;
					_nestedGraphView.blackboard = this.blackboard;
				}
			}
		}

		private NodeBase focusedNode{
			get
			{
				if (currentSelection == null)
					return null;
				if (typeof(NodeBase).IsAssignableFrom(currentSelection.GetType()))
					return currentSelection as NodeBase;			
				return null;
			}
		}

		private ConnectionBase focusedConnection{
			get
			{
				if (currentSelection == null)
					return null;
				if (typeof(ConnectionBase).IsAssignableFrom(currentSelection.GetType()) || currentSelection.GetType() == typeof(ConnectionBase))
					return currentSelection as ConnectionBase;			
				return null;
			}
		}

		private void Reset(){
			graphName = gameObject.name;
			gameObject.isStatic = true;
		}

		private void OnValidate(){
			
			foreach (Transform t in GetComponentsInChildren<Transform>(true))
				t.gameObject.isStatic = true;
		}

		//This is called outside Begin/End Windows from NodeGraphEditor.
		public void ShowNodeGraphControls(){

			if (graphName == string.Empty)
				graphName = gameObject.name;

			Event e = Event.current;
			
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUI.backgroundColor = new Color(1f,1f,1f,0.5f);

			if (GUILayout.Button("Select", EditorStyles.toolbarButton, GUILayout.Width(100)))
				Selection.activeObject = agent != null? agent : this;

			GUILayout.Space(5);

			if (blackboard != null && GUILayout.Button(showBlackboard? "Hide Blackboard" : "Show Blackboard", EditorStyles.toolbarButton, GUILayout.Width(100)))
				showBlackboard = !showBlackboard;

			if (GUILayout.Button(showComments? "Hide Comments" : "Show Comments", EditorStyles.toolbarButton, GUILayout.Width(100)))
				showComments = !showComments;

			GUILayout.Space(20);

			doSnap = GUILayout.Toggle(doSnap, "Snap", EditorStyles.toolbarButton, GUILayout.Width(90));
			autoConnect = GUILayout.Toggle(autoConnect, "Auto Connect", EditorStyles.toolbarButton, GUILayout.Width(90));
			showNodeInfo = GUILayout.Toggle(showNodeInfo, "Node Info", EditorStyles.toolbarButton, GUILayout.Width(90));
			iconMode = GUILayout.Toggle(iconMode, "Icon Mode", EditorStyles.toolbarButton, GUILayout.Width(90));

			GUILayout.FlexibleSpace();

			GUI.backgroundColor = new Color(1, 0.8f, 0.8f, 1);
			if (GUILayout.Button("Clear Graph", EditorStyles.toolbarButton, GUILayout.Width(100))){
				if (EditorUtility.DisplayDialog("Clear Canvas", "This will delete all nodes of the currently viewing graph! Are you sure?", "DO IT", "NO!")){
					ClearGraph();
					e.Use();
					return;
				}
			}

			GUILayout.EndHorizontal();

			GUI.backgroundColor = Color.white;
			ShowGraphCommentsGUI();

			if (e.button == 0 && e.type == EventType.MouseDown && inspectorRect.Contains(e.mousePosition))
				e.Use();

			if (!inspectorRect.Contains(e.mousePosition) && !blackboardRect.Contains(e.mousePosition)){
	
				//canvas click to deselect all
				if (e.button == 0 && e.isMouse && e.type == EventType.MouseDown){
					currentSelection = null;
					return;
				}

				//right click to add node
				if (e.button == 1 && e.type == EventType.MouseDown){
					var pos = e.mousePosition + scrollOffset;
					System.Action<System.Type> Selected = delegate(System.Type type){
						
						NodeBase newNode = AddNewNode(type);
						newNode.nodeRect.center = pos;
						if (autoConnect && focusedNode != null && (focusedNode.outConnections.Count < focusedNode.maxOutConnections || focusedNode.maxOutConnections == -1) ){
							ConnectNode(focusedNode, newNode);
						} else {
							currentSelection = newNode;
						}
					};

					EditorUtils.ShowComponentSelectionMenu(baseNodeType, Selected );
					e.Use();
				}
			}

			//Contract all nodes
			if (e.isKey && e.alt && e.keyCode == KeyCode.Q){
				foreach (NodeBase node in allNodes){
					node.nodeRect.width = NodeBase.minSize.x;
					node.nodeRect.height = NodeBase.minSize.y;
				}
				e.Use();
			}

			//Duplicate
			if (e.isKey && e.control && e.keyCode == KeyCode.D && focusedNode != null){
				currentSelection = focusedNode.Duplicate();
				e.Use();
			}
		}

		//This is called while within Begin/End windows and ScrollArea from the NodeGraphEditor. This is the main function that calls others
		public void ShowNodeGraphWindows(){

			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;

			CheckPanNodes();
			UpdateNodeIDsInGraph();

			for (int i= 0; i < allNodes.Count; i++)
				allNodes[i].ShowNodeGUI();

			EditorUtility.SetDirty(this);
		}

		//Show the comments window
		private void ShowGraphCommentsGUI(){
		
			if (showComments && !string.IsNullOrEmpty(graphComments)){
				GUI.backgroundColor = new Color(1f,1f,1f,0.3f);
				GUI.Box(new Rect(10, Screen.height - 110, 300, 60), graphComments, new GUIStyle("textArea"));
				GUI.backgroundColor = Color.white;
			}
		}

		//Show the target blackboard window
		public void ShowBlackboardGUI(){

			if (!showBlackboard || blackboard == null){
				blackboardRect.height = 0;
				GUILayout.BeginArea(new Rect());
				GUILayout.EndArea();
				return;
			}

			blackboardRect.width = 330;
			blackboardRect.x = Screen.width - 350;
			blackboardRect.y = 30;
			GUISkin lastSkin = GUI.skin;
			GUI.Box(blackboardRect, "", "windowShadow" );

			GUILayout.BeginArea(blackboardRect, "Variables", new GUIStyle("editorPanel"));
			GUILayout.Space(5);
			GUI.skin = null;

			blackboard.ShowVariablesGUI();

			GUILayout.Box("", GUILayout.Height(5), GUILayout.Width(blackboardRect.width - 10));
			GUI.skin = lastSkin;
			if (Event.current.type == EventType.Repaint)
				blackboardRect.height = GUILayoutUtility.GetLastRect().yMax + 5;
			GUILayout.EndArea();		
		}

		//This is the window shown at the top left with a GUI for extra editing opions of the selected node. Probably going to be scrappped.
		public void ShowInlineInspectorGUI(){
			
			if (!focusedNode && !focusedConnection){
				inspectorRect.height = 0;
				GUILayout.BeginArea(new Rect());
				GUILayout.EndArea();
				return;
			}

			inspectorRect.width = 330;
			inspectorRect.x = 15;
			inspectorRect.y = 30;
			GUISkin lastSkin = GUI.skin;
			GUI.Box(inspectorRect, "", "windowShadow" );

			GUILayout.BeginArea(inspectorRect, (focusedNode? focusedNode.nodeName : "Connection"), new GUIStyle("editorPanel"));
			GUILayout.Space(5);
			GUI.skin = null;

			if (focusedNode){
				if (showNodeInfo){
					GUI.backgroundColor = new Color(0.8f,0.8f,1);
					EditorGUILayout.HelpBox(focusedNode.nodeDescription, MessageType.None);
					GUI.backgroundColor = Color.white;
				}

				focusedNode.ShowNodeInspectorGUI();
			}
			
			if (focusedConnection)
				focusedConnection.ShowConnectionInspectorGUI();

			GUILayout.Box("", GUILayout.Height(5), GUILayout.Width(inspectorRect.width - 10));
			GUI.skin = lastSkin;
			if (Event.current.type == EventType.Repaint)
				inspectorRect.height = GUILayoutUtility.GetLastRect().yMax + 5;
			GUILayout.EndArea();
		}

		//This is the hierarchy shown at top middle. recusrsively show the path
		public void ShowNodeGraphHierarchy(){

			string agentInfo = agent != null? agent.gameObject.name : "No Agent";
			string bbInfo = blackboard? blackboard.blackboardName : "No Blackboard";

			GUI.color = new Color(1f,1f,1f,0.5f);
			GUILayout.BeginVertical();
			if (nestedGraphView == this || nestedGraphView == null){

				GUIStyle centerStyle = new GUIStyle("label");
				centerStyle.alignment = TextAnchor.UpperCenter;
				if (agent == null && blackboard == null){
		
					GUILayout.Label("<size=18>" + graphName + "</size>", centerStyle);	
		
				} else {

					GUILayout.Label("<size=18>" + graphName + "</size>" + "\n<size=10>" + agentInfo + " | " + bbInfo + "</size>", centerStyle);
				}

			} else {

				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("^ " + graphName, new GUIStyle("button"))){
					nestedGraphView = null;
					return;
				}
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();

				if (nestedGraphView != null)
					nestedGraphView.ShowNodeGraphHierarchy();
			}

			GUILayout.EndVertical();
			GUI.color = Color.white;
		}

		//Get the space required for the nodes. Called from NodeGraphEditor
		public Vector2 GetCanvasLimits(){

			float maxX = 0;
			float maxY = 0;
			
			for (int i= 0; i < allNodes.Count; i++){
				
				NodeBase node= allNodes[i];
				maxX = Mathf.Max(maxX, node.nodeRect.xMax+20);
				maxY = Mathf.Max(maxY, node.nodeRect.yMax+20);
			}

			return new Vector2(maxX, maxY);
		}

		//Function to pan the nodes about the canvas.
		private void CheckPanNodes(){

			Event e = Event.current;

			//Middle mouse is bugged so...
			if (e.button == 2 && e.type == EventType.MouseDown)
				middleButtonDown = true;

			if (e.button == 2 && e.type == EventType.MouseUp)
				middleButtonDown = false;

			//Pan only the child nodes of the focused node and the focused node itself
			if (e.button == 0 && e.isMouse && e.type == EventType.MouseDrag && e.control){

				if (focusedNode)
					focusedNode.PanNode(e.delta, true);
			}

			//Pan the whole canvas on MiddelClick
			if (middleButtonDown && e.isMouse && e.type == EventType.MouseDrag){

				foreach (NodeBase node in allNodes)
					node.PanNode(e.delta, false);
			}

			//snap all nodes if we not pan canvas
			if (!middleButtonDown && doSnap && e.control == false){
				foreach (NodeBase node in allNodes){
					Vector2 snapedPos = new Vector2(node.nodeRect.xMin, node.nodeRect.yMin);
					snapedPos.y = Mathf.Round(snapedPos.y / snapCellSize) * snapCellSize;
					node.nodeRect = new Rect(snapedPos.x, snapedPos.y, node.nodeRect.width, node.nodeRect.height);
				}
			}
		}

		//Recursively get which graph is currently showing. Used from Nodegraph editor
		public NodeGraphContainer CurrentlyShowingGraph(){

			if (nestedGraphView == this || nestedGraphView == null){

				return this;

			} else {

				return nestedGraphView.CurrentlyShowingGraph();
			}
		}

		#endif
	}
}