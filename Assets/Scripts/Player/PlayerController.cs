using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f; 
    [SerializeField]
    private float sprintMultiplier = 1.5f;
    public Transform FirePoint { get; private set; }

    private float horizontal;
    private float vertical;

    private Rigidbody2D rb; 

    
    public Vector2 Direction { get; private set; }
    public bool isSprinting;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        FirePoint = transform.Find("FirePoint");

        if (rb == null)
        {
            Debug.LogError("PlayerController richiede un componente Rigidbody2D sullo stesso GameObject.");
        }

        if (FirePoint == null)
        {
            Debug.LogError("FirePoint non trovato nel Player!");
        }

        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }
    public Vector2 GetMouseDirection()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        return direction;
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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Direction.magnitude > 0.1f)
                {
                    isSprinting = true;
                    rb.velocity = Direction * (speed * sprintMultiplier);
                }
                else
                {
                    rb.velocity = Vector2.zero;
                }
            }
            else if (Direction.magnitude > 0.1f)
            {
                isSprinting = false;
                rb.velocity = Direction * speed;
            }
            else // Se non c'è input di movimento
            {
                // Azzera la velocità del Rigidbody per fermare qualsiasi movimento residuo
                // Il Linear Drag elevato sul Rigidbody2D garantirà che questo sia istantaneo.
                isSprinting=false;
                rb.velocity = Vector2.zero;
            }
            
        }
    }

}