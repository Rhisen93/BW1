using UnityEngine;

public class BaseWeapon : AbstractWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    private Vector2 shootDirection = Vector2.right;
    private float baseLifetime;    

    private void Awake()
    {
        Init(3f, 0, 0);
        SetBulletPrefab(bulletPrefab);
    }

    private void Start()
    {
        if (GetCurrentBulletPrefab() != null)
        {
            AbstractBullet bullet = GetCurrentBulletPrefab().GetComponent<AbstractBullet>();
            if (bullet != null)
            {
                baseLifetime = bullet.GetLifeTime(); 
            }
        }
    }

    public override void Shoot()
    {
        if (GetCurrentBulletPrefab() == null || firePoint == null)
        {
            Debug.LogWarning("Nessun proiettile o firePoint non impostato!");
            return;
        }

        GameObject bulletGO = Instantiate(GetCurrentBulletPrefab(), firePoint.position, Quaternion.identity);
        AbstractBullet bullet = bulletGO.GetComponent<AbstractBullet>();

        if (bullet != null)
        {
            bullet.SetDirection(shootDirection);
            bullet.SetDamage(bullet.GetDamage() + GetDamageBuff());
            bullet.SetLifeTime(baseLifetime + GetLifeTimeBuff());

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
        SetDamageBuff(GetDamageBuff() + 2);
        SetAttackRateBuff(GetAttackRateBuff() + 0.2f);
        SetLifeTimeBuff(GetLifeTimeBuff() + 0.2f);

        Debug.Log("Base weapon Level Up!");
    }


}
