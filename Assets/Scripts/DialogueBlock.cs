using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public string actionName;         // method name to call
        public string actionParameter;    // parameter for the method
    }

    public List<Choice> choices = new List<Choice>();
}