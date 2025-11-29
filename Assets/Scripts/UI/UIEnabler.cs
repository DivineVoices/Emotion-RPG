using UnityEngine;
using UnityEngine.EventSystems;

public class UIEnabler : MonoBehaviour
{
    [Header("UI Settings")]
    public GameObject changedElement;
    [Header("Animation")]
    private bool isRevealed;

    private void Start()
    {
        isRevealed = changedElement.activeSelf;
    }

    void Update()
    {
        changedElement.SetActive(isRevealed);
    }
    public void SwapActiveState()
    {
        isRevealed = !isRevealed;
    }

}
