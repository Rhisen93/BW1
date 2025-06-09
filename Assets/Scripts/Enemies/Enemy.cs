using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 3f; 
    [SerializeField]
    private float damageToPlayerOnCollision = 20f; 

    private Transform playerTransform; // Riferimento alla posizione del giocatore
    private Rigidbody2D rb; 
    private LifeController lifeController;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Enemy richiede un componente Rigidbody2D sullo stesso GameObject.");
        }
        lifeController = GetComponent<LifeController>();
        if (lifeController == null)
        {
            Debug.LogError("Enemy richiede un componente LifeController sullo stesso GameObject.");
        }
    }

    private void Start()
    {
        // Cerca il GameObject del giocatore tramite il suo tag "Player"
        GameObject playerGO = GameObject.FindWithTag("Player");
        if (playerGO != null)
        {
            playerTransform = playerGO.transform;
        }
        else
        {
            Debug.LogError("Player non trovato! Assicurati che il tuo Player abbia il tag 'Player'.");
        }
    }

    private void FixedUpdate()
    {
        // Muovi automaticamente verso il Player
        if (playerTransform != null) // Si muove solo se il Player è stato trovato
        {
            // Calcola la direzione verso il player
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;

            // Calcola la nuova posizione desiderata
            Vector2 newPosition = rb.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime;

            // Muovi il Rigidbody del nemico
            rb.MovePosition(newPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si distrugge se collide con il Player (dopo avergli inflitto danno)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ottieni il LifeController del Player e infliggi danno
            LifeController playerLife = collision.gameObject.GetComponent<LifeController>();
            if (playerLife != null)
            {
                playerLife.TakeDamage(damageToPlayerOnCollision);
            }
            else
            {
                Debug.LogWarning("Il Player non ha un componente LifeController!");
            }
            Destroy(gameObject); // Distrugge il nemico dopo la collisione con il Player
        }
        // Puoi aggiungere logica per collisioni con altri oggetti (es. muri, altri nemici)
    }
}