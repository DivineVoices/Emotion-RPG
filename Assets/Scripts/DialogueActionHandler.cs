using UnityEngine;

[System.Serializable]
public class DialogueActionParams
{
    public string stringParam;
    public int intParam;
    public float floatParam;
    public bool boolParam;

    // Constructor for easier initialization
    public DialogueActionParams(string stringValue = "", int intValue = 0, float floatValue = 0f, bool boolValue = false)
    {
        stringParam = stringValue;
        intParam = intValue;
        floatParam = floatValue;
        boolParam = boolValue;
    }
}

public class DialogueActionHandler : MonoBehaviour
{
    [SerializeField] CameraShake cameraShakeRef;
    [SerializeField] GemModifier gemModifRef;
    [SerializeField] SceneSwitcher sceneSwitcherRef;
    [SerializeField] KarmaDisplayer karmaDisplayerRef;

    public void GiveItem(DialogueActionParams parameters)
    {
        string itemName = parameters.stringParam;
        int quantity = parameters.intParam > 0 ? parameters.intParam : 1;
        Debug.Log($"Giving {quantity} of {itemName}");
    }

    public void ShakeCam(DialogueActionParams parameters)
    {
        Debug.Log("Screen Shaking");
        cameraShakeRef.TriggerShake();
    }

    public void KarmaDisplay(DialogueActionParams parameters)
    {
        karmaDisplayerRef.DisplayKarma();
    }

    public void KarmaClear(DialogueActionParams parameters)
    {
        karmaDisplayerRef.ClearKarmaDisplay();
    }

    public void AddGem(DialogueActionParams parameters)
    {
        GemType gemConverted = GemTypeConverter.FromString(parameters.stringParam);
        gemModifRef.AddGemToInventory(gemConverted);
    }

    public void UpgradeGem(DialogueActionParams parameters)
    {
        GemType gemConverted = GemTypeConverter.FromString(parameters.stringParam);
        gemModifRef.UpgradeGem(gemConverted);
    }

    public void IsRightGemLevel(DialogueActionParams parameters)
    {
        GemType gemConverted = GemTypeConverter.FromString(parameters.stringParam);
        Debug.Log(GemChecker.HasEquippedGem(gemConverted));
    }

    public void EquipGem(DialogueActionParams parameters)
    {
        GemType gemConverted = GemTypeConverter.FromString(parameters.stringParam);
        gemModifRef.ChangeGem(gemConverted, parameters.intParam);
    }

    public void EnterFight(DialogueActionParams parameters)
    {
        sceneSwitcherRef.SwitchScene("CombatScene");
    }

    public void StartQuest(DialogueActionParams parameters)
    {
        string questId = parameters.stringParam;
        Debug.Log($"Starting quest: {questId}");
    }

    public void ChangeScene(DialogueActionParams parameters)
    {
        string sceneName = parameters.stringParam;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void PunchSFX(DialogueActionParams parameters)
    {
        AudioManager.Instance.PlaySound("Punch");
    }
}