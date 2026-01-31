using UnityEngine;

public class Player_TPS_Camera : MonoBehaviour
{
    public Transform target;          // Le joueur
    public float distance = 3f;       // Distance derrière le joueur
    public float height = 1.5f;       // Hauteur de la caméra
    public float mouseSensitivity = 200f;
    public float rotationSmoothTime = 0.1f;

    private float yaw;                // Rotation horizontale
    private float pitch;              // Rotation verticale
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -30f, 60f); // Limite haut/bas

        // Lissage de la rotation
        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        // Applique la rotation à la caméra
        transform.eulerAngles = currentRotation;

        // Position de la caméra derrière le joueur
        Vector3 offset = transform.forward * -distance + Vector3.up * height;
        transform.position = target.position + offset;

        // Oriente le joueur dans la direction de la caméra (optionnel mais recommandé)
        target.rotation = Quaternion.Euler(0f, yaw, 0f);
    }
}