using Assets.Script.Mvc.demo.model;

namespace Assets.Script.Mvc.demo.controller
{
    public class AddNumCommand : Command
    {
        private NumData numData;

        public override void OnRegister()
        {
            numData = Context.GetModel<NumData>();
        }

        public override void Execute(IEvent e)
        {
            int data = (e as Event<int>).Data;
            numData.Num += data;
        }
    }
}