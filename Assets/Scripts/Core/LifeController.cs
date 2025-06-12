using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;
    EnemyDrop drop;

    [SerializeField]
    private int scoreValue = 10; // Punteggio da aggiungere quando il nemico muore
    private void Awake()
    {
        currentHealth = maxHealth;
        drop = GetComponent<EnemyDrop>();
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Riduci la salute corrente

        Debug.Log($"{gameObject.name} ha ricevuto {amount} danno. Vita rimanente: {currentHealth}");

        // Gestione del pop dei danni
        if (DamagePopUpManager.Instance != null)
        {
            // Ottieni la posizione del GameObject che ha subito il danno
            Vector3 popUpPosition = transform.position;

            // Piccolo offset per posizionare il numero leggermente sopra il nemico
            popUpPosition.y += 0.5f; // Valore da adattare in base alla dimensione del gameObject
            // Spawna il pop-up di danno
            DamagePopUpManager.Instance.SpawnDamagePopUp(popUpPosition, Mathf.RoundToInt(amount), Color.red); // Colore rosso per il danno
        }


        // Controlla se la salute è scesa a zero o meno
        if (currentHealth <= 0)
        {
            Die(); // Chiama il metodo per la morte
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} è morto!");
        GameManager.Instance.AddScore(scoreValue);
        drop.Drop(gameObject.transform.position);

        
        // Distrugge l'intero GameObject a cui è attaccato questo script
        Destroy(gameObject);
        // Puoi aggiungere qui effetti di morte (es. animazioni, particelle, suono)
        // Oppure logica per gestire la morte del giocatore 
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}