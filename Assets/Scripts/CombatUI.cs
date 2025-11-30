using UnityEngine;
using UnityEngine.UI;
using TMPro;  // If you're using TextMeshPro
using System.Collections.Generic;

public class AttackSystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform gridParent;

    [Header("Runtime Data")]
    [SerializeField] private List<Button> attackButtons = new List<Button>();

    private int totalLevel = 0;
    private int gemCount = 0;

    void Start()
    {
        LoadGemData();
        GenerateAttackButtons();
        AssignButtonActions();
    }

    private void LoadGemData()
    {
        gemCount = GemChecker.CountEquippedGems();

        // Calculate total level based on equipped gems
        if (GemInventory.firstGemType != GemType.None)
            totalLevel += GemInventory.GetGemLevelIndex(GemInventory.firstGemType);

        if (GemInventory.secondGemType != GemType.None)
            totalLevel += GemInventory.GetGemLevelIndex(GemInventory.secondGemType);

        if (GemInventory.thirdGemType != GemType.None)
            totalLevel += GemInventory.GetGemLevelIndex(GemInventory.thirdGemType);

        Debug.Log("Total Attack Level: " + totalLevel);
    }

    private void GenerateAttackButtons()
    {
        for (int i = 0; i < gemCount; i++)
        {
            Button button = Instantiate(buttonPrefab.GetComponent<Button>(), gridParent);
            attackButtons.Add(button);

            // Set the button text based on the gem type
            SetButtonText(button, i);
        }
    }

    private void AssignButtonActions()
    {
        // Assign button actions dynamically based on equipped gems
        for (int i = 0; i < attackButtons.Count; i++)
        {
            int index = i; // Capture index for later use
            attackButtons[i].onClick.AddListener(() =>
            {
                // Get the gem type and level based on index
                GemType gemType = GetGemTypeForIndex(index);
                GemLevel gemLevel = GemInventory.GetGemLevel(gemType);

                if (gemType != GemType.None)
                {
                    // Perform the attack using the gem type and level
                    Attack(gemType, gemLevel, GameObject.Find("Emotion").GetComponent<EmotionGaugeManager>());
                }
                else
                {
                    Debug.LogWarning("No gem equipped for this slot!");
                }
            });
        }
    }

    public void Attack(GemType type, GemLevel level, EmotionGaugeManager gaugeManager)
    {
        int emotionGain = CalculateEmotionGain(type, level);

        gaugeManager.EmotionGauge += emotionGain;
        Debug.Log($"ATTACK used: {type} (Level {level}) | +{emotionGain} gauge");
    }

    private int CalculateEmotionGain(GemType type, GemLevel level)
    {
        int baseGain = 2;

        // Increase gauge based on gem level
        switch (level)
        {
            case GemLevel.Level1: baseGain = 2; break;
            case GemLevel.Level2: baseGain = 5; break;
            case GemLevel.Level3: baseGain = 10; break;
        }
        return baseGain;
    }

    private void SetButtonText(Button button, int index)
    {
        // Get the Text component (for TextMeshPro, use TMP_Text instead)
        Text buttonText = button.GetComponentInChildren<Text>();  // Assuming Text component is inside button

        if (buttonText != null)
        {
            // Get the gem type for the current index (first, second, or third gem)
            GemType gemType = GetGemTypeForIndex(index);

            // Set the button text to the GemType (e.g. "Topaz")
            buttonText.text = gemType.ToString();  // Displays "Topaz", "Amethyst", or "Ruby"
        }
        else
        {
            Debug.LogWarning("Button text component not found on the button!");
        }
    }

    private GemType GetGemTypeForIndex(int index)
    {
        // Return the corresponding GemType based on the index
        switch (index)
        {
            case 0: return GemInventory.firstGemType;
            case 1: return GemInventory.secondGemType;
            case 2: return GemInventory.thirdGemType;
            default: return GemType.None;
        }
    }
}
