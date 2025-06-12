using UnityEngine;

public class BaseWeapon : AbstractWeapon
{
    [SerializeField] private GameObject bulletPrefab;
    private Vector2 shootDirection = Vector2.right;

    private void Awake()
    {
        Init(2f, 0, 0);
        SetBulletPrefab(bulletPrefab);
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
            Debug.Log($"Sparo bullet {bullet.name} in direzione {shootDirection}");
        }
        else
        {
            Debug.LogError("Il prefab del proiettile non ha uno script AbstractBullet!");
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
