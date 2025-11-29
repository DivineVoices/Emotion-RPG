using UnityEngine;

public class DialogueActionHandler : MonoBehaviour
{
    [SerializeField] CameraShake cameraShakeRef;

    public void GiveItem(string itemName)
    {
        // Your give item logic here
        Debug.Log($"Giving item: {itemName}");
    }

    public void ShakeCam()
    {
        Debug.Log("Screen Shaking");
        cameraShakeRef.TriggerShake();
    }

    public void StartQuest(string questId)
    {
        // Your quest starting logic here
        Debug.Log($"Starting quest: {questId}");
    }

    public void ChangeScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}