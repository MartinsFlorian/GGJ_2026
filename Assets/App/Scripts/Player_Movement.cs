using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public float speed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Vérifie si le joueur touche le sol
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        // Récupère les inputs ZQSD (Q = gauche, D = droite, Z = avant, S = arrière)
        float x = Input.GetAxisRaw("Horizontal");   // Q / D
        float z = Input.GetAxisRaw("Vertical");     // Z / S

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Saut
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravité
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
