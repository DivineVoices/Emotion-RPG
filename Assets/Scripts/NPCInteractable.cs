//using UnityEngine;
//using TMPro;
//using System.Collections;

//public class NPCInteractable : MonoBehaviour, IInteractable
//{
//    [SerializeField] private TextMeshProUGUI Dialogue;
//    [SerializeField] private string[] normalMessages;
//    [SerializeField] private string crystalMessage;
//    [SerializeField] private GameObject dialoguePanel;
//    [SerializeField] private GameObject Canvas;
//    [SerializeField] GameObject CanvasInteract;
//    [SerializeField] private float dialogueSpeed = 0.05f;

//    private int messageIndex = 0;
//    private Coroutine typingCoroutine;
//    private bool isTyping = false;
//    private bool hasSeenCrystalMessage = false;
//    private bool hasCrystal = false;

//    void Start()
//    {
//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.OnCrystalCollected += OnCrystalCollected;
//            hasCrystal = GameManager.Instance.IsRedCrystalCollected() ||
//                         GameManager.Instance.IsYellowCrystalCollected();
//        }
//    }

//    void OnDestroy()
//    {
//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.OnCrystalCollected -= OnCrystalCollected;
//        }
//    }

//    private void OnCrystalCollected(bool isRedCrystal)
//    {
//        hasCrystal = true;
//    }

//    public void Interact()
//    {
//        if (hasCrystal && !hasSeenCrystalMessage)
//        {
//            ShowCrystalMessage();
//            return;
//        }

//        if (hasSeenCrystalMessage)
//        {
//            CloseDialogue();
//            return;
//        }

//        Messages normaux
//        if (messageIndex >= normalMessages.Length)
//        {
//            CloseDialogue();
//            return;
//        }

//        Canvas.SetActive(true);
//        dialoguePanel.SetActive(true);

//        if (isTyping)
//        {
//            StopCoroutine(typingCoroutine);
//            Dialogue.text = normalMessages[messageIndex];
//            isTyping = false;
//            return;
//        }

//        typingCoroutine = StartCoroutine(ShowDialogue(normalMessages[messageIndex]));
//        messageIndex++;
//    }

//    private void ShowCrystalMessage()
//    {
//        Canvas.SetActive(true);
//        dialoguePanel.SetActive(true);
//        hasSeenCrystalMessage = true;

//        if (isTyping)
//        {
//            StopCoroutine(typingCoroutine);
//            Dialogue.text = crystalMessage;
//            isTyping = false;
//            return;
//        }

//        typingCoroutine = StartCoroutine(ShowDialogue(crystalMessage));
//    }

//    IEnumerator ShowDialogue(string text)
//    {
//        isTyping = true;
//        Dialogue.text = "";
//        foreach (char letter in text)
//        {
//            Dialogue.text += letter;
//            yield return new WaitForSeconds(dialogueSpeed);
//        }
//        isTyping = false;
//    }

//    private void CloseDialogue()
//    {
//        dialoguePanel.SetActive(false);
//        Canvas.SetActive(false);
//    }

//    public void StopInteract() { }

//    public void SetTargetGroup(TargetGroupModifier modifier)
//    {
//        modifier.AddTarget(this.gameObject, 0.8f);
//        CanvasInteract.SetActive(true);
//    }

//    public void RemoveTargetGroup(TargetGroupModifier modifier)
//    {
//        modifier.RemoveTarget(this.gameObject);
//        CanvasInteract.SetActive(false);
//    }

//    public Transform GetTransform()
//    {
//        return transform;
//    }
//}