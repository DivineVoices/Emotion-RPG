using UnityEngine;

[CreateAssetMenu(menuName = "Attack System/Attack Data")]
public class AttackData : ScriptableObject
{
    public GemType type;
    public GemLevel level;
    public int emotionGain;
}
