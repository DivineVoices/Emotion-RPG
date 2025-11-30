using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] float fixedSize = 0.3f;

    [Header("Flip axes (inversion)")]
    [SerializeField] private bool flipX = false;
    [SerializeField] private bool flipY = false;
    [SerializeField] private bool flipZ = false;

    [Header("Rotation personnalis�e (en degr�s)")]
    [SerializeField] float angleX = 0f;
    [SerializeField] float angleY = 0f;
    [SerializeField] float angleZ = 0f;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    public void SetCamera(Camera camera)
    {
        camera = mainCamera;
    }

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            Debug.LogWarning("Main camera not assigned in Billboard.");
        }

        transform.LookAt(
            transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up
        );

        transform.Rotate(angleX, angleY, angleZ, Space.Self);

        // Taille constante
        float distance = Vector3.Distance(mainCamera.transform.position, transform.position);
        float scaleFactor = distance * 0.001f * fixedSize;
        Vector3 baseScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        baseScale.x *= flipX ? -1 : 1;
        baseScale.y *= flipY ? -1 : 1;
        baseScale.z *= flipZ ? -1 : 1;

        transform.localScale = baseScale;
    }
}