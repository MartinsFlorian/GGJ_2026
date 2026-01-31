using UnityEngine;

public class S_Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private string environmentTag = "Environment";

    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSO_ProjectileSpeed projectileSpeed;
    private float speed;

    private void OnEnable()
    {
        speed = projectileSpeed.Value;
    }
    private void OnDisable()
    {
        projectileSpeed.Value = speed;
    }
    private void Update()
    {
        transform.Translate(Vector3.forward * projectileSpeed.Value * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(playerTag))
        {
            // Handle collision with player (e.g., apply damage)
            Destroy(gameObject);
        }
        else if(other.CompareTag(environmentTag))
        {
            // Handle collision with environment (e.g., destroy projectile)
            Destroy(gameObject);
        }
    }
}