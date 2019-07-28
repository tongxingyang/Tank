

namespace Assets.Editor.CampaignMapEditor
{
    using System;

    using Assets.Tools.Script.Editor.Tool;
    using UnityEditor;
    using UnityEngine;
    using System.IO;
    using Object = UnityEngine.Object;

    public class CampaignMapEditorWin : EditorWindow
    {
        // [MenuItem("Test/CampaignMap")]
        // public static void Open()
        // {
        //     var win = EditorWindow.GetWindow<CampaignMapEditorWin>();
        //     win.Show();
        //     win.titleContent = new GUIContent("CampaignMap");
        // }

        public static CampaignMapEditorWin Instance;


        public Rect mapArea = new Rect(0 , 50, 160 * 8 , 90 * 8);

        private  CampaignMap map;

        private bool isChoseNode = false;

        private bool isMouseDown = false;

        private bool isLeftMouseDown;

        private bool isRightMouseDown;


        public static object CurrentSelection;

        public static CampaignNode curNode;

        public static NodeConnection CurrentConnection;

        private GUISkin skin;

        private string saveLuaFilePath;

        private void OnEnable()
        {
            //map = new CampaignMap();
        }

        public void Init(CampaignMap map , string saveLuaFilePath)
        {
            this.map = map;
            Instance = this;
            map.InitResource();
            skin = Resources.Load("NodeCanvasSkin") as GUISkin;
            CurrentSelection = map as object;
            this.saveLuaFilePath = saveLuaFilePath;
            for (int i = 0; i < map.NodeList.Count; i++)
            {
                var node = map.NodeList[i];
                node.NodeRect = new Rect(this.mapArea.x + node.PosX, this.mapArea.y + node.PosY, 50, 50);
                if(node.PreNodeIds != null)
                {
                    for (int j = 0; j < node.PreNodeIds.Count; j++)
                    {
                        var targetNode = this.GetTargetNode(node.PreNodeIds[j]);
                        node.NodeConnections.Add(new NodeConnection { source = node, target = targetNode });
                    }
                }
                
            }
        }

        private void OnGUI()
        { 
            if(Event.current.type == EventType.KeyDown && Event.current.control && Event.current.keyCode == KeyCode.D)
            {
                Debug.Log("prefss");
            }
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    DrawMap();
                    DrawNodeList();
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                {
                    DrawInspector();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.S && Event.current.control)
            {
                SaveMap();
                Event.current.Use();
            }
        }

        /// <summary>
        /// 保存地图
        /// </summary>
        private void SaveMap()
        {
            string serializeLua = "";
            var suc = this.map.SerialzeLua(out serializeLua); //BattleMap.SerializeLua(this.battleMap);
            if (!suc)
            {
                this.ShowNotification(new GUIContent("序列化失败，请检查资源"));
            }
            else
            {
                File.WriteAllText(this.saveLuaFilePath, serializeLua);
                this.ShowNotification(new GUIContent("保存成功"));
            }
        }


