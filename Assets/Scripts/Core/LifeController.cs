using UnityEngine;

public class LifeController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField]
    private int scoreValue = 10; // Punteggio da aggiungere quando il nemico muore
    private void Awake()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Riduci la salute corrente

        Debug.Log($"{gameObject.name} ha ricevuto {amount} danno. Vita rimanente: {currentHealth}");

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