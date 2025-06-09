using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponPrefabToInstantiate; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Ottieni il PlayerAimController dal giocatore
            PlayerAimController playerAimController = other.GetComponent<PlayerAimController>();
            if (playerAimController == null)
            {
                Debug.LogError("Il Player non ha un componente PlayerAimController! Impossibile equipaggiare l'arma.");
                return;
            }

            // Ottieni il FirePoint del Player
            Transform playerFirePoint = playerAimController.GetPlayerFirePoint();
            if (playerFirePoint == null)
            {
                Debug.LogError("PlayerAimController non ha un FirePoint assegnato!");
                return;
            }

           
            if (weaponPrefabToInstantiate != null)
            {
                // Trova e distruggi tutte le armi (con componente Gun) già presenti sul player
                Gun[] existingGuns = other.GetComponentsInChildren<Gun>();
                foreach (Gun gun in existingGuns)
                {
                    Destroy(gun.gameObject); // Distruggi l'intero GameObject dell'arma precedente
                }
                
                // Istanzia il prefab dell'arma come figlio del giocatore
                GameObject instantiatedWeapon = Instantiate(weaponPrefabToInstantiate, other.transform);
                instantiatedWeapon.transform.localPosition = Vector3.zero; // Posiziona l'arma al centro del giocatore
                instantiatedWeapon.transform.localRotation = Quaternion.identity; // Nessuna rotazione aggiuntiva

                Debug.Log($"Il giocatore ha raccolto {weaponPrefabToInstantiate.name}.");

                // Ottieni il componente Gun dall'arma appena istanziata
                Gun equippedGun = instantiatedWeapon.GetComponent<Gun>();
                if (equippedGun != null)
                {
                    // Assegna il FirePoint del Player all'arma equipaggiata
                    equippedGun.SetFirePoint(playerFirePoint);
                    Debug.Log($"FirePoint del Player assegnato all'arma {equippedGun.name}.");
                }
                else
                {
                    Debug.LogError("L'arma istanziata non ha un componente Gun!");
                }
            }
            else
            {
                Debug.LogWarning("Nessun prefab dell'arma assegnato al Pickup!");
            }

            // Distrugge l'oggetto Pickup dopo essere stato raccolto
            Destroy(gameObject);
        }
    }
}