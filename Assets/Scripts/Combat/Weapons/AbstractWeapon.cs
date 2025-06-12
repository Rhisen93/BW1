using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    protected float AttackRate { get; set; }
    protected int DamageBuff { get; set; }
    protected float AttackRateBuff { get; set; }

    private float _lastAttackTime;

    protected PlayerController playerController;
    protected GameObject CurrentBulletPrefab { get; set; }

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float firePointDistance = 0.1f;

    public void Init(float attackRate, int damageBuff, float attackRateBuff)
    {
        AttackRate = attackRate;
        DamageBuff = damageBuff;
        AttackRateBuff = attackRateBuff;
    }

    public void SetPlayerController(PlayerController controller)
    {
        playerController = controller;

        if (controller.FirePoint != null)
        {
            firePoint = controller.FirePoint;
        }
        else
        {
            GameObject firePointGO = new GameObject("AutoFirePoint");
            firePoint = firePointGO.transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = Vector3.zero;
        }
    }

    public virtual void SetBulletPrefab(GameObject newBullet)
    {
        if (newBullet != null)
        {
            CurrentBulletPrefab = newBullet;
        }
    }

    public GameObject GetCurrentBulletPrefab()
    {
        return CurrentBulletPrefab;
    }

    protected virtual void Update()
    {
        if (CanShoot())
        {
            if (playerController != null)
            {
                Vector2 direction = playerController.Direction;
                SetShootDirection(direction);
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
        return Time.time >= _lastAttackTime + 1f / AttackRate;
    }

    protected void ResetAttackTimer()
    {
        _lastAttackTime = Time.time;
    }

    protected abstract void SetShootDirection(Vector2 newDirection);
    public abstract void Shoot();
    public abstract void LevelUp();
}
