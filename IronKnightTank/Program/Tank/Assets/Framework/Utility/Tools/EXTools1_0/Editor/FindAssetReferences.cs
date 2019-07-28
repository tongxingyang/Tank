using UnityEditor;
using UnityEngine;
using System.Collections;

public class FindAssetReferences:UnityEditor.Editor
{
    //[MenuItem("Assets/FindReferences/in project")]
    private static void FindInProject()
    {
        foreach (var findAsset in AssetDatabase.FindAssets("ref:_Game_/Prefabs/MajiangPrefab.prefab"))
        {
            Debug.Log(findAsset);
        }

        
    }

    //[MenuItem("Assets/FindReferences/in scenes")]
    private static void OnSearchForReferences()
    {
        //确保鼠标右键选择的是一个Prefab
        if (Selection.gameObjects.Length != 1)
        {
            return;
        }

        //遍历所有游戏场景
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                //打开场景
                EditorApplication.OpenScene(scene.path);
                //获取场景中的所有游戏对象
                GameObject[] gos = (GameObject[])FindObjectsOfType(typeof(GameObject));
                foreach (GameObject go in gos)
                {
                    //判断GameObject是否为一个Prefab的引用
                    if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
                    {
                        UnityEngine.Object parentObject = EditorUtility.GetPrefabParent(go);
                        string path = AssetDatabase.GetAssetPath(parentObject);
                        //判断GameObject的Prefab是否和右键选择的Prefab是同一路径。
                        if (path == AssetDatabase.GetAssetPath(Selection.activeGameObject))
                        {
                            //输出场景名，以及Prefab引用的路径
                            Debug.Log(scene.path + "  " + GetGameObjectPath(go));
                        }
                    }
                }
            }
        }
    }
    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }
}
