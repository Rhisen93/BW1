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

        // GESTIONE ARMA
        if (weaponPrefabToInstantiate != null)
        {
            string newWeaponName = weaponPrefabToInstantiate.name.Replace("(Clone)", "");
            bool weaponAlreadyExists = existingWeapon != null &&
                                       existingWeapon.GetType() == weaponPrefabToInstantiate.GetComponent<AbstractWeapon>().GetType();

            if (weaponAlreadyExists)
            {
                existingWeapon.LevelUp();
                Debug.Log($"LevelUp dell'arma {newWeaponName} eseguito.");
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
                        newWeapon.SetBulletPrefab(bulletPrefabToAssign);
                }

                Debug.Log($"Nuova arma {newWeaponName} raccolta.");
            }
        }

        // GESTIONE BULLET
        if (bulletPrefabToAssign != null)
        {
            AbstractWeapon[] allWeapons = other.GetComponentsInChildren<AbstractWeapon>(true); 
            foreach (var weapon in allWeapons)
            {
                weapon.SetBulletPrefab(bulletPrefabToAssign);
                Debug.Log($"[PICKUP] Bullet {bulletPrefabToAssign.name} assegnato a {weapon.name}");
            }
        }

        Destroy(gameObject);
    }
}
