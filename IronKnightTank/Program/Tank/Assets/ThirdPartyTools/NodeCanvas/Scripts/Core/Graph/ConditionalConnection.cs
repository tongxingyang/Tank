#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas{

	[AddComponentMenu("")]
	public class ConditionalConnection : ConnectionBase{

		[SerializeField]
		private ConditionTask _condition;

		public ConditionTask condition{
			get {return _condition;}
			set
			{
				_condition = value;
				if (_condition != null)
					_condition.SetOwnerDefaults(graph);
			}
		}

		protected override NodeStates OnExecute(Component agent, Blackboard blackboard){

			if (!condition || condition.CheckCondition(agent, blackboard))
				return targetNode.Execute(agent, blackboard);

			targetNode.ResetNode();
			return NodeStates.Failure;
		}

		public bool CheckCondition(){
			return CheckCondition(graphAgent, graphBlackboard);
		}

		public bool CheckCondition(Component agent){
			return CheckCondition(agent, graphBlackboard);
		}

		//to be used if and when want to just check the connection without execution. The connection state change here is purely for GUI since it will always change when connection gets executed
		public bool CheckCondition(Component agent, Blackboard blackboard){

			if ( !isDisabled && (!condition || condition.CheckCondition(agent, blackboard) ) )
				return true;

			connectionState = NodeStates.Failure;
			return false;
		}


		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		[SerializeField]
		private bool _showConditionsGUI;

		protected override void OnConnectionGUI(){

			Event e = Event.current;

			var alpha = (NodeGraphContainer.currentSelection != this && condition == null)? 0.1f : 0.8f;

			GUI.color = new Color(1f,1f,1f,alpha);

			string textToShow= condition? condition.taskInfo : "No Condition";
			textToShow = _showConditionsGUI? textToShow : (condition? "-||-" : "---");

			Vector2 finalSize= new GUIStyle("Box").CalcSize(new GUIContent(textToShow));
			areaRect.width = finalSize.x;
			areaRect.height = finalSize.y;

			if (e.button == 1 && e.type == EventType.MouseDown && areaRect.Contains(e.mousePosition)){
				_showConditionsGUI = !_showConditionsGUI;
				e.Use();
			}

			GUI.Box(areaRect, textToShow);

			GUI.color = Color.white;
			GUI.backgroundColor = Color.white;
		}

		protected override void OnConnectionInspectorGUI(){

			if (!condition){
				EditorUtils.ShowComponentSelectionButton(gameObject, typeof(ConditionTask), delegate(Component c){condition = (ConditionTask)c;});
				return;
			}
			
			if (EditorUtils.TaskTitlebar(condition))
				condition.ShowTaskEditGUI();
		}

		#endif
	}
}