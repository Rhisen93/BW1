using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField]
    private Transform playerFirePoint; // Riferimento al FirePoint del Player (assegnato nell'Inspector)

    [Tooltip("Offset in gradi per la rotazione del FirePoint. Regola questo valore per allineare correttamente lo sprite. " +
             "Spesso -90 o +90 se lo sprite è orientato con 'su' (Y) come direzione 'avanti'.")]
    [SerializeField]
    private float rotationOffset = 0f; // Aggiustamento dell'angolo per allineare lo sprite

    [Tooltip("La distanza massima dal Player a cui il FirePoint può posizionarsi per seguire il mouse.")]
    [SerializeField]
    private float maxFirePointDistance = 2f; // Distanza massima dal player

    // Riferimento al Transform del Player stesso (questo script è attaccato al Player)
    private Transform playerTransform; 

    private void Awake()
    {
        playerTransform = transform; // 'transform' si riferisce al Transform del GameObject a cui è attaccato lo script (il Player)

        if (playerFirePoint == null)
        {
            Debug.LogError("PlayerFirePoint non assegnato in PlayerAimController! Assicurati di trascinare il GameObject PlayerFirePoint nell'Inspector.");
        }
    }

    private void Update()
    {
        if (playerFirePoint == null || Camera.main == null)
        {
            // Debug.LogError("Problemi con PlayerFirePoint o Camera.main!");
            return;
        }

        // 1. Ottieni la posizione del mouse nel mondo di gioco
        Vector3 mouseScreenPosition = Input.mousePosition;
        // Imposta la Z della posizione del mouse sulla stessa Z del Player/FirePoint
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(playerTransform.position).z; 
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
        
        // Calcola la direzione dal centro del Player al mouse
        Vector2 directionToMouse = (mouseWorldPosition - playerTransform.position).normalized;

        // 2. Vincola la posizione del FirePoint a una distanza massima dal Player
        // Calcola la posizione desiderata per il FirePoint
        Vector3 desiredFirePointPosition = (Vector2)playerTransform.position + directionToMouse * maxFirePointDistance;
        
        // Muovi il FirePoint alla posizione calcolata
        playerFirePoint.position = desiredFirePointPosition;

        // 3. Ruota il FirePoint per puntare verso il mouse (o nella direzione calcolata)
        // Calcola l'angolo in gradi rispetto all'asse X positivo
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Applica l'offset per correggere l'orientamento dello sprite
        angle += rotationOffset; 

        // Applica la rotazione al FirePoint
        playerFirePoint.rotation = Quaternion.Euler(0f, 0f, angle);

        // Debug visivo (solo in editor, non influisce sul gioco)
        // Debug.DrawLine(playerTransform.position, playerFirePoint.position, Color.red);
        // Debug.DrawLine(playerFirePoint.position, mouseWorldPosition, Color.blue);
    }

    /// <summary>
    /// Fornisce il riferimento al FirePoint del Player.
    /// Questo metodo sarà chiamato dallo script Gun quando equipaggiato.
    /// </summary>
    public Transform GetPlayerFirePoint()
    {
        return playerFirePoint;
    }
}