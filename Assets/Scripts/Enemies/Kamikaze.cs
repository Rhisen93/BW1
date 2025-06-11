using UnityEngine;


public class KamikazeEnemy : Enemy
{
    [Header("Kamikaze Settings")]
    public float normalSpeed = 2f;
    public float chargeSpeed = 5f;
    public float chargeRange = 2f; // distanza per iniziare a caricare
    public float explosionRadius = 1.0f;
    public float damage = 15f;
    public float explosionDelay = 0.1f;
    private bool isExploding = false;
    public GameObject Explosion;

    protected override void FixedUpdate()
    {
        if (playerTransform == null || isExploding) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        float currentSpeed = distanceToPlayer <= chargeRange ? chargeSpeed : normalSpeed;

        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);

        
    }

    private System.Collections.IEnumerator Explode()
    {

        isExploding = true;
        yield return new WaitForSeconds(explosionDelay);
        
        GameObject fx = Instantiate(Explosion, transform.position, Quaternion.identity);
        Animator anim = fx.GetComponent<Animator>();
        if (anim != null)
        {
            Debug.Log("Animator trovato, setto trigger");
            anim.SetTrigger("ExplosionTrigger");
        }
        else
        {
            Debug.LogError("Animator non trovato su Explosion prefab!");
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance <= explosionRadius)
                {
                    LifeController life = hit.GetComponent<LifeController>();
                    if (life != null)
                    {
                        life.TakeDamage(damage);
                    }
                }
            }
        }


        Destroy(gameObject);
        Destroy(fx, 1f);
    }

    // Se troppo vicino, esplodi
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isExploding)
        {
            StartCoroutine(Explode());
        }
    }


}
