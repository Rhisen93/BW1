using UnityEngine;

public class ChargerEnemy : Enemy
{
    [Header("Charger Settings")]
    public float speed = 3f;
    public float damage = 3f;
    public float damageInterval = 1f; // ogni quanto fa danno (in secondi)

    private float lastDamageTime = 0f;

    protected override void FixedUpdate()
    {
        if (playerTransform == null) return;

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                LifeController life = collision.gameObject.GetComponent<LifeController>();
                if (life != null)
                {
                    life.TakeDamage(damage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}
