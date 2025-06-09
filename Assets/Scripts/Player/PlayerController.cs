using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; // Velocità di movimento del giocatore

    private float horizontal;
    private float vertical;

    private Rigidbody2D rb; // Riferimento al componente Rigidbody2D del giocatore

    // Proprietà pubblica per la direzione di movimento, accessibile in lettura
    public Vector2 Direction { get; private set; }

    // Awake viene chiamato quando l'istanza dello script viene caricata
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerController richiede un componente Rigidbody2D sullo stesso GameObject.");
        }
        // Congela la rotazione Z per un player 2D
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }

    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Direction = new Vector2(horizontal, vertical).normalized;
    }

    // FixedUpdate è chiamato a intervalli di tempo fissi, ideale per la fisica
    private void FixedUpdate()
    {
        MovePlayer();
    }


    /// Gestisce il movimento del giocatore
    private void MovePlayer()
    {
        if (rb != null)
        {
            // Se c'è input di movimento
            if (Direction.magnitude > 0.1f)
            {
                // Imposta direttamente la velocità del Rigidbody nella direzione e con la velocità desiderata
                rb.velocity = Direction * speed;
            }
            else // Se non c'è input di movimento
            {
                // Azzera la velocità del Rigidbody per fermare qualsiasi movimento residuo
                // Il Linear Drag elevato sul Rigidbody2D garantirà che questo sia istantaneo.
                rb.velocity = Vector2.zero;
            }
        }
    }

}