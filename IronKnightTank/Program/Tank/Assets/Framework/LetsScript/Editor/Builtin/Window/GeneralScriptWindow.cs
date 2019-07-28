namespace Assets.Framework.LetsScript.Editor.Builtin.Window
{
    using System.Collections.Generic;

    using Assets.Framework.LetsScript.Editor;
    using Assets.Framework.LetsScript.Editor.Builtin.Window.Part;
    using Assets.Framework.LetsScript.Editor.Data;
    using Assets.Framework.LetsScript.Editor.Script;
    using Assets.Framework.LetsScript.Editor.Script.Editor;
    using Assets.Tools.Script.Editor.Tool;
    using Assets.Tools.Script.Editor.Window;

    using UnityEditor;

    using UnityEngine;

    [ScriptEditorWindow("RPGScript")]
    public class GeneralScriptWindow : ScriptEditorWindow
    {
        private static CommonContent ClipboardEventGroup;

        [MenuItem("Assets/LetsScript/GeneralScrip")]
        public static void CreateTestScript()
        {
            LetsScriptEditor.CreateScript("RPGScripTemplate");
//            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
//            var templatePath = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("RPGScripTemplate")[0]);
//            LetsScriptEditor.CreateScript("Assets/Editor/LetsScript/Template/RPGScripTemplate.lua", path + "/" + "GeneralScrip.lua");
        }

        private CommandRendererPart actionPart;
        private CommandRendererPart conditionPart;
        private TirggerPointEnumPart pointPart;

        private int currIndex = 1;
        

        protected override void OnInit()
        {
            this.titleContent = new GUIContent(this.Data.ScriptName);

            this.autoRepaintOnSceneChange = true;
            this.currIndex = 1;
            this.actionPart = new CommandRendererPart("执行动作", 20, new ContentProperty()
            {
                PropertyType = ContentType.List(ContentType.Action),
                PropertyName = "ActionList1",
            });
            this.conditionPart = new CommandRendererPart("触发条件", 10, new ContentProperty()
            {
                PropertyType = ContentType.List(ContentType.Boolean),
                PropertyName = "ConditionList1",
            });
            this.pointPart = new TirggerPointEnumPart(new ContentProperty()
            {
                PropertyName = "TriggerPoint1",
            });
            base.OnInit();
        }

        protected  List<ItemDetailPartInspector<ScriptData>> CreatePartInspector()
        {
            var partInspectors = new List<ItemDetailPartInspector<ScriptData>>();
            partInspectors.Add(this.pointPart);
            partInspectors.Add(this.conditionPart);
            partInspectors.Add(this.actionPart);

            return partInspectors;
        }

        protected override void ShowDetail()
        {
            var count = this.Data.Contents.GetChildContent("GroupCount").AsValue<int>();
            if (this.currIndex > count)
            {
                this.SelectGroup(1);
            }
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftArrow && Event.current.control)
            {
                this.MoveGroup(this.currIndex, this.currIndex - 1);
                Event.current.Use();
            }
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.RightArrow && Event.current.control)
            {
                this.MoveGroup(this.currIndex, this.currIndex + 1);
                Event.current.Use();
            }
            base.ShowDetail();
        }

        protected override void ShowMenuLeft()
        {
            var count = this.Data.Contents.GetChildContent("GroupCount").AsValue<int>();
            var nameContent = this.Data.Contents.GetChildContent("GroupName");
            var names = nameContent.AsList();

            for (int i = 1; i <= count; i++)
            {
                if (GUITool.Button(names[i - 1].AsValue<string>(), this.currIndex != i ? GUI.backgroundColor : Color.red, GUILayout.MinWidth(40)))
                {
                    this.SelectGroup(i);
                    //右键
                    if (Event.current.button == 1 )
                    {
                        this.EditEventTitle(i, names, count);
                    }
                }
            }
            if (GUILayout.Button("+", EditorStyles.toolbarButton, GUILayout.Width(40)))
            {
                this.NewEventTitle();
            }

            base.ShowMenuLeft();
        }

        private void SelectGroup(int index)
        {
            this.actionPart.Property.PropertyName = "ActionList" + index;
            this.conditionPart.Property.PropertyName = "ConditionList" + index;
            this.pointPart.Property.PropertyName = "TriggerPoint" + index;
            this.currIndex = index;
        }

        private void AddGroup(string groupName, CommonContent actionList, CommonContent conditionList, CommonContent triggerPoint)
        {
            var count = this.Data.Contents.GetChildContent("GroupCount").AsValue<int>();
            var nameContent = this.Data.Contents.GetChildContent("GroupName");

            Undo.RecordObjects(new[] { this.Data.Contents, nameContent }, "add group");
            nameContent.SetChildContent(count, new CommonContent().FromValue(groupName));

            this.Data.Contents.SetChildContent("GroupCount",new CommonContent().FromValue(count + 1));
            this.Data.Contents.SetChildContent("ActionList" + (count + 1), actionList);
            this.Data.Contents.SetChildContent("ConditionList" + (count + 1), conditionList);
            this.Data.Contents.SetChildContent("TriggerPoint" + (count + 1), triggerPoint);
            this.SelectGroup(count + 1);
        }

        private void RemoveGroup(int index, int count)
        {
            var nameContent = this.Data.Contents.GetChildContent("GroupName");
            Undo.RecordObjects(new []{ this.Data.Contents, nameContent }, "remove group");
            this.Data.Contents.SetChildContent("GroupCount", new CommonContent().FromValue(count - 1));

            //移除
            this.Data.Contents.RemoveContent(this.Data.Contents.GetChildContent("ActionList" + index));
            this.Data.Contents.RemoveContent(this.Data.Contents.GetChildContent("ConditionList" + index));
            this.Data.Contents.RemoveContent(this.Data.Contents.GetChildContent("TriggerPoint" + index));
            nameContent.RemoveContent(nameContent.GetChildContent(index - 1));

            //移动位置
            for (int i = index + 1; i <= count; i++)
            {
                this.Data.Contents.SetChildContent("ActionList" + (i - 1), this.Data.Contents.GetChildContent("ActionList" + (i)));
                this.Data.Contents.SetChildContent("ConditionList" + (i - 1), this.Data.Contents.GetChildContent("ConditionList" + (i)));
                this.Data.Contents.SetChildContent("TriggerPoint" + (i - 1), this.Data.Contents.GetChildContent("TriggerPoint" + (i)));
            }

            if (index == count || index == this.currIndex)
            {
                this.SelectGroup(1);
            }
        }

        private void MoveGroup(int from, int to)
        {
            var count = this.Data.Contents.GetChildContent("GroupCount").AsValue<int>();
            if (to < 1 || to > count || from == to)
            {
                return;
            }
            var nameContent = this.Data.Contents.GetChildContent("GroupName");
            Undo.RecordObjects(new[] { this.Data.Contents, nameContent }, "remove group");

            var fromName = nameContent.GetChildContent(from - 1);
            var toName = nameContent.GetChildContent(to - 1);

            var fromActionList = this.Data.Contents.GetChildContent("ActionList" + from);
            var fromConditionList = this.Data.Contents.GetChildContent("ConditionList" + from);
            var fromTriggerPoint = this.Data.Contents.GetChildContent("TriggerPoint" + from);

            var toActionList = this.Data.Contents.GetChildContent("ActionList" + to);
            var toConditionList = this.Data.Contents.GetChildContent("ConditionList" + to);
            var toTriggerPoint = this.Data.Contents.GetChildContent("TriggerPoint" + to);

            nameContent.SetChildContent(from - 1,toName);
            nameContent.SetChildContent(to - 1,fromName);

            this.Data.Contents.SetChildContent("ActionList" + from, toActionList);
            this.Data.Contents.SetChildContent("ConditionList" + from, toConditionList);
            this.Data.Contents.SetChildContent("TriggerPoint" + from, toTriggerPoint);

            this.Data.Contents.SetChildContent("ActionList" + to, fromActionList);
            this.Data.Contents.SetChildContent("ConditionList" + to, fromConditionList);
            this.Data.Contents.SetChildContent("TriggerPoint" + to, fromTriggerPoint);

            this.SelectGroup(to);
        }

        private void NewEventTitle()
        {
            var count = this.Data.Contents.GetChildContent("GroupCount").AsValue<int>();
            var eventEditMenu = new PopCustomWindow();
            string currName = "事件"+(count + 1);
            eventEditMenu.DrawGUI = () =>
                {
                    GUILayout.BeginHorizontal(GUITool.GetAreaGUIStyle());
                    GUILayout.Label("添加事件".SetSize(18));
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("名字");
                    currName = EditorGUILayout.TextField(currName, GUILayout.Width(200));
                    
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("创建"))
                    {
                        this.AddGroup(currName, new CommonContent(), new CommonContent(), new CommonContent());
                        eventEditMenu.CloseWindow();
                    }
                    if (ClipboardEventGroup != null && GUILayout.Button("粘帖"))
                    {
                        this.AddGroup(currName, ClipboardEventGroup.GetChildContent("ActionList"), ClipboardEventGroup.GetChildContent("ConditionList"), ClipboardEventGroup.GetChildContent("TriggerPoint"));
                        eventEditMenu.CloseWindow();
                    }
                    GUILayout.EndHorizontal();
                };
            eventEditMenu.PopWindow();
        }

        private void EditEventTitle(int index, List<CommonContent> names, int count)
        {
            var eventEditMenu = new PopCustomWindow();
            string currName = names[index - 1].AsValue<string>();
            eventEditMenu.DrawGUI = () =>
            {
                GUILayout.BeginHorizontal(GUITool.GetAreaGUIStyle());
                GUILayout.Label("事件操作".SetSize(18));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("名字");
                currName = EditorGUILayout.TextField(currName,GUILayout.Width(200));
                if (GUILayout.Button("确定"))
                {
                    Undo.RecordObject(names[index - 1],"rename event");
                    names[index - 1].SetValue(currName);
                    eventEditMenu.CloseWindow();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("复制"))
                {
                    ClipboardEventGroup = new CommonContent();
                    ClipboardEventGroup.SetChildContent("Name", new CommonContent().FromValue(currName));
                    ClipboardEventGroup.SetChildContent("ActionList", new CommonContent().FromList(this.Data.Contents.GetChildContent("ActionList" + index).AsList()));
                    ClipboardEventGroup.SetChildContent("ConditionList", new CommonContent().FromList(this.Data.Contents.GetChildContent("ConditionList" + index).AsList()));
                    ClipboardEventGroup.SetChildContent("TriggerPoint", new CommonContent().FromList(this.Data.Contents.GetChildContent("TriggerPoint" + index).AsList()));
                    ClipboardEventGroup = ClipboardEventGroup.Clone();
                    eventEditMenu.CloseWindow();
                }

                if (index != 1 && GUILayout.Button("前移(Ctrl + LeftArrow)"))
                {
                    this.MoveGroup(index, index - 1);
                    eventEditMenu.CloseWindow();
                }
                if (index != count && GUILayout.Button("后移(Ctrl + RightArrow)"))
                {
                    this.MoveGroup(index, index + 1);
                    eventEditMenu.CloseWindow();
                }

                if (count > 1 && GUILayout.Button("删除"))
                {
                    this.RemoveGroup(index, count);
                    eventEditMenu.CloseWindow();
                }
            };
            eventEditMenu.PopWindow();
        }
    }
}
