using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Animator playerAnimator; 
    [SerializeField]
    private SpriteRenderer playerSpriteRenderer; 
    
    // Riferimenti ad altri script sul Player
    private PlayerController playerController;
    private PlayerAimController playerAimController;

    [Header("Animation Settings")]
    [Tooltip("La soglia minima di movimento per attivare l'animazione di camminata.")]
    [SerializeField]
    private float movementThreshold = 0.1f; 

    private bool isMoving; // Nuova variabile per tenere traccia dello stato di movimento

    private void Awake()
    {
        if (playerAnimator == null) playerAnimator = GetComponent<Animator>();
        if (playerSpriteRenderer == null) playerSpriteRenderer = GetComponent<SpriteRenderer>();

        playerController = GetComponent<PlayerController>();
        playerAimController = GetComponent<PlayerAimController>();

        if (playerAnimator == null) Debug.LogError("PlayerAnimation: Animator non trovato.");
        if (playerSpriteRenderer == null) Debug.LogError("PlayerAnimation: SpriteRenderer non trovato.");
        if (playerController == null) Debug.LogError("PlayerAnimation: PlayerController non trovato.");
        if (playerAimController == null) Debug.LogError("PlayerAnimation: PlayerAimController non trovato.");
    }

    private void Update()
    {
        // Aggiorna lo stato di movimento all'inizio di Update
        isMoving = playerController.Direction.magnitude > movementThreshold;

        // Gestione delle animazioni di movimento (Idle/Walk)
        HandleMovementAnimation();

        // Gestione dell'orientamento del corpo e del flip dello sprite
        HandleSpriteOrientationAndFlip(); 
    }

    /// <summary>
    /// Controlla se il giocatore si sta muovendo e imposta il parametro 'IsWalking' nell'Animator.
    /// Imposta anche i parametri di direzione di movimento per il Walk Blend Tree.
    /// </summary>
    private void HandleMovementAnimation()
    {
        if (playerAnimator == null || playerController == null) return;
        
        playerAnimator.SetBool("IsWalking", isMoving);

        // Se il player si sta muovendo, usa la sua direzione di movimento per le animazioni di camminata
        if (isMoving)
        {
            playerAnimator.SetFloat("MoveDirectionX", playerController.Direction.x);
            playerAnimator.SetFloat("MoveDirectionY", playerController.Direction.y);
        }
        else
        {
            // Quando non si muove, resetta la direzione di movimento a zero
            playerAnimator.SetFloat("MoveDirectionX", 0);
            playerAnimator.SetFloat("MoveDirectionY", 0);
        }
    }

    /// <summary>
    /// Gestisce l'orientamento dello sprite (flipX) basandosi sulla direzione di movimento o del mouse.
    /// Imposta anche i parametri 'LookDirectionX/Y' per l'animazione di Idle.
    /// </summary>
    private void HandleSpriteOrientationAndFlip()
    {
        if (playerSpriteRenderer == null || playerAimController == null || playerAnimator == null) return;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = transform.position.z; 
        
        Vector2 directionToMouse = (mouseWorldPosition - transform.position).normalized;

        // I parametri LookDirectionX/Y sono sempre aggiornati in base al mouse per l'Idle Blend Tree
        playerAnimator.SetFloat("LookDirectionX", directionToMouse.x);
        playerAnimator.SetFloat("LookDirectionY", directionToMouse.y);

        // *** NUOVA LOGICA PER IL FLIP X ***
        float horizontalFlipDirection = 0;

        if (isMoving) // Se il player si sta muovendo
        {
            horizontalFlipDirection = playerController.Direction.x;
        }
        else // Se il player è fermo
        {
            horizontalFlipDirection = directionToMouse.x;
        }

        // Applica il flipX in base alla direzione orizzontale rilevata
        if (horizontalFlipDirection < 0) 
        {
            playerSpriteRenderer.flipX = true; 
        }
        else if (horizontalFlipDirection > 0) 
        {
            playerSpriteRenderer.flipX = false; 
        }
        // Se horizontalFlipDirection è 0 (es. movimento verticale puro o fermo senza input orizzontale del mouse),
        // lo sprite mantiene il suo ultimo orientamento.
    }
}