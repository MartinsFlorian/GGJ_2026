using UnityEngine;
using UnityEngine.UI;

public class Player_DashAndAim : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;
    public Transform dashPivot;       // Pivot pour direction 3D du dash
    public Camera cam;
    public Renderer playerRenderer;
    public Image aimOverlay;

    [Header("Camera Settings")]
    public Vector3 normalCamPosition = new Vector3(0f, 1.5f, -3f);
    public Vector3 aimCamPosition = new Vector3(0.8f, 1.6f, -2.5f);
    public Vector3 dashCamOffset = new Vector3(0f, 0f, -2f);
    public float camSmooth = 10f;

    [Header("Zoom Effect")]
    public float dashFOV = 90f;
    public float normalFOV = 70f;
    public float fovSmooth = 10f;

    [Header("Player Transparency")]
    [Range(0f, 1f)] public float aimingOpacity = 0.4f;

    [Header("Slow Motion")]
    public float slowMotionScale = 0.25f;

    [Header("Dash Physics")]
    public float dashPower = 20f;
    public float dashFriction = 8f;

    private bool isAiming = false;
    private bool isDashing = false;

    private Vector3 dashVelocity = Vector3.zero;

    void Update()
    {
        HandleAiming();
        UpdateDashPivotRotation();   // <-- pivot suit la caméra en X et le player en Y
        HandleDash();
        UpdateCameraPosition();
        UpdateCameraFOV();
        UpdatePlayerOpacity();
        UpdateOverlay();
    }

    // ------------------------------
    //  AIMING (clic droit)
    // ------------------------------
    void HandleAiming()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isAiming = true;

            Time.timeScale = slowMotionScale;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isAiming = false;

            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    // ------------------------------
    //  DASH (impulsion + friction)
    // ------------------------------
    void HandleDash()
    {
        if (isAiming && Input.GetMouseButtonDown(0))
        {
            // Direction 3D complète via le pivot
            Vector3 dir = dashPivot.forward.normalized;

            // Impulsion
            dashVelocity = dir * dashPower;
            isDashing = true;

            // Stop slow motion
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }

        if (isDashing)
        {
            controller.Move(dashVelocity * Time.deltaTime);

            dashVelocity = Vector3.Lerp(dashVelocity, Vector3.zero, dashFriction * Time.deltaTime);

            if (dashVelocity.magnitude < 0.1f)
            {
                dashVelocity = Vector3.zero;
                isDashing = false;
            }
        }
    }

    // ------------------------------
    //  CAMERA POSITION
    // ------------------------------
    void UpdateCameraPosition()
    {
        Vector3 targetPos;

        if (isDashing)
            targetPos = aimCamPosition + dashCamOffset;
        else if (isAiming)
            targetPos = aimCamPosition;
        else
            targetPos = normalCamPosition;

        cameraTransform.localPosition =
            Vector3.Lerp(cameraTransform.localPosition, targetPos, Time.deltaTime * camSmooth);
    }

    // ------------------------------
    //  CAMERA ZOOM (FOV)
    // ------------------------------
    void UpdateCameraFOV()
    {
        float targetFOV = isDashing ? dashFOV : normalFOV;

        cam.fieldOfView =
            Mathf.Lerp(cam.fieldOfView, targetFOV, Time.deltaTime * fovSmooth);
    }

    // ------------------------------
    //  PLAYER OPACITY
    // ------------------------------
    void UpdatePlayerOpacity()
    {
        if (playerRenderer == null) return;

        Color c = playerRenderer.material.color;

        float targetAlpha = isAiming ? aimingOpacity : 1f;

        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * 10f);

        playerRenderer.material.color = c;
    }

    // ------------------------------
    //  GREY OVERLAY
    // ------------------------------
    void UpdateOverlay()
    {
        if (aimOverlay == null) return;

        float targetAlpha = isAiming ? 0.4f : 0f;

        Color c = aimOverlay.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * 10f);
        aimOverlay.color = c;
    }

    // ------------------------------
    //  DASH PIVOT FOLLOWS CAMERA X + PLAYER Y
    // ------------------------------
    void UpdateDashPivotRotation()
    {
        float camX = cameraTransform.eulerAngles.x;

        if (camX > 180f)
            camX -= 360f;

        camX = Mathf.Clamp(camX, -45f, 45f);

        float playerY = transform.eulerAngles.y;

        dashPivot.rotation = Quaternion.Euler(camX, playerY, 0f);
    }
}