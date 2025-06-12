using UnityEngine;

public class PoisonBullet : AbstractBullet
{
    [SerializeField] private float poisonDuration = 3f;
    [SerializeField] private int poisonDamagePerSecond = 1;
    [SerializeField] private float effectChance = 0.1f;

    private void Awake()
    {
        SetDamageType(DamageType.POISON);        
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        transform.Translate(GetDirection() * Speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            LifeController life = other.GetComponent<LifeController>();
            if (life != null)
            {
                life.TakeDamage(Damage);
                ApplyEffect(other.gameObject);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }
    }

    public override void ApplyEffect(GameObject target)
    {
        if (Random.value <= effectChance)
        {
            if (target.GetComponent<PoisonEffect>() == null)
            {
                PoisonEffect effect = target.AddComponent<PoisonEffect>();
                effect.Initialize(poisonDamagePerSecond, poisonDuration);
            }
        }
    }


    public override void LevelUp()
    {
        poisonDuration += 1f;
        poisonDamagePerSecond += 1;
        effectChance += 0.05f;
        effectChance = Mathf.Clamp(effectChance, 0f, 1f);
    }
}

