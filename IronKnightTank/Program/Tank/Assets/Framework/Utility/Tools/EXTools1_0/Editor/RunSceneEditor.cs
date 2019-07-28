using System;
using UnityEditor;
using UnityEngine;

public class RunSceneEditor : EditorWindow
{
    public bool Running=false;
    public string CurrScene;

    private bool EditorApplicationRun=false;

    private bool _errorOpen;
    [MenuItem("Unity Test Tools/Run start #&R")]
    static void RunStart()
    {
        var runSceneEditor = EditorWindow.GetWindow(typeof(RunSceneEditor)) as RunSceneEditor;
        if (runSceneEditor.Running)
        {
            runSceneEditor.RunStartEnd();
            return;
        }
        if (Application.isPlaying)
        {
            runSceneEditor._errorOpen = true;
            runSceneEditor.Close();
            return;
        }
        EditorApplication.SaveScene(EditorApplication.currentScene);
        runSceneEditor.CurrScene = EditorApplication.currentScene;
        EditorApplication.OpenScene("Assets/scence/start.unity");
        EditorApplication.isPlaying = true;
        runSceneEditor.Running = true;

    }
    void RunStartEnd()
    {
        try
        {
            EditorApplication.isPlaying = false;
            OnEnd();
        }
        catch (Exception)
        {
            
        }
        
    }

    void OnEnd()
    {
        Running = false;
        EditorApplication.OpenScene(CurrScene);
        Close();
    }

    void OnDestroy()
    {
        if (_errorOpen) return;
        RunStartEnd();
    }

    public void OnGUI()
    {
        if (GUILayout.Button("End"))
        {
            RunStartEnd();
        }
    }

}
