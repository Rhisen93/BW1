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

   

    private void LateUpdate()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.LogWarning("CameraFollow: Target assegnato automaticamente al player con tag 'Player'.");
            }
            else
            {
                Debug.LogWarning("CameraFollow: Nessun oggetto trovato con il tag 'Player'.");
                return;
            }
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
    
}