using UnityEngine;

public class SpreadWeapon : AbstractWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletCount = 4;
    [SerializeField] private float spreadAngle = 45f;
    [SerializeField] private float customLifeTime = 0.25f;

    private Vector2 shootDirection = Vector2.right;

    private void Awake()
    {
        Init(1f, 0, 0);
        if (GetCurrentBulletPrefab() == null)
            SetBulletPrefab(bulletPrefab);
    }

    public override void Shoot()
    {
        GameObject bulletPrefab = GetCurrentBulletPrefab();
        if (bulletPrefab == null || firePoint == null || shootDirection == Vector2.zero) return;

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + angleStep * i;
            Vector2 spreadDir = Quaternion.Euler(0f, 0f, angle) * shootDirection;

            GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            AbstractBullet bullet = bulletGO.GetComponent<AbstractBullet>();

            if (bullet != null)
            {
                bullet.SetDirection(spreadDir.normalized);
                bullet.SetLifeTime(customLifeTime);
            }
        }
    }


    protected override void SetShootDirection(Vector2 newDirection)
    {
        if (newDirection != Vector2.zero)
        {
            shootDirection = newDirection.normalized;
        }
    }

    public override void LevelUp()
    {
        
    }
}
