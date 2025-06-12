using UnityEngine;
using TMPro; // Solo se vuoi fare controlli interni, non strettamente necessario per questo manager

public class DamagePopUpManager : MonoBehaviour
{
    public static DamagePopUpManager Instance { get; private set; } // Singleton Pattern

    [Header("Settings")]
    [Tooltip("Il prefabbricato del numero di danno fluttuante (con lo script DamagePopUp).")]
    [SerializeField]
    private DamagePopUp damagePopUpPrefab; // Riferimento al prefabbricato con lo script DamagePopUp già attaccato

    [Tooltip("La Canvas di tipo World Space su cui verranno istanziati i pop-up di danno.")]
    [SerializeField]
    private Canvas damagePopUpCanvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // Non necessario se il manager è specifico per la scena di gioco
        }

        if (damagePopUpPrefab == null)
        {
            Debug.LogError("DamagePopUpManager: Prefabbricato DamagePopUp non assegnato!");
            enabled = false;
        }
        if (damagePopUpCanvas == null)
        {
            Debug.LogError("DamagePopUpManager: Canvas di tipo World Space non assegnata!");
            enabled = false;
        }
    }

    /// <summary>
    /// Spawna un numero di danno fluttuante nella posizione specificata.
    /// </summary>
    /// <param name="position">La posizione nel mondo dove apparirà il numero.</param>
    /// <param name="damageAmount">La quantità di danno da visualizzare.</param>
    /// <param name="color">Colore opzionale del testo.</param>
    public void SpawnDamagePopUp(Vector3 position, int damageAmount, Color? color = null)
    {
        if (damagePopUpPrefab == null || damagePopUpCanvas == null)
        {
            Debug.LogError("DamagePopUpManager: Impossibile spawnare pop-up. Prefabbricato o Canvas non assegnati.");
            return;
        }

        // Istanzia il prefabbricato come figlio della Canvas World Space
        DamagePopUp popUp = Instantiate(damagePopUpPrefab, damagePopUpCanvas.transform);
        
        // Imposta la posizione del pop-up (assicurati che sia World Position)
        popUp.transform.position = position;

        // Imposta il valore del danno e il colore
        popUp.SetDamageAmount(damageAmount, color);
    }
}