        /// <summary>
        /// 绘制地图
        /// </summary>
        private void DrawMap()
        {
            GUILayout.Label("战役地图".SetSize(20));
            GUITool.Line(3);
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("地图属性", EditorStyles.toolbarButton))//, GUILayout.Width(this.mapArea.width / 2)))
                {
                    this.isChoseNode = false;
                    CurrentSelection = map;
                }
                if (GUILayout.Button("创建节点", EditorStyles.toolbarButton))//, GUILayout.Width(this.mapArea.width / 2)))
                {
                    this.CreateNode();
                }
            }
            GUILayout.EndHorizontal();
            if (map.BgTexture)
            {
                var oldColor = GUI.color;
                GUI.color = Color.black;
                GUILayout.Box(map.BgTexture, GUILayout.Height(this.mapArea.height), GUILayout.Width(this.mapArea.width));
                GUI.color = oldColor;
                GUI.DrawTexture(this.mapArea, map.BgTexture);
            }
            else
            {
                var oldColor = GUI.color;
                GUI.color = Color.grey;
                GUILayout.Button( "无选中图片" , GUILayout.Height(this.mapArea.height) , GUILayout.Width(this.mapArea.width));
                GUI.color = oldColor;
            }
        }

        private void DrawNodeList()
        {
            CampaignNode[] nodeArray = map.NodeList.ToArray();
            for (int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i].DrawNode();
            }
            var oldSkin = GUI.skin;
            GUI.skin = this.skin;
            for (int i = 0; i < nodeArray.Length; i++)
            {
                nodeArray[i].DrawConnection();
            }
            GUI.skin = oldSkin;
        }

        private Port portPos;

        
        



        private void DrawInspector()
        {
            if (CurrentSelection.GetType() == typeof(CampaignMap))
            {
                this.DrawMapInspector();
            }
            else if(CurrentSelection.GetType() == typeof(CampaignNode))
            {
                this.DrawNodeInspector(CurrentSelection as CampaignNode);
            }
            else if (CurrentSelection.GetType() == typeof(NodeConnection))
            {
                this.DrawConnectionInspector(CurrentSelection as NodeConnection);
            }
        }

        private void DrawMapInspector()
        {
            GUILayout.BeginVertical(new GUIStyle("WindowBackground") , GUILayout.Height(500));
            GUILayout.Label("地图属性".SetSize(20));
            GUITool.Line(3);
            this.mapArea.width = EditorGUILayout.FloatField("地图宽度", this.mapArea.width);
            this.mapArea.height = EditorGUILayout.FloatField("地图高度", this.mapArea.height);
            map.BgTexture = EditorGUILayout.ObjectField(new GUIContent("地图贴图") ,(Object)map.BgTexture, typeof(Texture) , false) as Texture;
            GUILayout.EndVertical();
        }

        private void DrawNodeInspector(CampaignNode node)
        {
            GUILayout.BeginVertical(new GUIStyle("WindowBackground"), GUILayout.Height(500));
            GUILayout.Label("节点属性".SetSize(20));
            GUITool.Line(3);
            if (node != null)
            {
                node.ID = EditorGUILayout.IntField("节点ID", node.ID);
                node.PosX = EditorGUILayout.FloatField("PosX", node.PosX);
                node.PosY = EditorGUILayout.FloatField("PosY", node.PosY);
                node.Texture = EditorGUILayout.ObjectField(
                                           new GUIContent("地图贴图"),
                                           (Object)node.Texture,
                                           typeof(Texture),
                                           false) as Texture;
            }
            else
            {
                GUILayout.Label("node null wrong");
            }

            GUILayout.EndVertical();
        }

        private void DrawConnectionInspector(NodeConnection con)
        {
            GUILayout.BeginVertical(new GUIStyle("WindowBackground"), GUILayout.Height(500));
            GUILayout.Label("链接属性".SetSize(20));
            GUITool.Line(3);
            if(con != null)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("来源ID：" + con.source.ID);
                    if (GUILayout.Button("S", GUILayout.Width(20)))
                    {
                        CurrentSelection = con.source;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("目标ID: " + con.target.ID);
                    if (GUILayout.Button("S", GUILayout.Width(20)))
                    {
                        CurrentSelection = con.target;
                    }
                }
                GUILayout.EndHorizontal();

                
            }
        }


        private void CreateNode()
        {
            CampaignNode data = new CampaignNode();
            data.NodeRect = new Rect(this.mapArea.x + data.PosX , this.mapArea.y + data.PosY , 50 , 50 );
            map.NodeList.Add(data);
        }


        public  void CopyNode(CampaignNode node)
        {
            CampaignNode newNode = new CampaignNode(node);
            newNode.PosX += 10;
            newNode.PosY += 10;
            newNode.NodeRect.x += 10;
            newNode.NodeRect.y += 10;
            map.NodeList.Add(newNode);
        }

        public  void DeleteNode(CampaignNode node)
        {
            map.NodeList.Remove(node);
            for (int i = 0; i < map.NodeList.Count; i++)
            {
                for (int j = 0; j < map.NodeList[i].NodeConnections.ToArray().Length; j++)
                {
                    var connect = map.NodeList[i].NodeConnections[j];
                    if(connect.target == node)
                    {
                        map.NodeList[i].RemoveConnection(connect);
                    }
                }

            }
        }
        
        public CampaignNode GetClickNode(Vector2 pos)
        {
            for (int i = 0; i < map.NodeList.Count; i++)
            {
                if (map.NodeList[i].NodeRect.Contains(pos))
                {
                    return map.NodeList[i];
                }
            }
            return null;
        }

        public CampaignNode GetTargetNode(int id)
        {
            for (int i = 0; i < map.NodeList.Count; i++)
            {
                if(map.NodeList[i].ID == id)
                {
                    return map.NodeList[i];
                }
            }
            return null;
        }

        private void OnDestroy()
        {
            
        }

        public class Port
        {
            public CampaignNode parent;

            public Vector3 pos;
        }
    }
}
