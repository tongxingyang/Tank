#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	[ScriptName("Action List")]
	[ScriptCategory("Systems")]
	[ExecuteInEditMode]
	public class ActionList : ActionTask{

		public List<ActionTask> actions = new List<ActionTask>();
		public bool runInParallel;
		
		private int currentActionIndex;

		public override float estimatedLength{
			get
			{
				float total = 0;
				foreach (ActionTask action in actions)
					total += action.estimatedLength;
				return total;			
			}
		}

		protected override string actionInfo{
			get
			{
				if (actions.Count == 0)
					return "No Actions";

				string finalText= string.Empty;
				for (int i= 0; i < actions.Count; i++)
					finalText += (actions[i].isRunning? "► " : "") + actions[i].taskInfo + (i == actions.Count -1? "" : "\n" );

				return finalText;			
			}
		}

		protected override void OnExecute(){

			if (actions.Count == 0){

				EndAction(false);
				return;
			}

			currentActionIndex = 0;
			
			if (runInParallel){
		
				for (int i= 0; i < actions.Count; i++)
					actions[i].ExecuteAction(agent, blackboard, OnNestedActionEnd);

			} else {

				actions[0].ExecuteAction(agent, blackboard, OnNestedActionEnd);
			}
		}

		//This is the callback from a nested action
		private void OnNestedActionEnd(System.ValueType didSucceed){

			if (!(bool)didSucceed){
				EndAction(false);
				return;
			}

			currentActionIndex ++;

			if (runInParallel){

				if (currentActionIndex == actions.Count){
					EndAction(true);
					return;
				}

			} else {

				if (currentActionIndex < actions.Count)
					actions[currentActionIndex].ExecuteAction(agent, blackboard, OnNestedActionEnd);
				else
					EndAction(true);
			}
		}

		protected override void OnStop(){

			foreach (ActionTask action in actions){
				if (action.isRunning)
					action.EndAction(false);
			}
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		private void OnDestroy(){

			foreach(ActionTask action in actions){
				var a = action;
				EditorApplication.delayCall += ()=>
				{
					if (a) DestroyImmediate(a, true);
				};
			}
		}

		public override Task CopyTo(GameObject go){

			if (this == null)
				return null;

			ActionList copiedList = (ActionList)go.AddComponent<ActionList>();
			UnityEditor.EditorUtility.CopySerialized(this, copiedList);
			copiedList.actions.Clear();

			foreach (ActionTask action in actions){
				var copiedAction = action.CopyTo(go);
				copiedList.AddAction(copiedAction as ActionTask);
			}

			return copiedList;
		}

		protected override void OnActionEditGUI(){

			ShowListGUI();
			ShowNestedActionsGUI();

			if (GUI.changed && this != null)
	            EditorUtility.SetDirty(this);
		}

		//The action list gui
		public void ShowListGUI(){

			if (this == null)
				return;

			EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ActionTask), delegate(Component a){ AddAction((ActionTask)a); });
			EditorGUILayout.Space();

			//Check first for possibly removed components
			foreach (ActionTask action in actions.ToArray()){
				if (action == null)
					actions.Remove(action);
			}

			if (actions.Count == 0){
				EditorGUILayout.HelpBox("Please add some Actions", MessageType.Info);
				return;
			}

			//Then present them
			for (int i= 0; i < actions.Count; i++){

				ActionTask action= actions[i];

				EditorGUILayout.BeginHorizontal();
					GUI.backgroundColor = new Color(1, 1, 1, 0.25f);

					if (i != actions.Count -1 && actions.Count != 1){
					
						if (GUILayout.Button("▼", GUILayout.Width(25), GUILayout.Height(20))){
							
							actions.Remove(action);
							actions.Insert(i+1, action);
							//ReorderComponents();
						}

					} else {

						GUILayout.Box("▬", GUILayout.Width(25), GUILayout.Height(20));
					}

					if (i != 0){
					
						if (GUILayout.Button("▲", GUILayout.Width(25), GUILayout.Height(20))){
							
							actions.Remove(action);
							actions.Insert(i-1, action);
							//ReorderComponents();
						}
					
					} else {

						GUILayout.Box("▬", GUILayout.Width(25), GUILayout.Height(20));
					}

					GUILayout.Label((action.isRunning? "► ": "") + action.taskInfo);

					GUILayout.FlexibleSpace();

					GUI.backgroundColor = EditorUtils.lightRed;
					if (GUILayout.Button("X", GUILayout.Width(20))){
						actions.Remove(action);
						DestroyImmediate(action);
					}
					GUI.backgroundColor = Color.white;
				EditorGUILayout.EndHorizontal();
			}
		}

		private void ReorderComponents(){

			foreach (ActionTask action in actions.ToArray()){
				ActionTask newAction= gameObject.AddComponent(action.GetType()) as ActionTask;
				EditorUtility.CopySerialized(action, newAction);
				DestroyImmediate(action);
				actions.Add(newAction);
			}		
		}

		public void ShowNestedActionsGUI(){

			foreach (ActionTask action in actions){
				EditorUtils.BoldSeparator();
				if (EditorUtils.TaskTitlebar(action))
					action.ShowTaskEditGUI();
			}		
		}

		private void AddAction(ActionTask action){
			actions.Add(action);
			action.SetOwnerDefaults(ownerSystem);
		}

		#endif
	}
}