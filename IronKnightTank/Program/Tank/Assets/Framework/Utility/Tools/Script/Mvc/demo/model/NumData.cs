using UnityEngine.UI;

namespace Assets.Script.Mvc.demo.model
{
    public class NumData : Model
    {
        public int Num
        {
            get
            {
                return num;
            }
            set
            {
                num = value;
                this.EventDispatcher.DispatchEvent(TestEvent.NumChange, num);
            }
        }

        private int num;
    }
}