namespace Assets.Editor.CampaignMapEditor
{
    using System;
    using System.Collections.Generic;
    using ParadoxNotion.Serialization;
    using UnityEditor;
    using Assets.Framework.Lua.Editor.Util;
    using UnityEngine;

    public class CampaignMap
    {
        public string MapBgTexturePath;
        
        [SerializeField]
        public List<CampaignNode> NodeList = new List<CampaignNode>();

        [NonSerialized]
        public Texture BgTexture;

        [NonSerialized]
        public Rect MapRect;

        public static CampaignMap DeserializeJson(string json)
        {
            var map = JSON.Deserialize<CampaignMap>(json);
            return map;
        }

        public  bool  SerializeJson(out string json)
        {
            json = "";
            MapBgTexturePath = AssetDatabase.GetAssetPath(this.BgTexture);
            if (!MapBgTexturePath.StartsWith("Assets//GameResource"))
            {
                Debug.Log("sprite wrong place");
                return false;
            }
            for (int i = 0; i < this.NodeList.Count; i++)
            {
                if (!this.NodeList[i].SerializePrepare())
                {
                    return false;
                }
            }
            json = JSON.Serialize<CampaignMap>(this);
            return true;
        }


        public void Init()
        {

        }

        public void InitResource()
        {
            this.BgTexture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/GameResource/" + this.MapBgTexturePath);
            for (int i = 0; i < this.NodeList.Count; i++)
            {
                this.NodeList[i].InitResource();
            }
        }

        private bool SerialzePrepar()
        {
            MapBgTexturePath = AssetDatabase.GetAssetPath(this.BgTexture);
            if (!MapBgTexturePath.StartsWith("Assets/GameResource/"))
            {
                Debug.Log("sprite wrong place");
                return false;
            }
            else
            {
                MapBgTexturePath = MapBgTexturePath.Replace("Assets/GameResource/", "");
            }
            for (int i = 0; i < this.NodeList.Count; i++)
            {
                var node = this.NodeList[i];
            }
            return true;
        }

        public bool SerialzeLua(out string luaStr)
        {
            luaStr = "";
            if (!this.SerialzePrepar())
            {
                return false;
            }
            for (int i = 0; i < this.NodeList.Count; i++)
            {
                if (!this.NodeList[i].SerializePrepare())
                {
                    return false;
                }
            }
            string serialze = LuaSerializer.Serialize(this);
            luaStr = string.Format("return {0}", serialze);
            return true;
        }

    }

    public class CampaignNode
    {
        public int ID;

        public string TexturePath;

        [SerializeField]
        public float PosX;

        [SerializeField]
        public float PosY;

        [SerializeField]
        public List<int> PreNodeIds = new List<int>();

        [NonSerialized]
        public Texture Texture;

        [NonSerialized]
        public List<NodeConnection> NodeConnections = new List<NodeConnection>();

        [NonSerialized]
        public Rect NodeRect;


        public CampaignNode()
        {

        }

        public CampaignNode(CampaignNode node)
        {
            this.ID = node.ID;
            this.TexturePath = node.TexturePath;
            this.Texture = node.Texture;
            this.PosX = node.PosX;
            this.PosY = node.PosY;
            this.NodeRect = new Rect(node.NodeRect.center  , node.NodeRect.size) ;
        }

        /// <summary>
        /// 本体绘制
        /// </summary>
        public void DrawNode()
        {
            var e = Event.current;
            if (CampaignMapEditorWin.CurrentSelection == this)
            {
                GUI.color = new Color(0.8f, 0.8f, 1);
            }

            GUI.Box(this.NodeRect, this.Texture);
            int id = GUIUtility.GetControlID(FocusType.Keyboard);
            var type = e.GetTypeForControl(id);
            switch (e.type)
            {
                case EventType.MouseDown:
                    if(e.isMouse && this.NodeRect.Contains(e.mousePosition))
                    {
                        if(e.button == 0)
                        {
                            CampaignMapEditorWin.CurrentSelection = this;
                            GUIUtility.hotControl = id;
                        }else if (e.button == 1)
                        {
                            GenericMenu menu = new GenericMenu();
                            menu.AddItem( new GUIContent("删除"), false , delegate() {
                                CampaignMapEditorWin.Instance.DeleteNode(this);
                            });
                            menu.AddItem(new GUIContent("Copy"), false, delegate () {
                                CampaignMapEditorWin.Instance.CopyNode(this);
                            });
                            menu.ShowAsContext();
                        }
                        e.Use();
                    }
                    break;
                case EventType.MouseDrag:
                    if(GUIUtility.hotControl == id)
                    {
                        CheckPan(e.delta);
                        e.Use();
                    }
                    break;
                case EventType.MouseUp:
                    if(GUIUtility.hotControl == id)
                    {
                        
                        GUIUtility.hotControl = 0;
                    }
                    break;
            }
            if(CampaignMapEditorWin.CurrentSelection == this )
            {
                if( e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete)
                {
                    CampaignMapEditorWin.Instance.DeleteNode(this);
                    e.Use();
                }else if (e.type == EventType.KeyDown && e.keyCode == KeyCode.D && e.control )
                {
                    Debug.Log("copy");
                }
                
                
            }
            GUI.color = Color.white;
        }

