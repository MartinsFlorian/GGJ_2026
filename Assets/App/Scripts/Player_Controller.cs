using UnityEngine;

public class FPSPlayerController : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("Dash")]
    public float dashPower = 20f;
    public float dashFriction = 8f;
    public float dashGravityMultiplier = 0.3f; // gravité réduite pendant dash
    public KeyCode dashKey = KeyCode.LeftShift;

    private Vector3 velocity;
    private Vector3 dashVelocity;
    private bool isDashing = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (controller == null)
            controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
        HandleDash();
    }

    // ------------------------------
    // MOUSE LOOK (FPS)
    // ------------------------------
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    // ------------------------------
    // MOVEMENT + GRAVITY
    // ------------------------------
    void HandleMovement()
    {
        if (isDashing)
            return; // pas de déplacement normal pendant le dash

        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * moveSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // ------------------------------
    // DASH
    // ------------------------------
    void HandleDash()
    {
        if (Input.GetKeyDown(dashKey) && !isDashing)
        {
            // direction EXACTE de la caméra (FPS)
            Vector3 dir = cameraTransform.forward.normalized;

            dashVelocity = dir * dashPower;
            isDashing = true;

            // on neutralise la gravité normale pendant le dash
            velocity.y = 0f;
        }

        if (isDashing)
        {
            float dashGravity = gravity * dashGravityMultiplier;
            dashVelocity.y += dashGravity * Time.deltaTime;

            controller.Move(dashVelocity * Time.deltaTime);

            dashVelocity = Vector3.Lerp(dashVelocity, Vector3.zero, dashFriction * Time.deltaTime);

            if (dashVelocity.magnitude < 0.2f)
            {
                isDashing = false;

                if (!controller.isGrounded)
                    velocity.y = dashVelocity.y;
                else
                    velocity.y = -2f;
            }
        }
    }
}