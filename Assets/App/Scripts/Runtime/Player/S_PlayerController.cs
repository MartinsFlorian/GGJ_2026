using UnityEngine;
using Unity.Cinemachine;

public class S_PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SSO_Player player;

    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private CinemachineCamera vCam;

    [Header("Inputs")]
    [SerializeField] private RSE_OnPlayerMove rse_OnPlayerMove;
    [SerializeField] private RSE_OnPlayerJump rse_OnPlayerJump;
    [SerializeField] private RSE_OnPlayerDash rse_OnPlayerDash;
    [SerializeField] private RSE_OnPlayerLook rse_OnPlayerLook;

    //[Header("Outputs")]

    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool isGrounded;
    private bool canDash = true;
    private bool resetFOVPending = false;

    private float yaw;
    private float pitch;
    private float coyoteTimer;
    private float jumpBufferTimer;
    private float bobTimer;

    private Vector3 cameraRootInitialPos;

    private void Awake()
    {
        cameraRootInitialPos = cameraRoot.localPosition;
        yaw = transform.eulerAngles.y;
    }

    private void OnEnable()
    {
        rse_OnPlayerMove.action += SetMove;
        rse_OnPlayerLook.action += SetLook;
        rse_OnPlayerJump.action += BufferJump;
        rse_OnPlayerDash.action += Dash;
    }
    private void OnDisable()
    {
        rse_OnPlayerMove.action -= SetMove;
        rse_OnPlayerLook.action -= SetLook;
        rse_OnPlayerJump.action -= BufferJump;
        rse_OnPlayerDash.action -= Dash;
    }

    private void Update()
    {
        RotateCamera();
        HandleHeadbob();
        HandleFOV();
    }
    private void FixedUpdate()
    {
        CheckGround();
        Move();
        RotatePlayer();
        HandleJump();
    }

    private void SetMove(Vector2 input)
    {
        moveInput = input;
    }
    private void SetLook(Vector2 input)
    {
        lookInput = input;
    }
    private void Move()
    {
        Vector3 camForward = Vector3.Scale(cameraRoot.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = cameraRoot.right;

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;

        Vector3 targetVelocity = moveDir * player.Value.moveSpeed;
        Vector3 velocity = rb.linearVelocity;

        Vector3 velocityChange = targetVelocity - new Vector3(velocity.x, 0, velocity.z);
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }
    private void RotatePlayer()
    {
        if(moveInput.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.Euler(0f, yaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, player.Value.rotationSpeed * Time.fixedDeltaTime);
    }
    private void RotateCamera()
    {
        yaw += lookInput.x * player.Value.lookSensitivity;
        pitch -= lookInput.y * player.Value.lookSensitivity;
        pitch = Mathf.Clamp(pitch, player.Value.minPitch, player.Value.maxPitch);

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        cameraTarget.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
    private void BufferJump()
    {
        jumpBufferTimer = player.Value.jumpBufferTime;
    }
    private void HandleJump()
    {
        jumpBufferTimer -= Time.fixedDeltaTime;

        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * player.Value.jumpForce, ForceMode.Impulse);

            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }   
    }
    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, player.Value.groundCheckDistance, player.Value.groundLayer);

        if (isGrounded)
        {
            coyoteTimer = player.Value.coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.fixedDeltaTime;
        }
    }
    private void Dash()
    {
        if(!canDash) return;

        canDash = false;
        resetFOVPending = false;

        Vector3 dashDir = Vector3.Scale(cameraRoot.forward, new Vector3(1, 0, 1)).normalized;
        rb.AddForce(dashDir * player.Value.dashForce, ForceMode.Impulse);


        Invoke(nameof(ResetFOV), player.Value.fovHoldTime);

        Invoke(nameof(ResetDash), player.Value.dashCooldown);
    }
    private void ResetDash()
    {
        canDash = true;
    }
    private void ResetFOV()
    {
        resetFOVPending = true;
    }
    private void HandleFOV()
    {
        float targetFOV = resetFOVPending ? player.Value.normalFOV : player.Value.dashFOV;
        vCam.Lens.FieldOfView = Mathf.Lerp(vCam.Lens.FieldOfView, targetFOV, Time.deltaTime * player.Value.fovTransitionSpeed);
    }
    private void HandleHeadbob()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if(!isGrounded || horizontalVel.magnitude < 0.1f)
        {
            bobTimer = 0f;
            cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, cameraRootInitialPos, Time.fixedDeltaTime * player.Value.bobSmooth);
            return;
        }

        bobTimer += Time.fixedDeltaTime * player.Value.bobFrequency;
        float bobOffsetY = Mathf.Sin(bobTimer) * player.Value.bobAmplitude;

        Vector3 targetPos = cameraRootInitialPos + Vector3.up * bobOffsetY;

        cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, targetPos, Time.fixedDeltaTime * player.Value.bobSmooth);
    }
}