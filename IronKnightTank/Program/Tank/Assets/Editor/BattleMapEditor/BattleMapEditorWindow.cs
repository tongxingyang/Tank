// ----------------------------------------------------------------------------
// <copyright file="MapEditorWindow.cs" company="上海序曲网络科技有限公司">
// Copyright (C) 2015 上海序曲网络科技有限公司
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited 
// without the prior written consent of the copyright owner.
// </copyright>
// <author>zhaowenpeng</author>
// <date>09/05/2018</date>
// ----------------------------------------------------------------------------
namespace Assets.Editor.BattleMapEditor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using System;
    using Assets.Tools.Script.Editor.Tool;
    using ParadoxNotion.Serialization;
    using Assets.Tools.Script.Editor.Inspector.Field;
    using System.IO;

    public class BattleMapEditorWindow : EditorWindow
    {
        //地图绘制数据
        private Vector4 mapAreaPos = new Vector4(0, 30, 1000, 800);
        private Vector4 mapAreaSize = new Vector4(0, 0, 1009, 800);

        private Vector4 terrianAreaPos = new Vector4(1050, 500, 300,300);
        private Vector4 terrianAreaSize = new Vector4(1000, 500, 600, 300);
        private Vector2 mapScrollView;
        private Vector2 terrianScrollView;
        ///当前选中的格子
        private GridPos selectPos = new GridPos(99 , 99);

        //当前选中Terrian
        private int currTerrian;

        //当前选中的格子
        private BlockData selectData;

        //当前选择的视图层
        private MapLayerType curMapType = MapLayerType.地块;

        //保存lua文件路径
        private string saveLuaFilePath = "";

        //地形數據
        Dictionary<int, TerrianElementData> terrianDic = new Dictionary<int, TerrianElementData>();

        //地形资源
        Dictionary<int, Material> terrianElementMaterialDic = new Dictionary<int, Material>();

        private BattleMap battleMap;

        private bool isSelectGrid = false;

        private bool isLeftMouseDown = false;
        private bool isRightMouseDown = false;

        private Texture none_texture;

        public void Init(BattleMap map, string luaPath)
        {
            saveLuaFilePath = luaPath;
            battleMap = map;
            LoadTerrianDataAndRes();
            none_texture = Resources.Load<Sprite>("None_Block").texture;
        }

        private void LoadTerrianDataAndRes()
        {
            string json = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/GameResource/Data/TerrainEleement.json").text;
            List<TerrianElementData> dataList = JSON.Deserialize<List<TerrianElementData>>(json);
            
            for (int i = 0; i < dataList.Count; i++)
            {
                TerrianElementData data = dataList[i];
                string paht = "Assets/GameResource/Material/Block/" + data.Resource_Name;
                terrianElementMaterialDic[data.TerrainEleement_ID] = AssetDatabase.LoadAssetAtPath<Material>(paht);
                if (terrianElementMaterialDic[data.TerrainEleement_ID] == null)
                {
                    Debug.LogError("path wrong" + paht);
                }
                terrianDic[data.TerrainEleement_ID] = data;
            }
        }

        public void OnGUI()
        {
            GUI.skin.label.richText = true;
            GUI.skin.button.richText = true;
            GUI.skin.box.richText = true;
            GUI.skin.textArea.richText = true;
            GUI.skin.textField.richText = true;
            GUI.skin.toggle.richText = true;
            GUI.skin.window.richText = true;
            if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {
                isLeftMouseDown = true;
            }
            if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
            {
                isLeftMouseDown = false;
            }
            if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                isRightMouseDown = true;
            }
            if (Event.current.type == EventType.MouseUp && Event.current.button == 1)
            {
                isRightMouseDown = false;
            }
            GUILayout.BeginHorizontal();
            {
                //左边
                GUILayout.BeginVertical();
                {
                    //顶部菜单
                    GUILayout.BeginHorizontal(GUILayout.Width(this.mapAreaSize.z), GUILayout.Height(this.mapAreaPos.y));
                    this.DrawMapLayerLayout();
                    GUILayout.EndHorizontal();

                    //地图
                    GUILayout.BeginHorizontal();
                    GUILayout.BeginArea(new Rect(this.mapAreaPos.x, this.mapAreaPos.y, this.mapAreaSize.z, this.mapAreaSize.w), new GUIStyle("WindowBackground"));
                    this.DrawMapArea();
                    GUILayout.EndArea();
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                //右边
                GUILayout.BeginVertical(GUILayout.Width(10));
                {
                    if (isSelectGrid)
                    {
                        this.DrawBlockInspector();
                    }
                    else
                    {
                        this.DrawMapInspector();
                    }
                    if (GUILayout.Button("创建地图"))
                    {
                        CreateMap();
                        // Debug.Log("创建地图");
                    }
                    GUILayout.BeginArea(new Rect(this.terrianAreaPos.x, terrianAreaPos.y, terrianAreaSize.z, terrianAreaSize.w), new GUIStyle("WindowBackground"));
                    switch (curMapType)
                    {
                        case MapLayerType.地块:
                            DrawChoseTerrianArea();
                            break;
                        default:
                            break;
                    }
                    GUILayout.EndArea();
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
            if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.Escape)
            {
                this.isSelectGrid = false;
                Event.current.Use();
            }
            //保存 Ctrl + S
            else if (Event.current.type == EventType.KeyUp && Event.current.keyCode == KeyCode.S && Event.current.control)
            {
                var serializeLua = this.battleMap.SerializeLua(); //BattleMap.SerializeLua(this.battleMap);
                File.WriteAllText(this.saveLuaFilePath, serializeLua);
                this.ShowNotification(new GUIContent("保存成功"));
                Event.current.Use();
            }
        }
        
        /// <summary>
        /// 地图内容
        /// </summary>
        private void DrawMapArea()
        {
            this.mapScrollView = GUI.BeginScrollView(new Rect(0, 0, this.mapAreaPos.z, this.mapAreaPos.w), this.mapScrollView, new Rect(this.mapAreaSize.x, this.mapAreaSize.y, 2000, 2000) , true , true);
            Color backgroundColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.clear;
            for (int i = 0; i < battleMap.Width; i++)
            {
                for (int j = battleMap.Height - 1; j >= 0 ; j--)
                {
                    var blockData = battleMap.GetBlock(new GridPos(i, j));
                    if (blockData == null)
                    {
                        continue;
                    }
                    switch (this.curMapType)
                    {
                        case MapLayerType.地块:
                            this.DrawTerrian(blockData);
                            break;
                        case MapLayerType.出生点:
                            this.DrawBorn(blockData);
                            break;
                        case MapLayerType.NPC:
                            this.DrawNpc(blockData);
                            break;
                        default:
                            break;
                    }
                }
            }

            GUI.backgroundColor = backgroundColor;
            GUI.EndScrollView();
        }

        private void DrawTerrian( BlockData data)
        {
            GridPos pos = data.pos;
            Rect r = new Rect(pos.x * 43, (battleMap.Height - pos.y )* 43, 53, 53);
            if (GUI.Button(new Rect(r), GetTerrianTexture(data.TerrianId)))
            {
                if (Event.current.button == 1)
                {
                    ClearBlockTerrian(data);
                }
                else
                {
                    SetBlocTerrian(data);
                    isSelectGrid = true;
                    this.selectPos = pos;
                }
            }
            if(isLeftMouseDown && r.Contains(Event.current.mousePosition)  )
            {
                SetBlocTerrian(data);
            }
            if (isRightMouseDown && r.Contains(Event.current.mousePosition) )
            {
                ClearBlockTerrian(data);
            }
        }

        private void SetBlocTerrian(BlockData data)
        {
            if (currTerrian != 0) //0 代表默认值 没有设置地块
            {
                data.TerrianId = currTerrian;
            }
        }

        private void ClearBlockTerrian(BlockData data)
        {
            data.TerrianId = 0;
        }

        private void DrawBorn(BlockData data)
        {
            GridPos pos = data.pos;
            Color c = GUI.color;
            if (data.IsBornBlock)
            {
                GUI.color = Color.red;
            }
            if (GUI.Button(new Rect(pos.x * 43, (battleMap.Height - pos.y) * 43, 53, 53), GetTerrianTexture(data.TerrianId)))
            {
                if (Event.current.button == 1)
                {
                    data.IsBornBlock = false;
                }
                else
                {
                    data.IsBornBlock = true;
                    isSelectGrid = true;
                    this.selectPos = pos;
                }
            }
            GUI.color = c;
        }

        public void DrawNpc( BlockData data)
        {
            GridPos pos = data.pos;
            if (GUI.Button(new Rect(pos.x * 43, (battleMap.Height - pos.y) * 43, 53, 53), GetTerrianTexture(data.TerrianId)))
            {
                if (Event.current.button == 1)
                {
                    data.NPCId = 0;
                    isSelectGrid = true;
                    this.selectPos = pos;
                }
                else
                {
                    isSelectGrid = true;
                    this.selectPos = pos;
                }
            }
            if (data.NPCId != 0)
            {
                GUI.Label(new Rect(pos.x * 43 + 5, (battleMap.Height - pos.y) * 43 + 5, 53, 53), data.NPCId.ToString().SetSize(15));
                
            }
        }

        /// <summary>
        /// inspector
        /// </summary>
        private void DrawBlockInspector()
        {
            GUILayout.BeginVertical(new GUIStyle("WindowBackground"), GUILayout.Height(10), GUILayout.Width(500));
            GUILayout.Label("地块属性".SetSize(24));
            GUITool.Line(3);
            BlockData data = battleMap.GetBlockData(selectPos);
            if (data != null)
            {
                data.TerrianId = int.Parse( EditorGUILayout.TextField("地形ID", data.TerrianId.ToString()));
                data.IsBornBlock =  EditorGUILayout.Toggle("是否为出生点", data.IsBornBlock );
                data.NPCId = int.Parse( EditorGUILayout.TextField("NPC_ID", data.NPCId.ToString()));
                if(data.NPCId != 0)
                {
                    data.Toward = (FaceToward)EditorGUILayout.EnumPopup("NPC朝向" , data.Toward);
                }
                EditorGUILayout.LabelField("坐标" + data.pos.x + "," + data.pos.y);
            }
            GUILayout.BeginArea(new Rect(this.terrianAreaPos.x, terrianAreaPos.y, terrianAreaSize.z, terrianAreaSize.w), new GUIStyle("WindowBackground"));
            
            GUILayout.EndArea();
        }

        /// <summary>
        /// mapinspector
        /// </summary>
        private void DrawMapInspector()
        {
            GUILayout.BeginVertical(new GUIStyle("WindowBackground"), GUILayout.Height(10), GUILayout.Width(500));
            GUILayout.Label("地图属性".SetSize(24));
            GUITool.Line(3);
            this.battleMap.Name = EditorGUILayout.TextField(new GUIContent("地图名字"), this.battleMap.Name);
            EditorGUI.BeginChangeCheck();
             this.battleMap.Width =int.Parse( EditorGUILayout.TextField(new GUIContent("地图宽度"), this.battleMap.Width.ToString()));
            if (EditorGUI.EndChangeCheck())
            {
                this.battleMap.FillMap();
            }
            EditorGUI.BeginChangeCheck();
            this.battleMap.Height = int.Parse(EditorGUILayout.TextField(new GUIContent("地图高度"), this.battleMap.Height.ToString()));
            if (EditorGUI.EndChangeCheck())
            {
                this.battleMap.FillMap();
            }
            
            this.battleMap.GridHeight = int.Parse(EditorGUILayout.TextField(new GUIContent("格子高度"), this.battleMap.GridHeight.ToString()));
            this.battleMap.GridWidth = int.Parse(EditorGUILayout.TextField(new GUIContent("格子宽度"), this.battleMap.GridWidth.ToString()));
            this.battleMap.Scripts = FieldInspectorTool.GenericField(this.battleMap, "Scripts") as List<string>;
            this.battleMap.Toward = (FaceToward)EditorGUILayout.EnumPopup("地图默认朝向", battleMap.Toward);
            GUILayout.EndVertical();
        }

        private void DrawChoseTerrianArea()
        {
            int n = 0;
            int column = 6;
            Color c = GUI.backgroundColor;
            GUI.Label(new Rect(0 , 0, 100 , 20) , "TerrianType");
            this.terrianScrollView = GUI.BeginScrollView(new Rect(0, 20, 600, 280), this.terrianScrollView, new Rect(0, 0, 600, 1000) , false , true);
            var idList = terrianElementMaterialDic.Keys;

            foreach (var pairs in terrianElementMaterialDic)
            {
                if (this.currTerrian == pairs.Key)
                {
                    GUI.color = Color.red;    
                }
                if (GUI.Button(new Rect(n % column * 80, n / column * 80, 50, 50), pairs.Value.mainTexture ))
                {
                    GUI.FocusControl("");
                    if (Event.current.button == 1 && this.currTerrian == pairs.Key)
                    {
                        this.currTerrian = 0;
                    }
                    else
                    {
                        this.currTerrian = pairs.Key;
                    }
                }
                GUI.color = c;
                n++;
            }
            GUI.backgroundColor = c;
            GUI.EndScrollView();
        }

        private Texture GetTerrianTexture(int id)
        {
            if (terrianElementMaterialDic.ContainsKey(id))
            {
                return terrianElementMaterialDic[id].mainTexture;
            }
            return none_texture;
        }

        private Material GetTerrianMaterial(int i)
        {
            if (terrianElementMaterialDic.ContainsKey(i))
            {
            return terrianElementMaterialDic[i];

            }
            else
            {
                Debug.Log("no key" + i.ToString());
                return null;
            }
        }

        /// <summary>
        /// 地图导航栏
        /// </summary>
        private void DrawMapLayerLayout()
        {
            var values = Enum.GetValues(typeof(MapLayerType));
            int index = 0;
            foreach (var value in values)
            {
                index++;
                var layer = (MapLayerType)value;
                if (GUITool.Button(string.Format("{0}  Alt+{1}", layer, index), this.curMapType == layer ? Color.red : Color.gray, GUILayout.Width(200), GUILayout.Height(30)))
                {
                    this.curMapType = layer;
                }
            }
        }

        private enum MapLayerType
        {
            地块,
            出生点,
            NPC,
        }

        private void CreateMap()
        {
            if (battleMap == null)
            {
                return;
            }
            

            float gridHeight = battleMap.GridHeight;
            float gridWidth = battleMap.GridWidth;
            float centerX = (battleMap.Width - 1) / 2;
            float centerY = (battleMap.Height - 1) / 2;
            GameObject prefab = Resources.Load<GameObject>("EditorBlock"); 
            GameObject map = GameObject.Find("Map");
            if (map)
            {
                DestroyImmediate(map);
            }
            map = new GameObject("Map");
            foreach (var pairs in battleMap.BlockDic)
            {
                GridPos pos = pairs.Key;
                BlockData block = pairs.Value;
                Vector3 worldPos = new Vector3((pos.x - centerX) * gridWidth, 0, (pos.y - centerY) * gridHeight);
                GameObject blockView = GameObject.Instantiate<GameObject>(prefab);
                blockView.transform.position = worldPos;
                blockView.transform.parent = map.transform;
                Material mat = GetTerrianMaterial(block.TerrianId);
                blockView.name = pos.x + "," + pos.y;
                SpriteRenderer spriteRenderer = blockView.transform.Find("block").GetComponent<SpriteRenderer>();
                
                spriteRenderer.sprite = GetSprite(mat);
            }
        }


        private static Sprite  GetSprite(Material mat)
        {
            var tex = mat.mainTexture;
            string path = AssetDatabase.GetAssetPath(tex);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite == null)
            {
                Debug.Log("texture is not sprite type "+ path);
            }

            return sprite;
        }


        
    }
}