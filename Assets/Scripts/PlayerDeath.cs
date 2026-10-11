using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private string enemyTag = "Enemy";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
            Die();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemyTag))
            Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
