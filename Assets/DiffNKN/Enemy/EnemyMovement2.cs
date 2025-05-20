using UnityEngine;

public class EnemyMovement2 : MonoBehaviour
{
    public float speed = 7f;
    private Transform player;
    private bool hasDamaged = false;
    public float impactHeightOffset = 0.2f;

    private void Start()
    {
        player = Camera.main?.transform;
    }

    private void Update()
    {
        if (player == null) return;

        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasDamaged || !other.CompareTag("Player")) return;

        hasDamaged = true;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        PlayerArmor playerArmor = other.GetComponent<PlayerArmor>();

        if (playerHealth != null)
        {
            int damage = Random.Range(6, 11);
            Vector3 hitDir = (playerHealth.transform.position - transform.position).normalized;
            hitDir.y -= impactHeightOffset;

            if (playerArmor != null)
                playerArmor.AbsorbDamage(damage, playerHealth, hitDir);
            else
                playerHealth.TakeDamage(damage, hitDir);

            Debug.Log($"EnemyMovement2 - Daño aplicado: {damage}");
        }

        Destroy(gameObject);
    }
}
