using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float fireRate = 0.5f; // Velocità di fuoco - proiettili al secondo (es. 0.5f = 2 proiettili al secondo)

    private Transform firePoint; // Punto da cui i proiettili verranno sparati
    private float nextFireTime = 0f; // Tempo in cui il prossimo proiettile può essere sparato

    public void SetFirePoint(Transform firePointTransform)
    {
        firePoint = firePointTransform;
    }
    private void Update()
    {
        // Controlla se il giocatore preme il tasto di sparo (es. tasto sinistro del mouse o freccia su)
        // e se è passato abbastanza tempo dall'ultimo sparo
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime) // "Fire1" è il tasto sinistro del mouse per default
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate; // Aggiorna il tempo del prossimo sparo
        }
    }

    /// Funzione per sparare un proiettile.
    private void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("Il Prefab del proiettile non è assegnato all'arma");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogError("Il FirePoint non è stato assegnato alla Gun! Impossibile sparare.");
            return;
        }

        // Istanzia il proiettile dalla posizione e rotazione del firePoint
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Bullet bullet = bulletGO.GetComponent<Bullet>();

          if (bullet != null)
        {
            // La direzione di sparo sarà la direzione "in avanti" (asse X o Y, a seconda dell'orientamento) del FirePoint
            // Dato che il FirePoint è ruotato per seguire il mouse, la sua 'right' o 'up' sarà la direzione desiderata.
            // Se il tuo FirePoint è ruotato in modo che l'asse Y (up) punti al mouse, usa currentFirePoint.up
            // Se il tuo FirePoint è ruotato in modo che l'asse X (right) punti al mouse, usa currentFirePoint.right
            bullet.SetDirection(firePoint.right); // Assumo che right sia la direzione di sparo dopo la rotazione
        }
        else
        {
            Debug.LogError("Il Prefab del proiettile non ha il componente Bullet attaccato!");
        }
    }
}