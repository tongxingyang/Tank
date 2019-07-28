namespace Assets.Tools.Script.Mvc.demo
{
    using Assets.Script.Mvc;
    using Assets.Script.Mvc.demo;
    using Assets.Script.Mvc.demo.controller;
    using Assets.Script.Mvc.demo.model;
    using Assets.Script.Mvc.demo.view;

    using UnityEngine;

    public class TestStart : MonoBehaviour
    {
        public NumView NumView;

        private MvcContext mvcContext;

        void Start()
        {
            this.mvcContext = new MvcContext();
            this.mvcContext.RegisterModel<NumData>(new NumData());
            this.mvcContext.RegisterView<NumView>(this.NumView);
            this.mvcContext.RegisterCommand(TestEvent.AddNum, new AddNumCommand());
        }
    }
}