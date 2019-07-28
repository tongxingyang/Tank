#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Keyboard Input")]
	[ScriptCategory("Input")]
	public class CheckKeyboardInput : ConditionTask{

		public enum PressTypes {KeyDown, KeyUp, KeyPressed}
		public PressTypes PressType= PressTypes.KeyDown;
		public KeyCode Key= KeyCode.Space;

		protected override string conditionInfo{
			get {return PressType.ToString() + " " + Key.ToString();}
		}

		protected override bool OnCheck(){

			bool pressed = false;

			switch(PressType){

				case(PressTypes.KeyDown):
					pressed = Input.GetKeyDown(Key);
				break;

				case(PressTypes.KeyUp):
					pressed = Input.GetKeyUp(Key);
				break;

				case(PressTypes.KeyPressed):

					pressed = Input.GetKey(Key);
				break;
			}

			return pressed;
		}
		

		////////////////////////////////////////
		///////////GUI AND EDITOR STUFF/////////
		////////////////////////////////////////
		#if UNITY_EDITOR

		protected override void OnConditionEditGUI(){

			EditorGUILayout.BeginHorizontal();
			PressType = (PressTypes)EditorGUILayout.EnumPopup(PressType);
			Key = (KeyCode)EditorGUILayout.EnumPopup(Key);
			EditorGUILayout.EndHorizontal();
		}

		#endif
	}
}