using UnityEngine;

public class DialogueActionHandler : MonoBehaviour
{
    [SerializeField] CameraShake cameraShakeRef;
    [SerializeField] GemModifier gemModifRef;

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

    public void AddGem(string gem)
    {
        GemType gemConverted = GemTypeConverter.FromString(gem);
        gemModifRef.AddGemToInventory(gemConverted);
    }

    public void UpgradeGem(string gem)
    {
        GemType gemConverted = GemTypeConverter.FromString(gem);
        gemModifRef.UpgradeGem(gemConverted);
    }

    public void IsRightGemLevel(string gem)
    {
        GemType gemConverted = GemTypeConverter.FromString(gem);
        Debug.Log(GemChecker.HasEquippedGem(gemConverted));
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