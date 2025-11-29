using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteractSphere : MonoBehaviour
{
    //Unfinished while waiting for Crow;
    [SerializeField] float detectionRange = 50f;
    [SerializeField] CamSwitcher camSwitcherRef;
    [SerializeField] TargetGroupModifier targetGroupRef;

    public void InteractInRange()
    {
        Vector3 origin = transform.position;
        LayerMask layers = LayerMask.GetMask("Interactable");

        Collider[] colliders = Physics.OverlapSphere(origin, detectionRange, layers);

        if (colliders.Length == 0)
        {
            camSwitcherRef.SwitchToCam(0, false);
            targetGroupRef.ClearTargets();
            return;
        }

        Collider closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Collider col in colliders)
        {
            float dist = Vector3.Distance(origin, col.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = col;
            }
        }

        if (closest != null)
        {
            GameObject closestGameObject = closest.gameObject;
            IInteractable interactable = null;
            switch (closest.tag)
            {
                case "NPC":
                    interactable = closestGameObject.GetComponent<IInteractable>();
                    interactable?.Interact();
                    break;
                case "Button":
                    interactable = closestGameObject.GetComponent<IInteractable>();
                    interactable?.Interact();
                    break;
                case "Item":
                    break;
                case "Untagged":
                    break;
            }

            AudioManager.Instance.StopSound("interact");
            AudioManager.Instance.PlaySound("interact");
        }
    }
}
