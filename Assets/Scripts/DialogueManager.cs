using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI textSpot;
    [SerializeField] Transform choiceContainer;
    [SerializeField] Button choiceButtonPrefab;

    [Header("Action Handler")]
    [SerializeField] PlayerMovement playerMovementRef;
    [SerializeField] DialogueActionHandler actionHandler; // Drag your DialogueActionHandler here in inspector

    [Header("Typing Settings")]
    [SerializeField] float textSpeed = 0.03f;

    DialogueBlock currentBlock;
    int currentLine = 0;
    bool isTyping = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(DialogueBlock block)
    {
        currentBlock = block;
        currentLine = 0;

        dialoguePanel.SetActive(true);
        ShowNextLine();
    }

    public void Advance()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            textSpot.text = currentBlock.lines[currentLine];
            isTyping = false;
            return;
        }

        // If more lines remain -> continue
        if (currentLine < currentBlock.lines.Count - 1)
        {
            currentLine++;
            ShowNextLine();
            return;
        }

        // End of lines: show choices if exist
        if (currentBlock.choices.Count > 0)
        {
            ShowChoices();
            return;
        }

        playerMovementRef.isInteracting = false;
        EndDialogue();
    }

    void ShowNextLine()
    {
        ClearChoices();
        StartCoroutine(TypeLine(currentBlock.lines[currentLine]));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        textSpot.text = "";

        foreach (char c in line)
        {
            textSpot.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
    }

    void ShowChoices()
    {
        ClearChoices();

        foreach (var c in currentBlock.choices)
        {
            var btn = Instantiate(choiceButtonPrefab, choiceContainer);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = c.text;

            btn.onClick.AddListener(() =>
            {
                // Execute the action with parameters if specified
                if (!string.IsNullOrEmpty(c.actionName) && actionHandler != null)
                {
                    var method = actionHandler.GetType().GetMethod(c.actionName);
                    if (method != null)
                    {
                        method.Invoke(actionHandler, new object[] { c.parameters });
                    }
                    else
                    {
                        Debug.LogWarning($"Method {c.actionName} not found in DialogueActionHandler");
                    }
                }

                ClearChoices();
                if (c.nextBlock != null)
                    StartDialogue(c.nextBlock);
                else
                    EndDialogue();
            });
        }
    }

    void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        currentBlock = null;
    }
}