using UnityEngine;

public class BaseBullet : AbstractBullet
{
    public void Awake()
    {
        SetDamageType(DamageType.BASE);        
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
        
    }

    public override void LevelUp()
    {
        SetDamage(Damage + 2);      
        SetSpeed(Speed + 1f);       
    }
}
