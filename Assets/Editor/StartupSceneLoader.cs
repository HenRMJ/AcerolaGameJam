using System;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartupSceneLoader
{
    private static int previousScene;

    static StartupSceneLoader()
    {
        EditorApplication.playModeStateChanged += EditorApplication_PlayModeStateChanged;
    }

    private static void EditorApplication_PlayModeStateChanged(PlayModeStateChange change)
    {
        switch (change)
        {
            case PlayModeStateChange.ExitingEditMode:
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                break;
            case PlayModeStateChange.EnteredPlayMode:
                previousScene = EditorSceneManager.GetActiveScene().buildIndex;

                if (EditorSceneManager.GetActiveScene().buildIndex != 0)
                {
                    EditorSceneManager.LoadScene(0);
                }
                break;
            case PlayModeStateChange.ExitingPlayMode when EditorSceneManager.GetActiveScene().buildIndex != previousScene:
                EditorSceneManager.LoadScene(previousScene);
                break;
        }
    }
}
