namespace Assets.Framework.Lua.LuaUi.Event
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using Assets.Framework.Lua.LuaUi.Event.Trigger;
    using Assets.Tools.Script.Reflec;

    using LuaInterface;

    using UnityEngine;

    public static class LuaEventTriggerHelper
    {
        private static Dictionary<string, ILuaEventTrigger> _triggerTypes; 
        private static Dictionary<string, ILuaEventTrigger> triggerTypes
        {
            get
            {
                if (_triggerTypes == null)
                {
                    _triggerTypes = new Dictionary<string, ILuaEventTrigger>();
                    var luauiTriggers = AssemblyTool.FindTypesInCurrentDomainWhereExtend<ILuaEventTrigger>();
                    foreach (var luauiTrigger in luauiTriggers)
                    {
                        var luaUiEventTrigger = ReflecTool.Instantiate(luauiTrigger) as ILuaEventTrigger;
                        _triggerTypes.Add(luaUiEventTrigger.EventName, luaUiEventTrigger);
                    }
                    
                    
                }
                return _triggerTypes;
            }
            
        }

        public static ILuaEventTrigger Create(this LuaUi.LuaUiEvent luaEvent,GameObject root, LuaFunction handler, LuaTable self)
        {
            return triggerTypes[luaEvent.EventType].Binding(root, handler, self);
        }

#if UNITY_EDITOR

        private static string DefaultSignature = "(arg)";

        private static List<string> EventList = null;

        public static string GetSignature(this LuaUi.LuaUiEvent luaEvent)
        {
            string signature = DefaultSignature;
            ILuaEventTrigger trigger;
            var tryGetValue = triggerTypes.TryGetValue(luaEvent.EventType, out trigger);
            if (tryGetValue)
            {
                signature = trigger.HandlerSignature;
            }
            return signature;
        }

        public static List<string> GetEventList()
        {
            if (EventList == null)
            {
                EventList = new List<string>();
                foreach (var luaUiEventTrigger in triggerTypes)
                {
                    EventList.Add(luaUiEventTrigger.Key);
                }
            }
            return EventList;
        }
        
#endif
    }
}