using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class OpenSceneMenuItem  {

	[MenuItem("Open Scene/Battle")]
	static void OpenBattle(){
		OpenScene ("Battle");
	}

    [MenuItem("Open Scene/Welcome")]
    static void OpenWelcome()
    {
        OpenScene("Welcome");
    }

    static void OpenScene (string sceneName)
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo () == true) {
			EditorSceneManager.OpenScene ("Assets/GameResource/Scenes/" + sceneName + ".unity");
		}
	}
}
