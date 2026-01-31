using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;

    private bool isGrounded;

    private KeyCode horizontalPriority = KeyCode.None;
    private KeyCode verticalPriority = KeyCode.None;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Gestion des priorités
        UpdatePriority(ref horizontalPriority, KeyCode.Q, KeyCode.D);
        UpdatePriority(ref verticalPriority, KeyCode.S, KeyCode.Z);

        // Valeurs finales
        float x = PriorityToAxis(horizontalPriority, KeyCode.Q, KeyCode.D);
        float z = PriorityToAxis(verticalPriority, KeyCode.S, KeyCode.Z);

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void UpdatePriority(ref KeyCode priority, KeyCode negative, KeyCode positive)
    {
        // Si aucune priorité, on prend la première touche pressée
        if (priority == KeyCode.None)
        {
            if (Input.GetKey(negative)) priority = negative;
            else if (Input.GetKey(positive)) priority = positive;
        }

        // Si la touche prioritaire est relâchée → on libère
        if (priority != KeyCode.None && !Input.GetKey(priority))
            priority = KeyCode.None;
    }

    float PriorityToAxis(KeyCode priority, KeyCode negative, KeyCode positive)
    {
        if (priority == negative) return -1f;
        if (priority == positive) return 1f;
        return 0f;
    }
}