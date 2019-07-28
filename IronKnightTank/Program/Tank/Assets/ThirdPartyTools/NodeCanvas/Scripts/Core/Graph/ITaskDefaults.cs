using UnityEngine;

namespace NodeCanvas{

	public interface ITaskDefaults{

		Component agent{ get; }
		Blackboard blackboard{ get; }

		void SendDefaults();
		void SendEvent(string eventName);
	}
}