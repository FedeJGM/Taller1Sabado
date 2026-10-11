using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class EagleEnemy : MonoBehaviour
{
    [Header("Flight")]
    [SerializeField] private float speed = 2.2f;
    [SerializeField] private float bobAmplitude = 0.35f;
    [SerializeField] private float bobFrequency = 1.5f;
    [SerializeField] private float despawnDistance = 28f;
    [SerializeField] private float wallCheckDistance = 0.8f;

    private SpriteRenderer sr;
    private float initialY;
    private float phase;
    private float dirX;
    private Vector3 spawnPos;

    private void Start()
    {
        spawnPos = transform.position;
        initialY = transform.position.y;
        phase = Random.value * Mathf.PI * 2f;
        dirX = Random.value < 0.5f ? -1f : 1f;
        sr = GetComponent<SpriteRenderer>();
        sr.flipX = dirX > 0f;
    }

    private void Update()
    {
        int obstacleMask = ~(1 << gameObject.layer);
        if (Physics2D.Raycast(transform.position, new Vector2(dirX, 0f), wallCheckDistance, obstacleMask))
        {
            dirX = -dirX;
            sr.flipX = dirX > 0f;
        }

        Vector3 pos = transform.position;
        pos.x += dirX * speed * Time.deltaTime;
        pos.y = initialY + Mathf.Sin((Time.time + phase) * bobFrequency) * bobAmplitude;
        transform.position = pos;

        if (Vector3.Distance(spawnPos, transform.position) > despawnDistance)
            Destroy(gameObject);
    }
}