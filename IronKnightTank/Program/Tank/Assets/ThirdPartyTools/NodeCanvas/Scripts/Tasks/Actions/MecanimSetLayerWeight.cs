﻿using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[ScriptCategory("Mecanim")]
	[ScriptName("Set Layer Weight")]
	[AgentType(typeof(Animator))]
	public class MecanimSetLayerWeight : ActionTask {

		public int layerIndex;
		public BBFloat layerWeight;

		[SliderField(0,1)]
		public float transitTime;

		[GetFromAgent]
		private Animator animator;

		private float currentValue;

		protected override string actionInfo{
			get {return "Set Layer " + layerIndex + " weight " + layerWeight;}
		}

		protected override void OnExecute(){

			currentValue = animator.GetLayerWeight(layerIndex);
		}

		protected override void OnUpdate(){

			animator.SetLayerWeight(layerIndex, Mathf.Lerp(currentValue, layerWeight.value, elapsedTime/transitTime));

			if (elapsedTime >= transitTime)
				EndAction(true);
		}
	}
}