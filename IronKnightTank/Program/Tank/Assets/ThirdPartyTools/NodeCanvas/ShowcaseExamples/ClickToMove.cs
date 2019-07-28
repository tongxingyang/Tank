using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class ClickToMove : MonoBehaviour {

	private UnityEngine.AI.NavMeshAgent navAgent;

	void Awake(){
		navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}

	void Update(){

		if (Input.GetMouseButtonDown(0)){

			RaycastHit hit;
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
				navAgent.SetDestination(hit.point);
		}
	}
}
