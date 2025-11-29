using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*
    private CinemachineCamera cinemachineCamera;
    private CharacterController controller;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Transform armature;

    private Animator animator;

    // -- Movement States --

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float mass = 1f;
    public float jumpHeight = 1.5f;

    private Vector2 moveDirection = Vector2.zero;
    private bool isRunning = false;
    public float verticalVelocity = 0f;

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f;
    public LayerMask groundMask;
    public bool isGrounded = false;

    // -- Look States --
    //[HideInInspector] public CursorController cursorController;

    // Input System
    //[HideInInspector] public InputSystem_Actions inputActions;

    // Interaction States
    [HideInInspector] public PlayerInteractSphere interactSphere;

    // -- Ethereal States --
    private bool isEthereal = false;

    // -- Crow States --
    private bool isCrowControlled = false;

    // -- Capture States --
    private bool isCapturing = false;
    private GameObject capturedObject = null;

    private GameObject lastHovered = null;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        armature = transform.Find("GhostArmature");
        inputActions = new InputSystem_Actions();
        cinemachineCamera = FindObjectOfType<CinemachineCamera>();
        //cursorController = GetComponent<CursorController>();
        interactSphere = GetComponent<PlayerInteractSphere>();
        isEthereal = false;
        isCapturing = false;
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        inputActions.Player.Enable();
    }

    private void OnDestroy()
    {
        inputActions.Disable();

    }

    private void Update()
    {
        UpdateGroundedState();
        HandleMovement();
        HandleLooking();
        HandleEthereal();
        HandleCapture();
        HandleInteract();
        HandleControlsInfo();
        //Debug.Log("Object captured: " + (capturedObject != null ? capturedObject.name : "None") + ", IsCapturing: " + isCapturing
        //            + ", IsCrowControlled: " + isCrowControlled
        //            + ", IsEthereal: " + isEthereal);
    }

    // ---- Movement Handling ----

    private void HandleMovement()
    {

        moveDirection = inputActions.Player.Move.ReadValue<Vector2>();
        Vector3 move = transform.right * moveDirection.x + transform.forward * moveDirection.y;
        move.Normalize();

        isRunning = inputActions.Player.Sprint.IsPressed();
        float currentSpeed = isRunning ? runSpeed : walkSpeed;

        controller.Move(move * currentSpeed * Time.deltaTime);

        CalculateVerticalVelocity();

        controller.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);

        // Rotation towards movement direction
        if (moveDirection != Vector2.zero)
        {
            float rotX = armature.eulerAngles.x;
            Quaternion targetRotation = Quaternion.LookRotation(move);
            targetRotation = Quaternion.Euler(rotX, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            //skinnedMeshRenderer.transform.rotation = Quaternion.Slerp(skinnedMeshRenderer.transform.rotation, targetRotation, Time.deltaTime * 10f);
            armature.rotation = Quaternion.Slerp(armature.rotation, targetRotation, Time.deltaTime * 10f);

        }

    }

    private void CalculateVerticalVelocity()
    {
        if (isGrounded)
        {
            if (inputActions.Player.Jump.WasPressedThisFrame())
            {
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
                AudioManager.Instance.StopSound("Jump");
                AudioManager.Instance.PlaySound("Jump");
            }
            else
            {
                verticalVelocity = -0.5f; // small negative value to keep grounded
            }

        }

        // appliquer gravité
        verticalVelocity += Physics.gravity.y * mass * Time.deltaTime;

    }

    private void UpdateGroundedState()
    {
        // Le CharacterController capsule démarre à controller.center
        Vector3 origin = transform.position + controller.center;
        float radius = controller.radius * 0.9f;
        float distance = groundCheckDistance;

        // SphereCast pour detecter le sol
        isGrounded = Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hit, distance, groundMask);
    }

    // ---- Looking Handling ----
    private void HandleLooking()
    {
    }

    // ---- Ethereal Handling ----
    private void HandleEthereal()
    {
        if (inputActions.Player.Ethereal.WasPressedThisFrame())
        {
            isEthereal = !isEthereal;

            AudioManager.Instance.StopSound("etherIn");
            AudioManager.Instance.StopSound("etherOut");

            if (!isEthereal)
            {
                controller.excludeLayers = LayerMask.GetMask("Nothing");
                AudioManager.Instance.PlaySound("etherOut");
            }
            else
            {
                controller.excludeLayers = LayerMask.GetMask("Ethereal");
                AudioManager.Instance.PlaySound("etherIn");
            }
                
        }
    }

    public bool IsEtheral => isEthereal;

    // ---- Capture Handling ----
    private void HandleCapture()
    {
        if (isEthereal)
            return;

        HandleCrow();

        if(isCrowControlled)
            return;

        GameObject obj = cursorController.GetObjectUnderCursor();
        ChangeOverlayCapture(obj);

        HandleCapturableWithCursor(obj);
    }

    private void HandleCapturableWithCursor(GameObject obj)
    {
        if (inputActions.Player.Capture.WasPressedThisFrame())
        {

            if (obj != null && obj.CompareTag("Capturable") && !isCapturing)
            {
                // Capture
                capturedObject = obj;
                isCapturing = true;
                CapturableController capturable = capturedObject.GetComponent<CapturableController>();
                if (capturable != null)
                {
                    Transform iconTransform = obj.transform.Find("Icon_Position");
                    iconTransform.Find("Icon_Xbox").gameObject.SetActive(false);
                    iconTransform.Find("Icon_Play").gameObject.SetActive(false);
                    iconTransform.Find("Icon_Keyboard").gameObject.SetActive(false);

                    AudioManager.Instance.StopSound("swapCapture");
                    AudioManager.Instance.PlaySound("swapCapture");

                    capturable.SetMainController();
                    inputActions.Disable();
                    interactSphere.enabled = false;
                }

                Debug.Log("Captured " + capturedObject.name);
            }
            else if (capturedObject != null && capturedObject.CompareTag("Capturable") && !isCapturing)
            {
                CapturableController capturable = capturedObject.GetComponent<CapturableController>();
                if (capturable != null)
                {
                    Transform iconTransform = capturedObject.transform.Find("Icon_Position");
                    iconTransform.Find("Icon_Xbox").gameObject.SetActive(false);
                    iconTransform.Find("Icon_Play").gameObject.SetActive(false);
                    iconTransform.Find("Icon_Keyboard").gameObject.SetActive(false);

                    AudioManager.Instance.StopSound("swapCapture");
                    AudioManager.Instance.PlaySound("swapCapture");

                    capturable.SetMainController();
                    inputActions.Disable();
                    interactSphere.enabled = false;
                }
            }
        }
    }

    private void HandleControlsInfo()
    {
        if(inputActions.Player.Info.WasPressedThisFrame())
        {
            SceneToggleLoader.ToggleScene("SceneControls");
        }
    }
    private void HandleCrow()
    {
        if(isCrowControlled)
            return;

        if (inputActions.Player.Crow.WasPressedThisFrame())
        {
            CrowController crowController = FindObjectOfType<CrowController>();
            if (crowController != null)
            {
                AudioManager.Instance.StopSound("swapCapture");
                AudioManager.Instance.PlaySound("swapCapture");

                isCrowControlled = true;
                crowController.LastController = gameObject;
                crowController.SetMainController();
                inputActions.Disable();
                interactSphere.enabled = false;
            }
        }
    }

    private void ChangeOverlayCapture(GameObject obj)
    {
        if (obj != null && obj.CompareTag("Capturable") && !isCapturing)
        {
            Transform iconTransform = obj.transform.Find("Icon_Position");

            if (iconTransform == null)
                return;

            if (Gamepad.current != null)
            {

                bool IsXbox(Gamepad gp)
                {
                    return gp.displayName.Contains("Xbox");
                }

                if (IsXbox(Gamepad.current))
                {
                    iconTransform.Find("Icon_Xbox").gameObject.SetActive(true);
                    iconTransform.Find("Icon_Play").gameObject.SetActive(false);
                    iconTransform.Find("Icon_Keyboard").gameObject.SetActive(false);

                }
                else
                {
                    iconTransform.Find("Icon_Xbox").gameObject.SetActive(false);
                    iconTransform.Find("Icon_Play").gameObject.SetActive(true);
                    iconTransform.Find("Icon_Keyboard").gameObject.SetActive(false);
                }

            }
            else
            {
                iconTransform.Find("Icon_Xbox").gameObject.SetActive(false);
                iconTransform.Find("Icon_Play").gameObject.SetActive(false);
                iconTransform.Find("Icon_Keyboard").gameObject.SetActive(true);
            }

            lastHovered = obj;
        }
        else
        {
            lastHovered?.transform.Find("Icon_Position").Find("Icon_Xbox").gameObject.SetActive(false);
            lastHovered?.transform.Find("Icon_Position").Find("Icon_Play").gameObject.SetActive(false);
            lastHovered?.transform.Find("Icon_Position").Find("Icon_Keyboard").gameObject.SetActive(false);
        }
    }

    public bool IsCapturing
    {
        get { return isCapturing; }
        set { isCapturing = value; }
    }

    public bool IsCrowControlled
    {
        get { return isCrowControlled; }
        set { isCrowControlled = value; }
    }

    // ---- Interaction Handling ----
    private void HandleInteract()
    {
        if(inputActions.Player.Interact.WasPressedThisFrame())
        {
            if (NPC.activeNPC != null)
            {
                AudioManager.Instance.StopSound("interact");
                AudioManager.Instance.PlaySound("interact");
                NPC.activeNPC.AdvanceDialogue();
                return;
            } 

            interactSphere.InteractInRange();
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (controller == null)
            controller = GetComponent<CharacterController>();
        // Le CharacterController capsule démarre à controller.center
        Vector3 origin = transform.position + controller.center;
        float radius = controller.radius * 0.9f;
        float distance = groundCheckDistance;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(origin + Vector3.down * distance, radius);
    }


    public void FreezeDialogueControls()
    {
        inputActions.Disable();
        inputActions.Player.Interact.Enable();
    }

    public void UnfreezeControls()
    {
        inputActions.Player.Enable();
    }
    */
}
