using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NodeCanvas.Actions{

	[ScriptName("Play Animation")]
	[ScriptCategory("Animation")]
	[AgentType(typeof(Animation))]
	public class PlayAnimationSimple : ActionTask{

		[RequiredField]
		public AnimationClip animationClip;
		[SliderField(0,1)]
		public float crossFadeTime = 0.25f;
		public WrapMode animationWrap= WrapMode.Loop;
		public bool waitUntilFinish;

		//holds the last played animationClip for each agent 
		//definetely not the best way to do it, but its a simple example
		private static Dictionary<Animation, AnimationClip> lastPlayedClips = new Dictionary<Animation, AnimationClip>();

		protected override string OnInit(){
			agent.GetComponent<Animation>().AddClip(animationClip, animationClip.name);
			return null;
		}

		protected override string actionInfo{
			get {return "PlayAnim '" + (animationClip? animationClip.name:"null") + "'";}
		}

		protected override void OnExecute(){

			if (lastPlayedClips.ContainsKey(agent.GetComponent<Animation>()) && lastPlayedClips[agent.GetComponent<Animation>()] == animationClip){
				EndAction(true);
				return;
			}

			lastPlayedClips[agent.GetComponent<Animation>()] = animationClip;
			agent.GetComponent<Animation>()[animationClip.name].wrapMode = animationWrap;
			agent.GetComponent<Animation>().CrossFade(animationClip.name, crossFadeTime);
			
			if (!waitUntilFinish)
				EndAction(true);
		}

		protected override void OnUpdate(){

			if (elapsedTime >= animationClip.length - crossFadeTime)
				EndAction(true);
		}
	}
}