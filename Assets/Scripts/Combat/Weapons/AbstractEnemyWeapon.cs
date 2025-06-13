using UnityEngine;

public abstract class AbstractEnemyWeapon : MonoBehaviour
{
    private float _attackRate;
    private int _damageBuff;
    private float _attackRateBuff;

    private float _lastAttackTime;

    protected PlayerController playerController;
    private GameObject _currentBulletPrefab;

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float firePointDistance = 0.1f;

    public void Init(float attackRate, int damageBuff, float attackRateBuff)
    {
        _attackRate = attackRate;
        _damageBuff = damageBuff;
        _attackRateBuff = attackRateBuff;
    }

    public float GetAttackRate() => _attackRate;
    public void SetAttackRate(float value) => _attackRate = value;

    public int GetDamageBuff() => _damageBuff;
    public void SetDamageBuff(int value) => _damageBuff = value;

    public float GetAttackRateBuff() => _attackRateBuff;
    public void SetAttackRateBuff(float value) => _attackRateBuff = value;

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;

        if (controller != null && controller.FirePoint != null)
        {
            firePoint = controller.FirePoint;
        }
        else
        {
            if (firePoint == null)
            {
                GameObject firePointGO = new GameObject("AutoFirePoint");
                firePoint = firePointGO.transform;
                firePoint.SetParent(transform);
                firePoint.localPosition = Vector3.zero;
            }
        }
    }

    public virtual void SetBulletPrefab(GameObject newBullet)
    {
        if (newBullet != null)
        {
            _currentBulletPrefab = newBullet;
        }
    }

    public GameObject GetCurrentBulletPrefab() => _currentBulletPrefab;

    protected virtual void Update()
    {
        if (CanShoot())
        {
            if (playerController != null)
            {
                Vector2 direction = playerController.Direction;
                // Qui non chiamiamo SetShootDirection, lascia all'erede
            }

            Shoot();
            ResetAttackTimer();
        }
    }

    protected virtual void FixedUpdate()
    {
        if (playerController != null)
        {
            Vector2 direction = playerController.Direction;
            UpdateFirePointPosition(direction);
        }
    }

    protected virtual void UpdateFirePointPosition(Vector2 direction)
    {
        if (firePoint == null || direction == Vector2.zero) return;

        firePoint.localPosition = direction.normalized * firePointDistance;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    protected bool CanShoot()
    {
        return Time.time >= _lastAttackTime + 1f / _attackRate;
    }

    protected void ResetAttackTimer()
    {
        _lastAttackTime = Time.time;
    }

    public abstract void SetShootDirection(Vector2 newDirection);
    public abstract void Shoot();
    public abstract void LevelUp();
}
