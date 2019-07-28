using UnityEngine;
using UnityEditor;
using System.Collections;
using NodeCanvas;
using NodeCanvas.BehaviourTree;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(BehaviourTreeOwner))]
	public class BehaviourTreeOwnerInspector : GraphOwnerInspector {

		BehaviourTreeOwner owner{
			get {return target as BehaviourTreeOwner; }
		}

		protected override void OnSpecifics(){
			owner.BT = (BTContainer)EditorGUILayout.ObjectField("Behaviour Tree", owner.BT, typeof(BTContainer), true);
		}

		protected override void OnExtraOptions(){
			
			owner.runForever = EditorGUILayout.Toggle("Run Forever", owner.runForever);

			if (owner.runForever)
				owner.updateInterval = EditorGUILayout.FloatField("Update Interval", Mathf.Max(0, owner.updateInterval) );

			if (Application.isPlaying && owner.BT != null){
			
				GUILayout.BeginVertical("box");
				
				GUILayout.Label("DEBUG");

				if (GUILayout.Button("Stop Behaviour"))
					owner.StopGraph();

				if (!owner.BT.isRunning && GUILayout.Button("Start/Continue Behaviour"))
					owner.StartGraph();

				if (owner.BT.isRunning && GUILayout.Button("Pause Behaviour"))
					owner.BT.PauseGraph();

				if (!owner.BT.isRunning && GUILayout.Button("Tick"))
					owner.Tick();
				
				GUILayout.EndVertical();
			}
		}
	}
}