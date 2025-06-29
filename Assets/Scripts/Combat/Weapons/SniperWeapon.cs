using UnityEngine;

public class SniperWeapon : AbstractWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float customSpeed = 15f;
    [SerializeField] private int customDamage = 15;
    [SerializeField] private float customLifeTime = 1.5f;    

    private Camera mainCamera;
    private Vector2 currentShootDirection;

    private void Awake()
    {
        Init(0.5f, 0, 0);
        if (GetCurrentBulletPrefab() == null)
            SetBulletPrefab(bulletPrefab);

        mainCamera = Camera.main;
    }

    public override void Shoot()
    {
        GameObject bulletPrefab = GetCurrentBulletPrefab();
        if (bulletPrefab == null || firePoint == null || mainCamera == null) return;

        SetShootDirection(Vector2.zero);

        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        AbstractBullet bullet = bulletGO.GetComponent<AbstractBullet>();

        if (bullet != null)
        {
            bullet.SetDirection(currentShootDirection);
            bullet.Init(customSpeed, customDamage, customLifeTime);
            bullet.Init(customSpeed, customDamage + GetDamageBuff(), customLifeTime + GetLifeTimeBuff());

        }
    }


    protected override void SetShootDirection(Vector2 unused)
    {
        if (mainCamera == null || firePoint == null) return;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        currentShootDirection = new Vector2(
            mouseWorldPos.x - firePoint.position.x,
            mouseWorldPos.y - firePoint.position.y
        ).normalized;
    }

    public override void LevelUp()
    {
        SetDamageBuff(GetDamageBuff() + 3);
        SetAttackRateBuff(GetAttackRateBuff() + 0.1f);
        SetLifeTimeBuff(GetLifeTimeBuff() + 0.3f);
    }
}
