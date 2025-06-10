using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    private float _attackRate;
    private int _damageBuff;
    private float _attackRateBuff;
    private float _lastAttackTime;

    public float AttackRate => _attackRate;

    public int DamageBuff
    {
        get => _damageBuff;
        protected set => _damageBuff = value;
    }

    public float AttackRateBuff
    {
        get => _attackRateBuff;
        protected set => _attackRateBuff = value;
    }

    public void Init(float attackRate, int damageBuff, float attackRateBuff)
    {
        _attackRate = attackRate;
        _damageBuff = damageBuff;
        _attackRateBuff = attackRateBuff;
    }

    protected bool CanShoot()
    {
        return Time.time >= _lastAttackTime + 1f / _attackRate;
    }

    protected void ResetAttackTimer()
    {
        _lastAttackTime = Time.time;
    }

    public abstract void Shoot();
    public abstract void LevelUp();
}
