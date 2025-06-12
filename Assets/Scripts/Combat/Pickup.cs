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
            existingWeapon.SetBulletPrefab(bulletPrefabToAssign);
            Debug.Log("Bullet aggiornato all'arma attiva.");
        }

        Destroy(gameObject);
    }
}
