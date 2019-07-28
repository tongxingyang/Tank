// ----------------------------------------------------------------------------
// <copyright file="TirggerPointSelectPart.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>HuHuiBin</author>
// <date>03/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Framework.LetsScript.Editor.Builtin.Window.Part
{
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor;
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    using UnityEngine;

    public class TirggerPointEnumPart : ItemDetailPartInspector<ScriptData>
    {
        public ContentProperty Property;


        public TirggerPointEnumPart(ContentProperty property)
        {
            this.Property = property;
        }

        public override string Title
        {
            get
            {
                return "事件触发点";
            }
        }

        protected override void OnShow(ScriptData item)
        {
            //
            var triggerPoints = item.Contents.GetChildContent(this.Property.PropertyName);
            var triggerPointEnum = this.GetTriggerPointEnum(item);

            var pointList = triggerPoints.AsList();
            for (int i = 0; i < pointList.Count; i++)
            {
                var point = pointList[i];
                GUILayout.BeginHorizontal();
                GUILayout.Label(this.GetTriggerPointLabel(point.AsValue<string>()).SetSize(16));
                if (GUILayout.Button("X",GUILayout.Width(40)))
                {
                    Undo.RecordObject(triggerPoints,"remove trigger point");
                    triggerPoints.RemoveContent(point);
                }
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("添加触发点"))
            {
                PopMenuWindow popMenu = new PopMenuWindow();
                popMenu.Gradable = true;
                popMenu.DefaultSize = new Vector2(200,400);
                for (int index = 0; index < triggerPointEnum.Count; index++)
                {
                    var point = triggerPointEnum[index];
                    popMenu.AddItem(point.Path, false,
                        () =>
                            {
                                Undo.RecordObject(triggerPoints, "add trigger point");
                                triggerPoints.InsertChildContent(new CommonContent().FromValue(point.Name), triggerPoints.AsList().Count);
                            });
                    popMenu.PopWindow();
                }
            }
        }

        public override int Order
        {
            get
            {
                return 5;
            }
        }

        private string GetTriggerPointLabel(string id)
        {
            foreach (var scriptTriggerPoint in LetsScriptSetting.TriggerPoint)
            {
                if (scriptTriggerPoint.Name == id)
                {
                    var strings = scriptTriggerPoint.Path.Split('/');
                    return strings[strings.Length - 1];
                }
            }
            return id;
        }

        private List<ScriptTriggerPoint> GetTriggerPointEnum(ScriptData item)
        {
            List<ScriptTriggerPoint> enums = new List<ScriptTriggerPoint>();
            
            var enumRange = item.Contents.GetChildContent("__TriggerPointEnum");
            if (enumRange != null)
            {
                var range = enumRange.AsList();
                foreach (var commonContent in range)
                {
                    var scriptTriggerPoint = LetsScriptSetting.GetTriggerPoint(commonContent.AsValue<string>());
                    if (scriptTriggerPoint == null)
                    {
                        Debug.LogErrorFormat("Can not fine a trigger point named '{0}'", commonContent.AsValue<string>());
                    }
                    else
                    {
                        enums.Add(scriptTriggerPoint);
                    }
                }
            }
            else
            {
                foreach (var scriptTriggerPoint in LetsScriptSetting.TriggerPoint)
                {
                    enums.Add(scriptTriggerPoint);
                }
            }

            return enums;
        }
    }
}