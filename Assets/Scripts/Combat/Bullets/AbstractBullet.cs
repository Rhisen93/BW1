using UnityEngine;

public abstract class AbstractBullet : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private float _lifeTime;
    private DamageType _damageType;
    private Vector2 _direction;
    
    public void Init(float speed, float damage, float lifeTime, DamageType damageType, Vector2 initialDirection)
    {
        _speed = speed;
        _damage = damage;
        _lifeTime = lifeTime;
        _damageType = damageType;
        SetDirection(initialDirection);

        Destroy(gameObject, _lifeTime); 
    }

    public float Speed
    {
        get => _speed;
        protected set => _speed = value;
    }

    public float Damage
    {
        get => _damage;
        protected set => _damage = value;
    }

    public float LifeTime
    {
        get => _lifeTime;
        protected set => _lifeTime = value;
    }

    public DamageType DamageType => _damageType;

    public void SetDirection(Vector2 newDirection)
    {
        _direction = newDirection.normalized;
    }

    public Vector2 GetDirection() => _direction;
    
    public abstract void Move();
}
