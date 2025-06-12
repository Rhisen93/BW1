using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefabToInstantiate;
    [SerializeField] private GameObject bulletPrefabToAssign;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController playerController = other.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("PlayerController mancante sul player!");
            return;
        }
        
        AbstractWeapon existingWeapon = other.GetComponentInChildren<AbstractWeapon>();
                
        if (weaponPrefabToInstantiate != null)
        {
            string newWeaponName = weaponPrefabToInstantiate.name.Replace("(Clone)", "");
            bool weaponAlreadyExists = existingWeapon != null && existingWeapon.GetType() == weaponPrefabToInstantiate.GetComponent<AbstractWeapon>().GetType();

            if (weaponAlreadyExists)
            {
                existingWeapon.LevelUp();
                Debug.Log("LevelUp dell'arma attuale eseguito.");
            }
            else
            {                
                GameObject weaponGO = Instantiate(weaponPrefabToInstantiate, other.transform);
                weaponGO.transform.localPosition = Vector3.zero;

                AbstractWeapon newWeapon = weaponGO.GetComponent<AbstractWeapon>();
                if (newWeapon != null)
                {
                    newWeapon.SetPlayerController(playerController);
                                        
                    if (bulletPrefabToAssign != null)
                    {
                        newWeapon.SetBulletPrefab(bulletPrefabToAssign);
                    }
                }
            }
        }

        if (bulletPrefabToAssign != null && existingWeapon != null)
        {
            GameObject currentBulletPrefab = existingWeapon.GetCurrentBulletPrefab();

            if (currentBulletPrefab != null)
            {
                DamageType newType = bulletPrefabToAssign.GetComponent<AbstractBullet>().GetDamageType();

                bool hasEffect = currentBulletPrefab.GetComponent(newType switch
                {
                    DamageType.ICE => typeof(IceBullet),
                    DamageType.POISON => typeof(PoisonBullet),
                    _ => null
                }) != null;

                if (hasEffect)
                {
                    AbstractBullet bulletEffect = (AbstractBullet)currentBulletPrefab.GetComponent(newType switch
                    {
                        DamageType.ICE => typeof(IceBullet),
                        DamageType.POISON => typeof(PoisonBullet),
                        _ => null
                    });

                    bulletEffect?.LevelUp();
                    Debug.Log($"LevelUp dell'effetto {newType} sul bullet attivo.");
                }
                else
                {
                    GameObject newBullet = Instantiate(currentBulletPrefab);
                    Destroy(newBullet.GetComponent<AbstractBullet>()); 

                    AbstractBullet baseScript = currentBulletPrefab.GetComponent<AbstractBullet>();
                    AbstractBullet newBaseScript = newBullet.AddComponent(baseScript.GetType()) as AbstractBullet;

                    newBaseScript.SetDamage(baseScript.GetDamage());
                    newBaseScript.SetSpeed(baseScript.GetSpeed());
                    newBaseScript.SetLifeTime(baseScript.GetLifeTime());
                    newBaseScript.SetDamageType(baseScript.GetDamageType());


                    if (newType == DamageType.ICE)
                    {
                        newBullet.AddComponent<IceBullet>();
                    }
                    else if (newType == DamageType.POISON)
                    {
                        newBullet.AddComponent<PoisonBullet>();
                    }

                    existingWeapon.SetBulletPrefab(newBullet);
                    Debug.Log($"Aggiunto effetto {newType} al bullet attivo.");
                }
            }
            else
            {
                existingWeapon.SetBulletPrefab(bulletPrefabToAssign);
                Debug.Log("Bullet iniziale assegnato.");
            }
        }


        Destroy(gameObject);
    }
}
