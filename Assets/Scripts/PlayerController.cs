using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 _move;
    [SerializeField] private Camera _camera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] PlayerInteractSphere _interactSphere;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("State Booleans")]
    //private bool isWalking = false;
    //private bool isInCombat = false;
    //private bool isInteracting = false;



    [Header("Input Action References")]
    [SerializeField] InputActionReference _MoveInput;
    [SerializeField] InputActionReference _InteractInput;

    private void Start()
    {
        _MoveInput.action.Enable();
        _MoveInput.action.performed += OnMove;
        _MoveInput.action.canceled += OnMove;

        //_InteractInput.action.Enable();
        //_InteractInput.action.performed += OnInteract;
        //_InteractInput.action.canceled += OnInteract;

        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        // Movement
        _MoveInput.action.performed -= OnMove;
        _MoveInput.action.canceled -= OnMove;
    }

    void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }
    //void OnInteract(InputAction.CallbackContext context)
    //{
    //    if (!isInteracting)
    //    {
    //        isInteracting = true;
    //        _interactSphere.InteractInRange();
    //    }
    //    else
    //    {

    //    }
    //}

    public void Update()
    {
        Move(_move);
    }

    //private void HandleAnimation()
    //{
    //    _animator.SetBool("Walking", _move != Vector2.zero);
    //    _animator.SetBool("Grounded", grounded);
    //    _animator.SetBool("Attacking", isAttacking);
    //    _animator.SetBool("AttackingStrong", isAttackingStrong);
    //    _animator.SetBool("Charging", isCharging);
    //    _animator.SetBool("Flying", isFlying);
    //}

    private void Move(Vector2 input)
    {
        if (input.sqrMagnitude < 0.01f)
            return;

        float scaledSpeed = moveSpeed * Time.deltaTime;

        Vector3 moveDir;

        if (_camera != null)
        {
            Vector3 camForward = _camera.transform.forward;
            Vector3 camRight = _camera.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            // THIS is the correct, rotation-proof mapping
            moveDir = camForward * input.y + camRight * input.x;
        }
        else
        {
            moveDir = new Vector3(input.x, 0, input.y);
        }

        transform.Translate(moveDir * scaledSpeed, Space.World);

        if (moveDir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(moveDir);
    }
    public void FreezeMovement(bool freeze)
    {
        if (freeze)
        {
            // Disable the input action to prevent movement
            _MoveInput.action.Disable();
        }
        else
        {
            // Re-enable the input action to allow movement again
            _MoveInput.action.Enable();
        }
    }

}