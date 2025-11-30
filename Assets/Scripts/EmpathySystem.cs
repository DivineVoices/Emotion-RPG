using UnityEngine;
using UnityEngine.UI;
using TMPro;  // If you're using TextMeshPro
using System.Collections.Generic;

public class EmpathySystem : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform gridParent;

    [Header("Runtime Data")]
    [SerializeField] private List<Button> dialogueButtons = new List<Button>();
    [SerializeField] private Image emotionGauge;
    [SerializeField] private SceneSwitcher sceneSwitcher;

    private int totalLevel = 0;
    private int gemCount = 0;

    void Start()
    {
        LoadGemData();
        GenerateDialogueButtons();
        AssignButtonActions();
    }

    private void Update()
    {
        if (emotionGauge.fillAmount == 0)
        {
            sceneSwitcher.SwitchScene("Exploration Debug");
        }
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

        Debug.Log("Total Empathy Level: " + totalLevel);
    }

    private void GenerateDialogueButtons()
    {
        dialogueButtons.Clear();  // Clear any existing buttons

        // Generate dialogue buttons based on equipped gems
        GenerateGemDialogueButtons(GemInventory.firstGemType);
        GenerateGemDialogueButtons(GemInventory.secondGemType);
        GenerateGemDialogueButtons(GemInventory.thirdGemType);
    }

    private void GenerateGemDialogueButtons(GemType gemType)
    {
        if (gemType != GemType.None)
        {
            int gemLevel = GemInventory.GetGemLevelIndex(gemType);
            for (int level = 1; level <= gemLevel; level++)
            {
                // Instantiate a button for each level of the gem
                Button button = Instantiate(buttonPrefab.GetComponent<Button>(), gridParent);
                dialogueButtons.Add(button);

                // Set the button text to show the gem emotion and level
                SetButtonText(button, gemType, level);
            }
        }
    }

    private void AssignButtonActions()
    {
        // Assign button actions dynamically based on equipped gems
        for (int i = 0; i < dialogueButtons.Count; i++)
        {
            int index = i; // Capture index for later use
            dialogueButtons[i].onClick.AddListener(() =>
            {
                // Get the gem type and level based on index
                GemType gemType = GetGemTypeForIndex(index);
                GemLevel gemLevel = GemInventory.GetGemLevel(gemType);

                if (gemType != GemType.None)
                {
                    // Perform the empathy action using the gem type and level
                    RespondWithEmpathy(gemType, gemLevel, GameObject.Find("Emotion").GetComponent<EmotionGaugeManager>());
                }
                else
                {
                    Debug.LogWarning("No gem equipped for this slot!");
                }
            });
        }
    }

    private void RespondWithEmpathy(GemType type, GemLevel level, EmotionGaugeManager gaugeManager)
    {
        // Calculate the empathy effect (e.g., increase/decrease gauge based on gem emotion)
        int emotionEffect = CalculateEmotionEffect(type, level);

        // Update the emotion gauge based on the effect of the empathy response
        gaugeManager.EmotionGauge += emotionEffect;

        // Ensure the EmotionGauge stays within the 0 to 100 range
        gaugeManager.EmotionGauge = Mathf.Clamp(gaugeManager.EmotionGauge, 0, 100);

        // Update the fillAmount of the Emotion Gauge Image, making sure it's in the [0, 1] range
        emotionGauge.fillAmount = gaugeManager.EmotionGauge / 100f;  // Ensure division is by 100f (float division)
    }

    private int CalculateEmotionEffect(GemType type, GemLevel level)
    {
        int baseEffect = 2;

        // Calculate the effect based on gem emotion (fear, joy, anger)
        switch (type)
        {
            case GemType.Amethyst:  // Fear-based response
                baseEffect = (int)level == 1 ? -5 : (int)level == 2 ? -10 : -15;
                break;
            case GemType.Topaz:    // Joy-based response
                baseEffect = (int)level == 1 ? 5 : (int)level == 2 ? 10 : 15;
                break;
            case GemType.Ruby:     // Anger-based response
                baseEffect = (int)level == 1 ? -10 : (int)level == 2 ? -15 : -20;
                break;
        }
        return baseEffect;
    }

    private void SetButtonText(Button button, GemType gemType, int level)
    {
        // Get the Text component (for TextMeshPro, use TMP_Text instead)
        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();  // For TextMeshPro

        if (buttonText != null)
        {
            // Set the button text to the emotion-based sentence based on the gem type and level
            buttonText.text = GetDialogueForGem(gemType, level);  // Shows corresponding dialogue
            Debug.Log($"{gemType} Level {level}: {buttonText.text}");
        }
        else
        {
            Debug.LogWarning("Button text component not found on the button!");
        }
    }

    private string GetDialogueForGem(GemType gemType, int level)
    {
        // Return dialogue based on gem emotion (fear, joy, anger) and level
        switch (gemType)
        {
            case GemType.Amethyst:
                return level == 1 ? "I feel a bit nervous..." :
                       level == 2 ? "I'm so scared... I can barely think!" :
                                    "I'm so fucking scared, you should be too!";
            case GemType.Topaz:
                return level == 1 ? "I’m feeling really happy today!" :
                       level == 2 ? "I can’t stop smiling, everything is perfect!" :
                                    "I feel so full of joy, everything's amazing!";
            case GemType.Ruby:
                return level == 1 ? "I’m angry and frustrated!" :
                       level == 2 ? "I’m ready to explode with rage!" :
                                    "I can’t control my anger, I want to smash something!";
            default:
                return "No dialogue available!";
        }
    }

    private GemType GetGemTypeForIndex(int index)
    {
        // Return the corresponding GemType based on the index
        int firstGemLevel = GemInventory.GetGemLevelIndex(GemInventory.firstGemType);
        int secondGemLevel = GemInventory.GetGemLevelIndex(GemInventory.secondGemType);
        int thirdGemLevel = GemInventory.GetGemLevelIndex(GemInventory.thirdGemType);

        // Depending on the index, return the corresponding gem type
        if (index < firstGemLevel)
        {
            return GemInventory.firstGemType;
        }
        else if (index < firstGemLevel + secondGemLevel)
        {
            return GemInventory.secondGemType;
        }
        else if (index < firstGemLevel + secondGemLevel + thirdGemLevel)
        {
            return GemInventory.thirdGemType;
        }

        return GemType.None;
    }
}
