using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[ScriptName("Check Float")]
	[ScriptCategory("Interop")]
	public class CheckFloat : ConditionTask{

		public enum CheckTypes
		{
			EqualTo,
			GreaterThan,
			LessThan
		}
		public BBFloat valueA = new BBFloat(){useBlackboard = true};
		public CheckTypes checkType = CheckTypes.EqualTo;
		public BBFloat valueB = new BBFloat();

		protected override string conditionInfo{
			get
			{
				string symbol = " == ";
				if (checkType == CheckTypes.GreaterThan)
					symbol = " > ";
				if (checkType == CheckTypes.LessThan)
					symbol = " < ";
				return valueA.ToString() + symbol + valueB.ToString();
			}
		}

		protected override bool OnCheck(){

			if (checkType == CheckTypes.EqualTo){
				if (Mathf.Abs(valueA.value - valueB.value) <= 0.05)
					return true;
				return false;
			}

			if (checkType == CheckTypes.GreaterThan){
				if (valueA.value > valueB.value)
					return true;
				return false;
			}

			if (checkType == CheckTypes.LessThan){
				if (valueA.value < valueB.value)
					return true;
				return false;
			}

			return true;
		}
	}
}