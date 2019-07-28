using UnityEngine;
using UnityEditor;
using System.Collections;
using NodeCanvas;
using NodeCanvas.FSM;

namespace NodeCanvasEditor{

	[CustomEditor(typeof(FSMOwner))]
	public class FSMOwnerInspector : GraphOwnerInspector {

		FSMOwner owner{
			get {return target as FSMOwner; }
		}

		protected override void OnSpecifics(){
			owner.FSM = (FSMContainer)EditorGUILayout.ObjectField("FSM", owner.FSM, typeof(FSMContainer), true);
		}

		protected override void OnExtraOptions(){
			
			if (Application.isPlaying && owner.FSM != null){
			
				GUILayout.BeginVertical("box");
				
				GUILayout.Label("DEBUG");

				if (GUILayout.Button("Stop FSM"))
					owner.StopGraph();

				if (!owner.FSM.isRunning && GUILayout.Button("Start FSM"))
					owner.StartGraph();

				GUILayout.EndVertical();
			}
		}
	}
}