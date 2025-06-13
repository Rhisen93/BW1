using UnityEngine;

public class BaseEnemyBullet : AbstractEnemyBullet
{
    private Vector2 direction;

    private void Awake()
    {
        SetDamageType(DamageType.BASE);
    }

    private void Update()
    {
        Move();
    }
    public override void Move()
    {
        transform.Translate(_direction * GetSpeed() * Time.deltaTime, Space.World);
    }

    public override void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;
        Debug.Log("Direction set: " + _direction);
        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LifeController life = other.GetComponent<LifeController>();
            if (life != null)
            {
                life.TakeDamage(GetDamage());
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
        // eventuali effetti
    }

    public override void LevelUp()
    {
        SetDamage(GetDamage() + 2);
        SetSpeed(GetSpeed() + 1f);
    }

}

    
