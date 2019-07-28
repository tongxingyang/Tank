#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.Variables;

namespace NodeCanvas{

	///The base class for all Actions and Conditions. You dont actually use or derive this class. Instead derive from ActionTask and ConditionTask
	abstract public class Task : MonoBehaviour{

		[SerializeField]
		private MonoBehaviour _ownerSystem;
		[SerializeField]
		private Component _agent;
		private bool _agentIsOverride;
		private System.Type _agentType;
		private Blackboard _blackboard;


		//These are special so I write them first
		public void SetOwnerDefaults(ITaskDefaults newOwner){

			if (newOwner == null)
				return;

			_ownerSystem = (MonoBehaviour)newOwner;
			blackboard = newOwner.blackboard;
			UpdateBBFields(blackboard);
		}

		protected ITaskDefaults ownerSystem{
			get {return _ownerSystem as ITaskDefaults;}
		}

		private Component ownerAgent{
			get
			{
				if (_ownerSystem == null)
					return null;
				return (_ownerSystem as ITaskDefaults).agent;
			}
		}

		private Blackboard ownerBlackboard{
			get
			{
				if (_ownerSystem == null)
					return null;
				return (_ownerSystem as ITaskDefaults).blackboard;
			}
		}
		///


		///The type that the agent will be set to by getting component from itself when the task initialize
		///By returning null Type, specifies that the task needs no agent at all. You can omit this to keep the agent passed as is.
		public System.Type agentType{

			get
			{
				if (_agentType == null){
					AgentTypeAttribute agentTypeAttribute = this.GetType().GetCustomAttributes(typeof(AgentTypeAttribute), true).FirstOrDefault() as AgentTypeAttribute;
					if (agentTypeAttribute != null && typeof(Component).IsAssignableFrom(agentTypeAttribute.type) ){
						_agentType = agentTypeAttribute.type;
					} else {
						_agentType = typeof(Component);
					}
				}
				return _agentType;
			}
		}

		//The friendly task name
		public string taskName{
			get
			{
				ScriptNameAttribute nameAttribute = this.GetType().GetCustomAttributes(typeof(ScriptNameAttribute), false).FirstOrDefault() as ScriptNameAttribute;
				if (nameAttribute != null){

					return nameAttribute.name;

				} else {

					#if UNITY_EDITOR
					return EditorUtils.CamelCaseToWords(EditorUtils.TypeName(this.GetType()));
					#endif
					#if !UNITY_EDITOR
					return this.GetType().Name;
					#endif
				}			
			}
		}

		//A short description of what the task will finaly do. Derived tasks may override this.
		virtual public string taskInfo{
			get {return taskName;}
		}


		///Agent is set to override if on task awake the agent is assigned. eg by the inspector
		public bool agentIsOverride{
			get
			{
				if (!Application.isPlaying)
					return _agent != null;

				return _agentIsOverride;
			}

			private set { _agentIsOverride = value;	}
		}

		///The current or last executive agent of this task
		protected Component agent{
			get
			{

				return _agent != null? _agent : ownerAgent;
			}

			private set
			{
				if (_agent != null && _agent.gameObject == value.gameObject)
					return;

				_agent = TransformAgent(value, agentType);
			}
		}

		///The current or last blackboard of this task
		protected Blackboard blackboard{
			get
			{
				if (_blackboard == null){
					_blackboard = ownerBlackboard;
					UpdateBBFields(_blackboard);
				}

				if (_blackboard == null && _agent != null){
					_blackboard = _agent.GetComponent<Blackboard>();
					UpdateBBFields(_blackboard);
				}

				return _blackboard;
			}

			private set
			{
				if (_blackboard != value)
					UpdateBBFields(value);

				_blackboard = value;
			}
		}

		//////////

		void Awake(){

			enabled = false;
			CheckNullBBFields();
			OnPreAwake();

			if (_agent != null){

				agentIsOverride = true;
				_agent = TransformAgent(_agent, agentType);
				Initialize(_agent);
			}

			OnAwake();
		}

		void Reset(){
			CheckNullBBFields();
		}

		void OnValidate(){
			enabled = false;
			CheckNullBBFields();
		}

