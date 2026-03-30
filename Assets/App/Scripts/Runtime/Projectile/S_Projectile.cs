using UnityEngine;

public class S_Projectile : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float lifeTime = 5f;
    //[Header("References")]

    //[Header("Inputs")]

    [Header("Outputs")]
    [SerializeField] private RSO_ProjectileSpeed projectileSpeed;
    [SerializeField] private RSE_OnPlayerTakeDamage onPlayerTakeDamage;
    private float speed;

    private void OnEnable()
    {
        speed = projectileSpeed.Value;
        if (lifeTime > 0f)
            Destroy(gameObject, lifeTime);
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
            onPlayerTakeDamage.Call();
            Destroy(gameObject);
        }
    }
}