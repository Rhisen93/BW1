using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    protected float Speed { get; set; }
    protected int Damage { get; set; }
    protected float LifeTime { get; set; }
    protected DamageType DamageType { get; private set; }

    private Vector2 _direction;

    public void Init(float speed, int damage, float lifetime, DamageType damageType)
    {
        Speed = speed;
        Damage = damage;
        LifeTime = lifetime;
        DamageType = damageType;
    }
    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection.normalized;
    }

    public Vector2 GetDirection() => _direction;

    public void SetDamageType(DamageType type)
    {
        DamageType = type;
    }

    public abstract void Move();
}