		//Using preawake to set the agent as override through code
		//you are hardly going to need it
		virtual protected void OnPreAwake(){

		}

		///Override in your own Tasks. Use this instead of Awake
		virtual protected void OnAwake(){

		}

		//Tasks can start coroutine through MonoManager for even when they are disabled
		new protected Coroutine StartCoroutine(IEnumerator routine){

			return MonoManager.current.StartCoroutine(routine);
		}

		///Override in your own Tasks. This is called after a NEW agent is set, after initialization and before execution
		virtual protected string OnInit(){
			return null;
		}

		///Used for short typing. Sends an event to the owner system to handle (same as calling ownerSystem.SendEvent)
		protected void SendEvent(string eventName){

			if (ownerSystem != null)
				ownerSystem.SendEvent(eventName);
		}

		//Actions and Conditions call this. Bit reduntant code but returns if the task was sucessfully initialized as well
		protected bool Set(Component newAgent, Blackboard newBB){

			//set blackboard with normal setter first
			blackboard = newBB;

			if (agentIsOverride){
				if (_agent == null){
					Debug.LogError("A Static agent reference is missing on Task '" + taskName + "'...", gameObject);
					return false;
				}
				return true;
			}

			//If are the same dont re-init.
			if (_agent != null && _agent.gameObject == newAgent.gameObject)
				return true;

			_agent = TransformAgent(newAgent, agentType);
			return Initialize(_agent);
		}

		//helper function
		private Component TransformAgent(Component newAgent, System.Type type){
			return (type != typeof(Component))? newAgent.GetComponent(type) : newAgent;
		}


		//Initialize whenever agent is set to a new value. Essentially usage of the attributes
		private bool Initialize(Component newAgent){

			if (newAgent == null){
				Debug.LogError("<b>Task Init:</b> Failed to change Agent to type '" + agentType + "', for Task '" + taskName + "' or new Agent is NULL. Does the Agent has that Component?", gameObject);
				return false;			
			}

			//Usage of [EventListener] attribute
			EventListenerAttribute msgAttribute = this.GetType().GetCustomAttributes(typeof(EventListenerAttribute), true).FirstOrDefault() as EventListenerAttribute;
			if (msgAttribute != null){

				AgentUtilities agentUtils = newAgent.GetComponent<AgentUtilities>();

				if (agentUtils == null)
					agentUtils = newAgent.gameObject.AddComponent<AgentUtilities>();

				foreach (string msg in msgAttribute.messages)
					agentUtils.Listen(this, msg);
			}

			//Usage of [RequiresBlackboard] attribute
			RequiresBlackboardAttribute requireBB = this.GetType().GetCustomAttributes(typeof(RequiresBlackboardAttribute), true).FirstOrDefault() as RequiresBlackboardAttribute;
			if (requireBB != null && blackboard == null){
				Debug.LogError("<b>Task Init:</b> Task '" + taskName + "' requires a Blackboard to have been passed, but no Blackboard reference exists");
				return false;
			}

			//Usage of [RequiredField] and [GetFromAgent] attributes
			foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){
				
				RequiredFieldAttribute requiredAttribute = field.GetCustomAttributes(typeof(RequiredFieldAttribute), true).FirstOrDefault() as RequiredFieldAttribute;
				if (requiredAttribute != null){

					var value = field.GetValue(this);
					var valueType = value.GetType();

					if (value == null){
						Debug.LogError("<b>Task Init:</b> A required field for Task '" + taskName + "', is not set! Field: '" + field.Name + "' ", gameObject);
						return false;
					}

					//must check against casted UnityEngine.Object due to Unity serializer
					if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType)){
						if (value as UnityEngine.Object == null){
							Debug.LogError("<b>Task Init:</b> A required field for Task '" + taskName + "', is not set! Field: '" + field.Name + "' ", gameObject);
							return false;
						}
					}

					if (valueType == typeof(string) && string.IsNullOrEmpty((string)value) ){
						Debug.LogError("<b>Task Init:</b> A required string for Task '" + taskName + "', is not set! Field: '" + field.Name + "' ", gameObject);
						return false;
					}

