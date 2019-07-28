#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	[ScriptName("Conditions List")]
	[ScriptCategory("Systems")]
	[ExecuteInEditMode]
	public class ConditionsList : ConditionTask{

		public List<ConditionTask> conditions = new List<ConditionTask>();
		public bool allSuccessRequired = true;

		override protected string conditionInfo{
			get
			{
				string finalText = conditions.Count != 0? "" : "No Conditions";
				if (conditions.Count > 1)
					finalText += "<b>(" + (allSuccessRequired? "ALL True" : "ANY True") + ")</b>\n";

				for (int i= 0; i < conditions.Count; i++)
					finalText += conditions[i].taskInfo + (i == conditions.Count -1? "" : "\n" );
				return finalText;
			}
		}

		override protected bool OnCheck(){

			int succeedChecks = 0;

			foreach (ConditionTask con in conditions){

				if (con.CheckCondition(agent, blackboard)){

					if (!allSuccessRequired)
						return true;

					succeedChecks ++;
				}
			}

			return succeedChecks == conditions.Count;
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		private void OnDestroy(){

			foreach(ConditionTask condition in conditions){
				var c = condition;
				EditorApplication.delayCall += ()=>
				{
					if (c) DestroyImmediate(c, true);
				};
			}
		}

		public override Task CopyTo(GameObject go){

			if (this == null)
				return null;

			ConditionsList copiedList = (ConditionsList)go.AddComponent<ConditionsList>();
			UnityEditor.EditorUtility.CopySerialized(this, copiedList);
			copiedList.conditions.Clear();

			foreach (ConditionTask condition in conditions){
				var copiedCondition = condition.CopyTo(go);
				copiedList.AddCondition(copiedCondition as ConditionTask);
			}

			return copiedList;
		}

		override protected void OnConditionEditGUI(){

			ShowListGUI();
			ShowNestedConditionsGUI();

			if (GUI.changed && this != null)
	            EditorUtility.SetDirty(this);
		}

		public void ShowListGUI(){

			if (this == null)
				return;

			EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){ AddCondition((ConditionTask)c) ;});

			//Check for possibly removed components
			foreach (ConditionTask condition in conditions.ToArray()){
				if (condition == null)
					conditions.Remove(condition);
			}

			foreach (ConditionTask con in conditions.ToArray()){

				GUILayout.BeginHorizontal();
					GUILayout.Label(con.taskInfo);
					GUI.backgroundColor = EditorUtils.lightRed;
					if (GUILayout.Button("X", GUILayout.MaxWidth(20))){
						conditions.Remove(con);
						DestroyImmediate(con);			
					}
					GUI.backgroundColor = Color.white;
				GUILayout.EndHorizontal();
			}

			EditorUtils.Separator();

			if (conditions.Count > 1){
				GUI.backgroundColor = new Color(0.5f,0.5f,0.5f);
				if (GUILayout.Button(allSuccessRequired? "ALL True Required":"ANY True Suffice"))
					allSuccessRequired = !allSuccessRequired;
				GUI.backgroundColor = Color.white;
			}
		}


		public void ShowNestedConditionsGUI(){

			foreach (ConditionTask condition in conditions){
				EditorUtils.BoldSeparator();
				if (EditorUtils.TaskTitlebar(condition))
					condition.ShowTaskEditGUI();			
			}
		}

		public void AddCondition(ConditionTask condition){
			conditions.Add(condition);
			condition.SetOwnerDefaults(ownerSystem);
		}

		#endif
	}
}