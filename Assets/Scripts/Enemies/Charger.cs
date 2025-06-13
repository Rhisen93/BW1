using UnityEngine;

public class ChargerEnemy : Enemy
{
    [Header("Charger Settings")]
    public float speed = 3f;
    public float damage = 3f;
    public float damageInterval = 1f;

    private float lastDamageTime = 0f;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isPlayerDead = false;
    private LifeController playerLife;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (playerTransform != null)
            playerLife = playerTransform.GetComponent<LifeController>();
    }

    protected override void FixedUpdate()
    {
        if (playerTransform == null || isPlayerDead)
        {
            SetIdle();
            return;
        }

        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Flip verso il player
        if (direction.x != 0)
            spriteRenderer.flipX = direction.x > 0;

        // Movimento
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        // Stato movimento (Attack come movimento)
        animator.SetInteger("Atk", 1);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isPlayerDead || !collision.gameObject.CompareTag("Player")) return;

        if (Time.time >= lastDamageTime + damageInterval)
        {
            if (playerLife != null)
            {
                playerLife.TakeDamage(damage);
                lastDamageTime = Time.time;

                animator.SetTrigger("Hit");

                if (playerLife.GetCurrentHealth() <= 0)
                {
                    isPlayerDead = true;
                    SetIdle();
                }
            }
        }
    }

    public override void Die()
    {
        base.Die();
        animator.SetBool("isDead", true);
        this.enabled = false;
    }

    private void SetIdle()
    {
        animator.SetInteger("State", 0); // 0 = Idle
    }
}