					if (typeof(BBValue).IsAssignableFrom(valueType) && (value as BBValue).isNull ) {
						Debug.LogError("<b>Task Init:</b> A required BBVariable value for Task '" + taskName + "', is not set! Field: '" + field.Name + "' ", gameObject);
						return false;
					}
				}

				GetFromAgentAttribute getterAttribute = field.GetCustomAttributes(typeof(GetFromAgentAttribute), true).FirstOrDefault() as GetFromAgentAttribute;
				if (getterAttribute != null){

					if (typeof(Component).IsAssignableFrom(field.FieldType)){

						field.SetValue(this, newAgent.GetComponent(field.FieldType));
						if ( (field.GetValue(this) as UnityEngine.Object) == null){

							Debug.LogError("<b>Task Init:</b> GetFromAgent Attribute failed to get the required component of type '" + field.FieldType + "' from '" + agent.gameObject.name + "'. Does it exist?", agent.gameObject);
							return false;
						}
					
					} else {

						Debug.LogWarning("<b>Task Init:</b> You've set a GetFromAgent Attribute on a field (" + field.Name + ") whos type does not derive Component on Task '" + taskName + "'", gameObject);
					}
				}
			}

			//let user make further adjustments and inform us if there was an error
			string errorString = OnInit();
			if (errorString != null){
				Debug.LogError("<b>Task Init:</b> " + errorString + ". Task '" + taskName + "'");
				return false;
			}
			return true;
		}

		//Set the target blackboard for all BBValues found in class. This is done every time the blackboard of the Task is set to a new value
		private void UpdateBBFields(Blackboard bb){

			foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){

				if ((field.GetValue(this) as BBValue) != null)
					(field.GetValue(this) as BBValue).bb = bb;

				if (typeof(BBValueSet) == field.FieldType)
					(field.GetValue(this) as BBValueSet).bb = bb;
			}
		}

		//Helper to ensure that BBValues are not null.
		private void CheckNullBBFields(){

			foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)){

				if (typeof(BBValue).IsAssignableFrom(field.FieldType) && field.GetValue(this) == null){
					var cons = field.FieldType.GetConstructor(Type.EmptyTypes);
					field.SetValue(this, cons.Invoke(null));
				}
			}
		}

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[SerializeField]
		private bool _unfolded = true;

		public bool unfolded{
			get {return _unfolded;}
			set {_unfolded = value;}
		}

		virtual public void ShowTaskEditGUI(){

			if (agentIsOverride && _agent == null){
				GUI.color = EditorUtils.lightRed;
				EditorGUILayout.LabelField("Missing Agent Reference!");
				GUI.color = Color.white;
				return;
			}

			if (agentType != typeof(Component))
				agentIsOverride = EditorUtils.AgentField(ref _agent, agentType, ownerAgent, agentIsOverride);
		}

		virtual public Task CopyTo(GameObject go){

			if (this == null)
				return null;

			Task copiedTask = (Task)go.AddComponent(this.GetType());
			UnityEditor.EditorUtility.CopySerialized(this, copiedTask);
			return copiedTask;
		}

		#endif


		//If the field is deriving Component then it will be retrieved from the agent. The field is also considered Required for correct initialization
		[AttributeUsage(AttributeTargets.Field)]
		public class GetFromAgentAttribute : Attribute{

		}

		//Designates that the task requires Unity messages to be forwarded from the agent and to the task
		[AttributeUsage(AttributeTargets.Class)]
		public class EventListenerAttribute : Attribute{

			public string[] messages;

			public EventListenerAttribute(params string[] args){
				this.messages = args;
			}
		}

		//Designates that the task REQUIRES a blackboard to work properly
		[AttributeUsage(AttributeTargets.Class)]
		public class RequiresBlackboardAttribute : Attribute{

		}

		//Designates what type of component to get and set the agent from the agent itself on initialization
		[AttributeUsage(AttributeTargets.Class)]
		public class AgentTypeAttribute : Attribute{

			public System.Type type;

			public AgentTypeAttribute(System.Type type){
				this.type = type;
			}
		}
	}
}