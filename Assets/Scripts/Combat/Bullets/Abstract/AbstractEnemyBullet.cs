using UnityEngine;

public abstract class AbstractEnemyBullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _lifeTime = 0.75f;
    private DamageType _damageType;
    protected Vector2 _direction;



    public void Init(float speed, int damage, float lifetime)
    {
        _speed = speed;
        _damage = damage;
        SetLifeTime(lifetime);
    }

    public float GetSpeed() => _speed;
    public int GetDamage() => _damage;
    public float GetLifeTime() => _lifeTime;
    public DamageType GetDamageType() => _damageType;


    public void SetSpeed(float speed) => _speed = speed;
    public void SetDamage(int damage) => _damage = damage;

    public void SetLifeTime(float lifetime)
    {
        _lifeTime = lifetime;
        Destroy(gameObject, _lifeTime);
    }

    public void SetDamageType(DamageType type) => _damageType = type;

    public abstract void SetDirection(Vector2 newDirection);
    public Vector2 GetDirection() => _direction;

    protected float Speed => _speed;
    protected int Damage => _damage;
    protected float LifeTime => _lifeTime;
    protected DamageType DamageType => _damageType;
    public abstract void Move();
    public abstract void ApplyEffect(GameObject target);
    public abstract void LevelUp();
}
