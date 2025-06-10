using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponPrefabToInstantiate; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAimController playerAimController = other.GetComponent<PlayerAimController>();
            if (playerAimController == null)
            {
                Debug.LogError("Il Player non ha un componente PlayerAimController! Impossibile equipaggiare l'arma.");
                return;
            }

            Transform playerFirePoint = playerAimController.GetPlayerFirePoint();
            if (playerFirePoint == null)
            {
                Debug.LogError("PlayerAimController non ha un FirePoint assegnato!");
                return;
            }

            if (weaponPrefabToInstantiate != null)
            {
               
                var existingWeapons = other.GetComponentsInChildren<AbstractWeapon>();
                foreach (var weapon in existingWeapons)
                {
                    Destroy(weapon.gameObject);
                }

                
                GameObject instantiatedWeapon = Instantiate(weaponPrefabToInstantiate, other.transform);
                instantiatedWeapon.transform.localPosition = Vector3.zero;
                instantiatedWeapon.transform.localRotation = Quaternion.identity;

                Debug.Log($"Il giocatore ha raccolto {weaponPrefabToInstantiate.name}.");

                
                var equippedWeapon = instantiatedWeapon.GetComponent<AbstractWeapon>();
                if (equippedWeapon != null)
                {
                    
                    var setFirePointMethod = equippedWeapon.GetType().GetMethod("SetFirePoint");
                    if (setFirePointMethod != null)
                    {
                        setFirePointMethod.Invoke(equippedWeapon, new object[] { playerFirePoint });
                        Debug.Log($"FirePoint del Player assegnato all'arma {equippedWeapon.GetType().Name}.");
                    }
                    else
                    {
                        Debug.LogWarning("L'arma non implementa SetFirePoint.");
                    }
                }
                else
                {
                    Debug.LogError("L'arma istanziata non deriva da AbstractWeapon!");
                }
            }
            else
            {
                Debug.LogWarning("Nessun prefab dell'arma assegnato al Pickup!");
            }

            Destroy(gameObject);
        }
    }
}