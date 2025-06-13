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
    private PlayerAimController playerAimController; // Potrebbe non servire più per il flip, ma lo lasciamo per altri scopi

    [Header("Animation Settings")]
    [Tooltip("La soglia minima di movimento per attivare l'animazione di camminata.")]
    [SerializeField]
    private float movementThreshold = 0.1f; 

    private bool isMoving; // Variabile per tenere traccia dello stato di movimento
    

    private void Awake()
    {
        if (playerAnimator == null) playerAnimator = GetComponent<Animator>();
        if (playerSpriteRenderer == null) playerSpriteRenderer = GetComponent<SpriteRenderer>();

        playerController = GetComponent<PlayerController>();
        playerAimController = GetComponent<PlayerAimController>(); // Ancora utile per l'aim, ma non per il flip

        if (playerAnimator == null) Debug.LogError("PlayerAnimation: Animator non trovato.");
        if (playerSpriteRenderer == null) Debug.LogError("PlayerAnimation: SpriteRenderer non trovato.");
        if (playerController == null) Debug.LogError("PlayerAnimation: PlayerController non trovato.");
    }

    private void Update()
    {
        // Aggiorna lo stato di movimento all'inizio di Update
        isMoving = playerController.Direction.magnitude > movementThreshold;
        

        HandleMovementAnimation();

        HandleSpriteOrientationAndFlip(); 
    }

    /// Controlla se il giocatore si sta muovendo e imposta il parametro 'IsWalking' nell'Animator.
    /// Imposta anche i parametri di direzione di movimento per il Walk Blend Tree.
    private void HandleMovementAnimation()
    {
        if (playerAnimator == null || playerController == null) return;
        
        playerAnimator.SetBool("IsWalking", isMoving);

        // La gestione di IsSprinting può essere semplificata se è solo un flag
        playerAnimator.SetBool("IsSprinting", playerController.isSprinting); 

        // I parametri MoveDirectionX/Y sono sempre aggiornati in base alla direzione di movimento
        // sia in camminata che in corsa
        playerAnimator.SetFloat("MoveDirectionX", playerController.Direction.x);
        playerAnimator.SetFloat("MoveDirectionY", playerController.Direction.y);
    }

    /// Gestisce l'orientamento dello sprite (flipX) basandosi *unicamente* sulla direzione di movimento.
    /// Imposta anche i parametri 'LookDirectionX/Y' per l'animazione di Idle (che ora seguiranno il movimento).
    private void HandleSpriteOrientationAndFlip()
    {
        if (playerSpriteRenderer == null || playerAnimator == null) return; // Non serve più playerAimController per il flip

        // Il flipX dello sprite ora dipende SOLO dalla direzione di movimento orizzontale
        float horizontalFlipDirection = playerController.Direction.x;

        // Se il player si muove orizzontalmente
        if (horizontalFlipDirection < 0) 
        {
            playerSpriteRenderer.flipX = true; // Guarda a sinistra
        }
        else if (horizontalFlipDirection > 0) 
        {
            playerSpriteRenderer.flipX = false; // Guarda a destra
        }

        {
             playerAnimator.SetFloat("LookDirectionX", playerController.Direction.x);
             playerAnimator.SetFloat("LookDirectionY", playerController.Direction.y);
        }

    }
}