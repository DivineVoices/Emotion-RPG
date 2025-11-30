using UnityEngine;

public class BillBoard : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;
    void LateUpdate()
    {
        transform.LookAt(cameraObject.transform);
        transform.Rotate(0f, 180f, 0f);
    }
}
