﻿#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Reflection;

namespace NodeCanvas{

	///Base class for all actions. Extend this to create your own.
	abstract public class ActionTask : Task{

		[SerializeField] [HideInInspector]
		private float _actionDelay;
		private float _elapsedTime;
		private float _estimatedLength;
		private bool _isRunning;
		private System.Action<System.ValueType> FinishCallback;

		public float actionDelay{
			get {return _actionDelay;}
			set {_actionDelay = value;}
		}

		///The time in seconds this action is running if at all
		public float elapsedTime{
			get {return _elapsedTime;}
			private set {_elapsedTime = value;}
		}

		///Is the action currently running
		public bool isRunning{
			get {return _isRunning;}
			private set {_isRunning = value;}
		}

		//Override.The estimated length this action will take to complete
		virtual public float estimatedLength{
			get {return _estimatedLength;}
			private set {_estimatedLength = value;}
		}

		sealed override public string taskInfo{
			get {return (agentIsOverride? "*" : "") + actionInfo;}
		}

		//Editor: Override in your own actions to provide the visible editor action info whenever it's shown
		virtual protected string actionInfo{
			get {return taskName;}
		}

		
		////////
		////////

		public void ExecuteAction(Component agent, System.Action<System.ValueType> callback){

			ExecuteAction(agent, this.blackboard, callback);
		}


		///Executes the action optionaly providing a blackboard and a callback function that will be called with a ValueType argument
		///once the action is completed. The argument in most cases will be a boolean specifying if the action did succeed or failed.
		public void ExecuteAction(Component agent, Blackboard blackboard, System.Action<System.ValueType> callback){

			if (isRunning)
				return;

			//if Init fails just call back & return
			if (!Set(agent, blackboard))
				return;

			FinishCallback = callback;
			isRunning = true;
			enabled = true;

			OnExecute();

			if (isRunning)
				MonoManager.current.AddMethod(UpdateAction);
		}

		private void UpdateAction(){
			elapsedTime += Time.deltaTime;
			OnUpdate();
		}

		///Override in your own actions. Called once when the actions is executed.
		virtual protected void OnExecute(){

		}

		///Override in your own actions. Called every frame, if and while the action is running and until it ends.
		virtual protected void OnUpdate(){

		}

		public void EndAction(){
			EndAction(true);
		}

		///Ends the action either in success or failure. The callback function (passed on ExecuteAction) is called with the same parameter that this function was called.
		///If not called, the action will run indefinetely. The action is then reseted.
		public void EndAction(System.ValueType param){
			
			if (!isRunning)
				return;
			
			MonoManager.current.RemoveMethod(UpdateAction);
			enabled = false;
			isRunning = false;
			estimatedLength = elapsedTime;
			elapsedTime = 0;
			OnStop();

			if (FinishCallback != null)
				FinishCallback(param);
			FinishCallback = null;
		}

		///Called whenever the action ends due to any reason.
		virtual protected void OnStop(){

		}

		void OnDestroy(){
			MonoManager.current.RemoveMethod(UpdateAction);
		}

		//////////////////////////////////
		/////////GUI & EDITOR STUFF///////
		//////////////////////////////////
		#if UNITY_EDITOR

		///Editor: Draw the action's controls.
		override public void ShowTaskEditGUI(){

			if (Application.isPlaying){
				if (elapsedTime > 0)
					GUI.color = Color.yellow;

				EditorGUILayout.LabelField("Elapsed Time", elapsedTime.ToString());
			}
			
			GUI.color = Color.white;

			base.ShowTaskEditGUI();
			OnActionEditGUI();

			if (GUI.changed && this != null)
				EditorUtility.SetDirty(this);
		}

		///Editor: Override to show custom controls whenever the ShowTaskEditGUI is called. By default controls will automaticaly show for most types
		virtual protected void OnActionEditGUI(){

			EditorUtils.ShowAutoEditorGUI(this);
		}

		public static string CreateNewActionScript(string name){
			
			string template =
			"using UnityEngine;\n" +
			"using NodeCanvas;\n" +
			"using NodeCanvas.Variables;\n\n" + 
			"public class " + name + " : ActionTask {\n\n" +
			"\tprotected override void OnExecute(){\n" +
			"\t\tEndAction(true);\n" +
			"\t}\n\n" +
			"\tprotected override void OnUpdate(){\n" +
			"\t\t\n" + 
			"\t}\n\n" +
			"\tprotected override void OnStop(){\n" +
			"\t\t\n" +
			"\t}\n" + 
			"}";

			bool dirExists = System.IO.Directory.Exists(Application.dataPath + "/MyNCActions/");
			if (!dirExists)
				System.IO.Directory.CreateDirectory(Application.dataPath + "/MyNCActions/");

			var scriptPath = Application.dataPath + "/MyNCActions/" + name + ".cs";
			System.IO.File.WriteAllText(scriptPath, template);
			UnityEditor.AssetDatabase.ImportAsset("Assets/MyNCActions/" + name + ".cs");
			return scriptPath;
		}

		#endif
	}
}