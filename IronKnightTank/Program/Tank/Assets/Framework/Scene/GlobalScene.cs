using UnityEngine;
using UnityEngine.SceneManagement;

namespace XQFramework.Scene
{
    using Scene = UnityEngine.SceneManagement.Scene;

    public class GlobalScene
    {
        public static Scene Global
        {
            get
            {
                if (!global.isLoaded)
                {
                    global = SceneManager.GetSceneByName("GlobalScene");
                    if (!global.isLoaded)
                    {
                        global = SceneManager.CreateScene("GlobalScene");
                    }
                }
                return global;
            }
        }
        private static Scene global;

        public static void Add(GameObject o)
        {
            SceneManager.MoveGameObjectToScene(o, Global);
        }

        public static GameObject CreateRoot(string name,Vector3 position)
        {
            var gameObject = new GameObject(name);
            gameObject.transform.position = position;
            SceneManager.MoveGameObjectToScene(gameObject, Global);
            return gameObject;
        }
    }
}