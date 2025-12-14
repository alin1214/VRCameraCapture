using UnityEngine;
using UnityEngine.InputSystem;

public class VRLocomotion : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform head;
    [SerializeField] private CharacterController characterController;

    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference turnAction;
    [SerializeField] private InputActionReference jumpAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 1.75f;

    [Header("Turning")]
    [SerializeField] private float snapTurnDegrees = 45f;
    [SerializeField] private float snapTurnCooldown = 0.25f;

    [Header("Gravity / Jump")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpSpeed = 3.0f;
    [SerializeField] private bool enableJump = false;

    private float verticalVelocity;
    private float lastTurnTime = -999f;

    void Reset()
    {
        characterController = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        if (moveAction != null) moveAction.action.Enable();
        if (turnAction != null) turnAction.action.Enable();
        if (jumpAction != null) jumpAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null) moveAction.action.Disable();
        if (turnAction != null) turnAction.action.Disable();
        if (jumpAction != null) jumpAction.action.Disable();
    }

    void Update()
    {
        if (head == null || characterController == null) return;

        MatchCapsuleToHead();

        Vector2 move = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
        Vector2 turn = turnAction != null ? turnAction.action.ReadValue<Vector2>() : Vector2.zero;

        Vector3 forward = new Vector3(head.forward.x, 0f, head.forward.z).normalized;
        Vector3 right = new Vector3(head.right.x, 0f, head.right.z).normalized;
        Vector3 horizontal = (forward * move.y + right * move.x) * moveSpeed;

        ApplyGravityAndJump();

        Vector3 motion = horizontal + Vector3.up * verticalVelocity;
        characterController.Move(motion * Time.deltaTime);

        HandleSnapTurn(turn.x);
    }

    private void ApplyGravityAndJump()
    {
        bool grounded = characterController.isGrounded;

        if (grounded && verticalVelocity < 0f)
            verticalVelocity = -0.5f;

        if (enableJump && grounded && jumpAction != null && jumpAction.action.WasPressedThisFrame())
            verticalVelocity = jumpSpeed;

        verticalVelocity += gravity * Time.deltaTime;
    }

    private void HandleSnapTurn(float x)
    {
        if (Mathf.Abs(x) < 0.8f) return;
        if (Time.time - lastTurnTime < snapTurnCooldown) return;

        float dir = Mathf.Sign(x);
        transform.Rotate(0f, dir * snapTurnDegrees, 0f);
        lastTurnTime = Time.time;
    }

    private void MatchCapsuleToHead()
    {
        Vector3 headLocal = transform.InverseTransformPoint(head.position);

        characterController.center = new Vector3(headLocal.x, characterController.height * 0.5f, headLocal.z);

        float targetHeight = Mathf.Clamp(headLocal.y, 1.0f, 2.0f);
        characterController.height = targetHeight;

        characterController.center = new Vector3(headLocal.x, targetHeight * 0.5f, headLocal.z);
    }
}
