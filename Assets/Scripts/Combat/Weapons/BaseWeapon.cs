using UnityEngine;

public class BaseWeapon : AbstractWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float bulletDamage = 10f;
    [SerializeField] private float bulletLifeTime = 3f;
    [SerializeField] private DamageType bulletDamageType = DamageType.FIRE;

    private Vector2 shootDirection = Vector2.right;

    private void Update()
    {
        if (CanShoot())
        {
            Shoot();
            ResetAttackTimer(); 
        }
    }

    public override void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BaseBullet bullet = bulletGO.GetComponent<BaseBullet>();
        bullet.Init(bulletSpeed, bulletDamage, bulletLifeTime, bulletDamageType, shootDirection);
    }

    public void SetShootDirection(Vector2 newDirection)
    {
        if (newDirection != Vector2.zero)
        {
            shootDirection = newDirection.normalized;
        }
    }

    public override void LevelUp()
    {
        // Potenziare danni e attacco
    }
}
