using UnityEngine;

public class IceBullet : AbstractBullet
{
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float effectChance = 0.1f;

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
            if (target.GetComponent<IceEffect>() == null)
            {
                IceEffect effect = target.AddComponent<IceEffect>();
                effect.Initialize(slowMultiplier, slowDuration);
            }
        }
    }


    public override void LevelUp()
    {
        slowDuration += 1f;
        slowMultiplier -= 0.1f;
        effectChance += 0.05f;
        slowMultiplier = Mathf.Clamp(slowMultiplier, 0.1f, 1f);
        effectChance = Mathf.Clamp(effectChance, 0f, 1f);
    }

    public override DamageType GetDamageType() => DamageType.ICE;
}
