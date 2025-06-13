using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target; 

    [SerializeField]
    private float smoothSpeed = 0.125f; // (valori più piccoli = più lento/morbido)

    [SerializeField]
    private Vector3 offset; // Un offset opzionale per posizionare la telecamera (es. per vederla leggermente in alto o in basso)

    // LateUpdate è chiamato dopo che tutti gli Update sono stati chiamati.
    // È ideale per il movimento della telecamera, in modo che segua l'oggetto dopo che si è mosso.

   /* private void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform; // <- assegna qui il target
                Debug.LogWarning("CameraFollow: Target assegnato automaticamente al player con tag 'Player'.");
            }
            else
            {
                Debug.LogWarning("CameraFollow: Nessun oggetto trovato con il tag 'Player'.");
            }
        }
    }*/

    private void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target non assegnato. Assegna il Player al campo Target nell'Inspector.");
            return; // blocca qui se target è null
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
    /*
    private void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Debug.LogWarning("CameraFollow: Target non assegnato. Assegna il Player al campo Target nell'Inspector.");
            //return;
        }

        // Calcola la posizione desiderata della telecamera
        Vector3 desiredPosition = target.position + offset;

        // Utilizza Vector3.Lerp per un movimento più fluido (interpolazione lineare)
        // La telecamera si sposta gradualmente dalla sua posizione attuale a quella desiderata
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Nota: in un gioco 2D top-down, la componente Z della telecamera deve rimanere fissa
        // per mantenere la prospettiva corretta. L'offset dovrebbe gestire la Z desiderata.
        // Se la tua telecamera è 2D (Orthographic), la sua Z non dovrebbe cambiare dal valore iniziale.
        // Ad esempio, se l'offset fosse Vector3(0, 0, -10), la telecamera manterrebbe la Z a -10.
    }
    */
}