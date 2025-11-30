using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/Dialogue Block")]
public class DialogueBlock : ScriptableObject
{
    [TextArea]
    public List<string> lines = new List<string>();

    [System.Serializable]
    public class Choice
    {
        public string text;               // text shown as the choice
        public DialogueBlock nextBlock;   // what this choice leads to
        public string actionName;         // Method name to call in DialogueActionHandler
        public DialogueActionParams parameters; // Parameters for the method
    }

    public List<Choice> choices = new List<Choice>();
}