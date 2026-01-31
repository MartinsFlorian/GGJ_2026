using UnityEngine;

public class FPSRigidbodyController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody rb;
    public Transform cameraTransform;
    public Transform playerVisual;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 5f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    [Header("Dash")]
    public float dashPower = 20f;
    public float dashDecay = 6f;      // <-- baisse progressive
    public float dashControl = 0.4f;  // <-- contrôle pendant dash

    private bool isDashing = false;
    private Vector3 dashVelocity;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleMouseLook();
        HandlePlayerVisualRotation();
        HandleJump();
        HandleDashInput();
    }

    void FixedUpdate()
    {
        HandleMovementPhysics();
        HandleDashPhysics();
    }

    // ------------------------------
    // MOUSE LOOK
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
    // PLAYER VISUAL ROTATION
    // ------------------------------
    void HandlePlayerVisualRotation()
    {
        if (playerVisual != null)
            playerVisual.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }

    // ------------------------------
    // MOVEMENT (Rigidbody)
    // ------------------------------
    void HandleMovementPhysics()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = (transform.right * x + transform.forward * z).normalized;

        Vector3 targetVel = move * moveSpeed;
        Vector3 velChange = targetVel - new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(velChange, ForceMode.VelocityChange);
    }

    // ------------------------------
    // JUMP
    // ------------------------------
    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    // ------------------------------
    // DASH INPUT (clic droit)
    // ------------------------------
    void HandleDashInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 dir = cameraTransform.forward.normalized;

            dashVelocity = dir * dashPower;
            isDashing = true;
        }
    }

    // ------------------------------
    // DASH PHYSICS (fluide, sans stop)
    // ------------------------------
    void HandleDashPhysics()
    {
        if (!isDashing)
            return;

        // Ajoute la vitesse du dash à la vitesse actuelle
        rb.linearVelocity = new Vector3(
            dashVelocity.x,
            rb.linearVelocity.y,     // <-- gravité normale
            dashVelocity.z
        );

        // Décélération progressive
        dashVelocity = Vector3.Lerp(dashVelocity, Vector3.zero, dashDecay * Time.fixedDeltaTime);

        // Contrôle léger pendant dash
        float x = Input.GetAxisRaw("Horizontal") * dashControl;
        float z = Input.GetAxisRaw("Vertical") * dashControl;
        Vector3 control = (transform.right * x + transform.forward * z);
        rb.AddForce(control, ForceMode.Acceleration);

        // Fin du dash
        if (dashVelocity.magnitude < 0.5f)
            isDashing = false;
    }
}
