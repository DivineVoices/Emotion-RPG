using UnityEngine;

public class DialogueEventRelay : MonoBehaviour
{
    [SerializeField] CameraShake followCamShake;

    public void TriggerCamShake()
    {
        followCamShake.TriggerShake();
    }
}