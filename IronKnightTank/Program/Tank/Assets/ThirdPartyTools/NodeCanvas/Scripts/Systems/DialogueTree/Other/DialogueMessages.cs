using System;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTree{

	public class DialogueOptionsInfo{

		public Dictionary<Statement, int> finalOptions = new Dictionary<Statement, int>();
		public float availableTime = 0;

		public Action<int> SelectOption;

		public DialogueOptionsInfo(Dictionary<Statement, int> finalOptions, float availableTime, Action<int> callback){
			this.finalOptions = finalOptions;
			this.availableTime = availableTime;
			this.SelectOption = callback;
		}
	}

	public class DialogueSpeechInfo{

		public DialogueActor actor;
		public Statement statement;
		
		public Action DoneSpeaking;

		public DialogueSpeechInfo(DialogueActor actor, Statement statement, Action callback){
			this.actor = actor;
			this.statement = statement;
			this.DoneSpeaking = callback;
		}
	}
}