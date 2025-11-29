using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] DialogueBlock npcDialogue;

    public void Interact()
    {
        DialogueManager.Instance.StartDialogue(npcDialogue);
    }
}
