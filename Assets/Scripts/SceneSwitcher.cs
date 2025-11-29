using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    //[SerializeField] Progress levelBeaten;

    public void SwitchScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneSwitcher: No scene name provided!");
            return;
        }

        // Optional: check if the scene exists in build settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            //GameProgress.progressionLevel = levelBeaten;
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"SceneSwitcher: Scene \"{sceneName}\" not found in build settings!");
        }
    }

    public void SwitchSceneAdditive(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("SceneSwitcher: No scene name provided!");
            return;
        }

        // Optional: check if the scene exists in build settings
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogError($"SceneSwitcher: Scene \"{sceneName}\" not found in build settings!");
        }
    }

    // Optional: Reload current scene
    public void ReloadScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // Optional: Quit game (useful for menu buttons)
    public void QuitGame()
    {
        Debug.Log("SceneSwitcher: Quitting game...");
        Application.Quit();
    }
}