        private static Port clickedPort;

        class Port
        {
            public Vector2 pos;

            public Port(Vector2 pos)
            {
                this.pos = pos;
            }
        }
        

        /// <summary>
        /// 链接部分绘制
        /// </summary>
        public void DrawConnection()
        {
            Event e = Event.current;

            var nodeOutputBox = new Rect(NodeRect.x, NodeRect.yMax - 4, NodeRect.width, 12);
            GUI.Box(nodeOutputBox, "", new GUIStyle("nodePortContainer"));

            for (int i = 0; i < this.NodeConnections.Count + 1; i++)
            {
                Rect portRect = new Rect(0, 0, 10, 10);
                portRect.center = new Vector2(((NodeRect.width / (NodeConnections.Count + 1)) * (i + 0.5f)) + NodeRect.xMin, NodeRect.yMax + 6);
                GUI.Box(portRect, "", "nodePortEmpty");
                int id = GUIUtility.GetControlID(FocusType.Keyboard);
                var type = e.GetTypeForControl(id);
                switch (type)
                {
                    case EventType.MouseDown:
                        if (portRect.Contains(e.mousePosition))
                        {
                            GUIUtility.hotControl = id;
                            clickedPort = new Port( portRect.center);
                            e.Use();
                        }
                        break;
                    case EventType.MouseDrag:
                        if (GUIUtility.hotControl == id)
                        {
                            e.Use();
                        }
                        break;
                    case EventType.MouseUp:
                        if (GUIUtility.hotControl == id)
                        {
                            GUIUtility.hotControl = 0;
                            var clickNode = CampaignMapEditorWin.Instance.GetClickNode(e.mousePosition);
                            if(clickNode != null && clickedPort != null)
                            {
                                this.ConnectNode(clickNode);
                            }
                            clickedPort = null;
                            e.Use();
                        }
                        break;
                } 
            }


            //draw the new connection line if in link mode
            if (clickedPort != null )
            {
                Handles.DrawBezier(clickedPort.pos, e.mousePosition, clickedPort.pos, e.mousePosition, new Color(0.5f, 0.5f, 0.8f, 0.8f), null, 2);
            }

            //draw all connected lines
            for (int connectionIndex = 0; connectionIndex < this.NodeConnections.Count; connectionIndex++)
            {

                var connection = NodeConnections[connectionIndex];
                if (connection != null)
                {
                    var sourcePos = new Vector2(((connection.source.NodeRect.width / (NodeConnections.Count + 1)) * (connectionIndex + 1)) + connection.source.NodeRect.xMin, connection.source.NodeRect.yMax + 6);
                    var targetPos = new Vector2(connection.target.NodeRect.center.x, connection.target.NodeRect.y);

                    Rect connectedPortRect = new Rect(0, 0, 12, 12);
                    connectedPortRect.center = sourcePos;
                    GUI.Box(connectedPortRect, "", "nodePortConnected");
                    connection.DrawConnectionGUI(sourcePos, targetPos);

                    //On right click disconnect connection from the source.
                    if (e.button == 1 && e.type == EventType.MouseDown && connectedPortRect.Contains(e.mousePosition))
                    {
                        this.RemoveConnection(connection);
                        e.Use();
                        return;
                    }
                }
            }
        }

        public void RemoveConnection(NodeConnection connection)
        {
            this.NodeConnections.Remove(connection);
        }

        private void CheckPan(Vector2 delta)
        {
            this.NodeRect.x += delta.x;
            this.NodeRect.y += delta.y;
            this.PosX = this.NodeRect.x - CampaignMapEditorWin.Instance.mapArea.x;
            this.PosY = this.NodeRect.y - CampaignMapEditorWin.Instance.mapArea.y;
        }


        public void ConnectNode(CampaignNode node)
        {
            if (this == node) return;
            for (int i = 0; i < this.NodeConnections.Count; i++)
            {
                if( this.NodeConnections[i].target == node)
                {
                    return;
                }
            }
            this.NodeConnections.Add(new NodeConnection { source = this, target = node });
        }


        public void InitResource()
        {
            this.Texture = AssetDatabase.LoadAssetAtPath<Texture>("Assets/GameResource/" + this.TexturePath);
        }

        public bool SerializePrepare()
        {
            for (int i = 0; i < this.NodeConnections.Count; i++)
            {
                if (!this.PreNodeIds.Contains(this.NodeConnections[i].target.ID))
                {
                    this.PreNodeIds.Add(this.NodeConnections[i].target.ID);
                }
            }
            
            TexturePath = AssetDatabase.GetAssetPath(this.Texture);
            if (!TexturePath.StartsWith("Assets/GameResource/"))
            {
                Debug.Log("sprite wrong place");
                return false;
            }
            TexturePath = TexturePath.Replace("Assets/GameResource/", "");
            return true;
        }

    }
}
