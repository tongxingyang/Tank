using UnityEngine;
using System.Collections;

public class TestMgr : MonoBehaviour
{
    public static TestMgr Instance;
    // Use this for initialization
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Test()
    {
        Debug.Log("Test");
    }


    public static void Test2()
    {
        Debug.Log("Test2");
    }
}
