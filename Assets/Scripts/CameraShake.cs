using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineCamera cineCam;
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;

    void Awake()
    {
        cineCam = GetComponent<CinemachineCamera>();
        if (cineCam != null)
            noise = cineCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void TriggerShake(float amplitude = 5f, float frequency = 5f, float duration = 1f)
    {
        if (noise == null)
            return;

        StopAllCoroutines();
        StartCoroutine(ShakeRoutine(amplitude, frequency, duration));
    }

    IEnumerator ShakeRoutine(float amp, float freq, float dur)
    {
        noise.AmplitudeGain = amp;
        noise.FrequencyGain = freq;

        yield return new WaitForSeconds(dur);

        noise.AmplitudeGain = 0;
        noise.FrequencyGain = 0;
    }
}
