using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float lifeTime = 3f;

    private Vector2 direction;

    // Metodo per inizializzare il proiettile con una direzione
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Distrugge il proiettile dopo un certo tempo 
    }

    private void Update()
    {
        // Muove il proiettile usando Transform.Translate
        // Questo metodo è semplice per i proiettili che non interagiscono pesantemente con la fisica
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Esempio: se il proiettile colpisce un "Enemy"
        // Dovresti avere un modo per identificare i nemici (es. Tag, Layer)
        if (other.CompareTag("Enemy")) // Assicurati che i tuoi nemici abbiano il Tag "Enemy"
        {
            // Qui dovresti richiamare una funzione per infliggere danno al nemico
            // Per ora, useremo un Debug.Log
            Debug.Log($"Proiettile ha colpito un Nemico! Danno: {damage}");
            other.GetComponent<LifeController>().TakeDamage(damage); // Esempio: chiama un metodo di un componente di salute

            // Distrugge il proiettile dopo aver inflitto danno
            Destroy(gameObject);
        }
        // Esempio: se il proiettile colpisce un "Obstacle"
        else if (other.CompareTag("Obstacle")) // Assicurati che i tuoi ostacoli abbiano il Tag "Obstacle"
        {
            Debug.Log("Proiettile ha colpito un Ostacolo!");
            // Distrugge il proiettile all'impatto con un ostacolo
            Destroy(gameObject);
        }
        // Puoi aggiungere altre condizioni per collisioni specifiche
    }
}