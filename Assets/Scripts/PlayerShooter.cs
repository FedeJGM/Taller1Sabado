using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Fire Settings")]
    [SerializeField] private float fireCooldown = 0.35f;
    [SerializeField] private float bulletSpeed = 14f;

    private SpriteRenderer spriteRenderer;
    private float nextFireTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (firePoint == null || bulletPrefab == null) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        bool fire = keyboard.fKey.wasPressedThisFrame || keyboard.leftCtrlKey.wasPressedThisFrame;
        if (fire && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;
            Fire();
        }
    }

    private void Fire()
    {
    bool left = spriteRenderer.flipX;
    Vector2 dir = left ? Vector2.left : Vector2.right;

    Vector3 pos = firePoint.position;
    if (left) pos.x = transform.position.x - (firePoint.position.x - transform.position.x);

    var bullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
    bullet.GetComponent<Bullet>().Launch(dir * bulletSpeed);
    }
}