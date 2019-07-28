﻿using UnityEngine;
using System.Collections;

namespace NodeCanvas.Conditions{

	[ScriptCategory("System Events")]
	[AgentType(typeof(Collider))]
	[EventListener("OnCollisionEnter", "OnCollisionExit", "OnCollisionStay")]
	public class CheckCollision : ConditionTask {

		public enum CheckTypes
		{
			CollisionEnter = 0,
			CollisionExit  = 1,
			CollisionStay  = 2
		}

		public CheckTypes checkType = CheckTypes.CollisionEnter;
		public bool specifiedTagOnly;
		[TagField]
		public string objectTag = "Untagged";

		private int current = -1;

		protected override string conditionInfo{
			get {return checkType.ToString() + ( specifiedTagOnly? (" '" + objectTag + "' tag") : "" );}
		}

		protected override bool OnCheck(){

			return (int)checkType == current;
		}

		void OnCollisionEnter(Collision collisionInfo){

			if (!specifiedTagOnly || collisionInfo.gameObject.tag == objectTag){
				current = 0;
				StartCoroutine(ResetCurrent());
			}
		}

		IEnumerator OnCollisionExit(Collision collisionInfo){
			
			if (!specifiedTagOnly || collisionInfo.gameObject.tag == objectTag){
				yield return null;
				current = 1;
				StartCoroutine(ResetCurrent());
			}
		}

		IEnumerator OnCollisionStay(Collision collisionInfo){

			if (!specifiedTagOnly || collisionInfo.gameObject.tag == objectTag){
				yield return null;
				current = 2;
			}
		}

		IEnumerator ResetCurrent(){
			yield return null;
			current = -1;
		}
	}
}