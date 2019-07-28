using UnityEngine;
using System.Collections.Generic;

namespace NodeCanvas{

	public class MonoManager : MonoBehaviour {

		//This is actually faster than adding/removing to delegate
		private List<System.Action> updateMethods = new List<System.Action>();
		private List<System.Action> guiMethods = new List<System.Action>();
		private static bool isQuiting;

		private static MonoManager _current;
		public static MonoManager current{
			get
			{
				if (_current == null && !isQuiting)
					_current = new GameObject("_MonoManager").AddComponent<MonoManager>();

				return _current;			
			}

			private set {_current = value;}
		}

		public static void Create(){
			_current = current;
		}

		//This is actually faster than adding/removing to delegate
		public void AddMethod(System.Action method){
			updateMethods.Add(method);
		}

		//This is actually faster than adding/removing to delegate
		public void RemoveMethod(System.Action method){
			updateMethods.Remove(method);
		}

		public void AddGUIMethod(System.Action method){
			guiMethods.Add(method);
		}

		public void RemoveGUIMethod(System.Action method){
			guiMethods.Remove(method);
		}

		void OnApplicationQuit(){

			isQuiting = true;
		}

		void Awake(){

			if (_current != null && _current != this){
				DestroyImmediate(this.gameObject);
				return;
			}

			_current = this;
		}

		void Update(){

			for (int i = 0; i < updateMethods.Count; i++)
				updateMethods[i]();
		}

		void OnGUI(){

			for (int i = 0; i < guiMethods.Count; i++)
				guiMethods[i]();
		}
	}
}