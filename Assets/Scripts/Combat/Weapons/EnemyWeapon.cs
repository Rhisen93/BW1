using UnityEngine;

public class EnemyWeapon : AbstractEnemyWeapon
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
        if (GetCurrentBulletPrefab() == null || firePoint == null) return;

        GameObject bulletGO = Instantiate(GetCurrentBulletPrefab(), firePoint.position, Quaternion.identity);
        BaseEnemyBullet bullet = bulletGO.GetComponent<BaseEnemyBullet>();


        if (bullet != null)
        {
            bullet.SetDirection(shootDirection);
        }
        else
        {
            Debug.LogError("Il prefab del proiettile non ha uno script AbstractBullet!");
        }
    }


    public override void SetShootDirection(Vector2 newDirection)
    {
        if (newDirection != Vector2.zero)
        {
            shootDirection = newDirection.normalized;
        }
    }

    public void SetDirection(Vector2 dir)
    {
        SetShootDirection(dir);
    }

    public override void LevelUp()
    {
        // Qui puoi implementare potenziamenti dell’arma
    }
}
