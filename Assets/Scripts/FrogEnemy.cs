using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class FrogEnemy : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private float moveSpeed = 1.6f;
    [SerializeField] private float hopForce = 5.5f;
    [SerializeField] private float hopInterval = 2.5f;
    [SerializeField] private float hopIntervalVariance = 1f;

    [Header("Ground & Edge Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform frontCheck;
    [SerializeField] private float checkRadius = 0.1f;
    [SerializeField] private float edgeRayLength = 0.5f;
    [SerializeField] private float edgeProbeRadius = 0.15f;
    [SerializeField] private float stepTolerance = 0.3f;
    [SerializeField] private LayerMask groundLayer = ~0;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private bool isGrounded;
    private float nextHopTime;

    private static readonly int HopHash = Animator.StringToHash("Hop");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        nextHopTime = Time.time + Random.Range(0.5f, 1.5f);
        ApplyFacing();
    }

    private void FixedUpdate()
    {
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        else
            isGrounded = rb.linearVelocity.y <= 0.05f;

        if (isGrounded && Time.time >= nextHopTime)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, hopForce);
            animator.SetTrigger(HopHash);
            nextHopTime = Time.time + Random.Range(hopInterval - hopIntervalVariance, hopInterval + hopIntervalVariance);
        }

        float dir = spriteRenderer.flipX ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        if (isGrounded && frontCheck != null && !Physics2D.CircleCast(frontCheck.position, edgeProbeRadius, Vector2.down, edgeRayLength, groundLayer))
            Flip();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == gameObject) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, col);
            return;
        }
        if (collision.contactCount == 0) return;

        ContactPoint2D contact = collision.GetContact(0);
        bool sideHit = Mathf.Abs(contact.normal.x) > 0.5f;
        bool aboveFeet = contact.point.y > transform.position.y + stepTolerance;

        if (sideHit && aboveFeet)
            Flip();
    }

    private void Flip()
    {
        spriteRenderer.flipX = !spriteRenderer.flipX;
        ApplyFacing();
    }

    private void ApplyFacing()
    {
        if (frontCheck == null) return;
        Vector3 p = frontCheck.localPosition;
        p.x = Mathf.Abs(p.x) * (spriteRenderer.flipX ? 1f : -1f);
        frontCheck.localPosition = p;
    }
}