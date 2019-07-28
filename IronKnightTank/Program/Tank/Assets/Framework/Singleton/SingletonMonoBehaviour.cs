using UnityEngine;
using System.Collections;

public abstract class SingletonMonoBehaviour<T> where T:MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get {
            if(!_instance)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).ToString();
                _instance = go.AddComponent<T>();
            }
            return _instance;
        }
    }
}
