using System;
using Unity.Cinemachine;
using UnityEngine;

public class TargetGroupModifier : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup targetGroup;
    public void AddTarget(GameObject npcTarget)
    {
        if (npcTarget == null) return;
        if (targetGroup.FindMember(npcTarget.transform) != -1) return;
        CinemachineTargetGroup.Target AddedTarget = new();
        AddedTarget.Object = npcTarget.transform;
        targetGroup.AddMember(npcTarget.transform, 1f, 1f);
    }

    public void ClearTargets()
    {
        targetGroup.Targets.Clear();
    }

    public void RemoveTargetAt(int targetIndex)
    {
        if (targetIndex < 0 || targetIndex >= targetGroup.Targets.Count) return;
        targetGroup.RemoveMember(targetGroup.Targets[targetIndex].Object);
    }
}
