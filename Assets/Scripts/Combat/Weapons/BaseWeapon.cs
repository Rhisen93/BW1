using UnityEngine;

public class BaseWeapon : AbstractWeapon
{
    [SerializeField] private GameObject bulletPrefab;        

    private Vector2 shootDirection = Vector2.right;

    public void Awake()
    {
        Init(2f, 0, 0);

        if (CurrentBulletPrefab == null && bulletPrefab != null)
        {
            CurrentBulletPrefab = bulletPrefab;
        }
    }

    public override void Shoot()
    {
        
        if (CurrentBulletPrefab == null || firePoint == null) return;

        GameObject bulletGO = Instantiate(CurrentBulletPrefab, firePoint.position, Quaternion.identity);
        AbstractBullet bullet = bulletGO.GetComponent<AbstractBullet>();
        bullet.SetDirection(shootDirection);

        if (bullet == null)
        {
            Debug.LogError("Il prefab del proiettile non ha uno script AbstractBullet!");
        }
        else
        {
            bullet.SetDirection(shootDirection);
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
