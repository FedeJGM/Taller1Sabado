using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;
    private float alive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        alive = 0f;
    }

    public void Launch(Vector2 velocity)
    {
        rb.linearVelocity = velocity;
    }

    private void Update()
    {
        alive += Time.deltaTime;
        if (alive >= lifetime) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("Enemy"))
            Destroy(other.gameObject);

        Destroy(gameObject);
    }
}