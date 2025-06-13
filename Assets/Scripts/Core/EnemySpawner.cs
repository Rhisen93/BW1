using UnityEngine;
using System.Collections.Generic; // Necessario per List<T>

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [Tooltip("Lista di tutti i prefabbricati dei nemici disponibili.")]
    [SerializeField]
    private GameObject[] allEnemyPrefabs; // Tutti i tipi di nemici che possono spawnare (l'ordine è l'indice)

    [Header("Spawn Settings")]
    [Tooltip("Tempo minimo tra uno spawn e l'altro.")]
    [SerializeField]
    private float minSpawnInterval = 2f;
    [Tooltip("Tempo massimo tra uno spawn e l'altro.")]
    [SerializeField]
    private float maxSpawnInterval = 5f;
    [Tooltip("Margine oltre i bordi della telecamera per lo spawn.")]
    [SerializeField]
    private float spawnMargin = 2f; 
    [Tooltip("Numero massimo di nemici contemporaneamente in scena (iniziale, verrà sovrascritto dalle fasi).")]
    [SerializeField]
    private int maxEnemiesInSceneDefault = 10; // Valore iniziale, sarà gestito dalle fasi
    [Tooltip("Riferimento al Player per evitare spawn troppo vicini.")]
    [SerializeField]
    private Transform playerTransform;
    [Tooltip("Distanza minima di spawn dal Player.")]
    [SerializeField]
    private float minSpawnDistanceFromPlayer = 5f;

    // Struttura per definire le fasi di difficoltà in base al punteggio
    [System.Serializable]
    public class DifficultyPhase
    {
        [Tooltip("Punteggio al quale si attiva questa fase di difficoltà.")]
        public int scoreThreshold;
        [Tooltip("Nuovi prefabbricati di nemici che diventano disponibili da questa fase in poi.")]
        public List<GameObject> newEnemyTypesToUnlock; // Ora sono i prefabbricati diretti!
        [Tooltip("Moltiplicatore per la frequenza di spawn in questa fase (1 = normale, 0.5 = più veloce).")]
        public float spawnIntervalMultiplier = 1f;
        [Tooltip("Massimo nemici in scena per questa fase.")]
        public int maxEnemiesForPhase = 10;
    }

    [Header("Difficulty Progression")]
    [Tooltip("Definisci le fasi di difficoltà basate sul punteggio.")]
    [SerializeField]
    private List<DifficultyPhase> difficultyPhases;

    private float nextSpawnTime;
    private Camera mainCamera;
    private GameManager gameManager; 

    private int currentEnemyCount = 0; // Contatore dei nemici attivi

    // Lista dei nemici attualmente spawnabili 
    private List<GameObject> currentlySpawnableEnemies = new List<GameObject>();
    private int currentPhaseIndex = 0; // Indice della fase di difficoltà attiva

    private void Awake()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("EnemySpawner: Main Camera non trovata! Assicurati che una telecamera abbia il tag 'MainCamera'.");
        }

        gameManager = FindObjectOfType<GameManager>(); 
        if (gameManager == null)
        {
            Debug.LogWarning("EnemySpawner: GameManager non trovato. Lo spawn dei nemici non si adatterà al punteggio.");
        }

        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player"); 
            if (player != null) playerTransform = player.transform;
            else Debug.LogWarning("EnemySpawner: Player non assegnato e non trovato con il tag 'Player'.");
        }

        //  Inizializza la prima fase di difficoltà e sblocca i primi nemici 
        // Assicurati che la prima fase (indice 0) abbia sempre scoreThreshold = 0
        if (difficultyPhases.Count > 0 && difficultyPhases[0].scoreThreshold == 0)
        {
            currentlySpawnableEnemies.AddRange(difficultyPhases[0].newEnemyTypesToUnlock);
        } else {
            Debug.LogError("EnemySpawner: La prima fase di difficoltà (indice 0) deve avere 'Score Threshold' a 0 e 'New Enemy Types To Unlock' configurati.");
        }


        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    private void Update()
    {
        if (mainCamera == null || playerTransform == null) return;

        int currentScore = (gameManager != null) ? gameManager.Score : 0;

        // Controlla e avanza la fase di difficoltà 
        CheckAndAdvanceDifficultyPhase(currentScore);

        // Ottieni la fase di difficoltà corrente ( da currentPhaseIndex)
        DifficultyPhase activePhase = difficultyPhases[currentPhaseIndex];

        // Se il tempo è scaduto e non abbiamo troppi nemici in scena, spawniamo
        if (Time.time >= nextSpawnTime && currentEnemyCount < activePhase.maxEnemiesForPhase)
        {
            SpawnEnemy(activePhase);
            nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval) * activePhase.spawnIntervalMultiplier;
        }
    }

    /// Controlla se il punteggio attuale ha raggiunto la soglia per la prossima fase di difficoltà
    /// e aggiorna i nemici spawnabili e i parametri di spawn.
    /// <param name="currentScore">Il punteggio attuale del giocatore.</param>
    private void CheckAndAdvanceDifficultyPhase(int currentScore)
    {
        // Se non ci sono più fasi o siamo già all'ultima, non fare nulla
        if (currentPhaseIndex + 1 >= difficultyPhases.Count) return;

        // Se il punteggio supera la soglia della prossima fase
        if (currentScore >= difficultyPhases[currentPhaseIndex + 1].scoreThreshold)
        {
            currentPhaseIndex++; // Avanza alla prossima fase
            DifficultyPhase newActivePhase = difficultyPhases[currentPhaseIndex];

            // Sblocca i nuovi tipi di nemici per questa fase
            currentlySpawnableEnemies.AddRange(newActivePhase.newEnemyTypesToUnlock);

            Debug.Log($"EnemySpawner: Avanzato alla fase di difficoltà {currentPhaseIndex}. Nuovi nemici sbloccati.");
        }
    }


    /// Spawna un nemico in una posizione fuori dalla telecamera.
    /// <param name="activePhase">La fase di difficoltà attiva per i parametri.</param>
    private void SpawnEnemy(DifficultyPhase activePhase)
    {
        // Rimuove eventuali riferimenti nulli da spawn precedenti
        currentlySpawnableEnemies.RemoveAll(item => item == null);
        // *** MODIFICATO: Usiamo 'currentlySpawnableEnemies' ***
        if (currentlySpawnableEnemies.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: Nessun nemico attualmente sbloccato per lo spawn.");
            return;
        }

        // Scegli un tipo di nemico casuale dalla lista dei nemici attualmente sbloccati
        GameObject enemyToSpawn = currentlySpawnableEnemies[Random.Range(0, currentlySpawnableEnemies.Count)];

        Vector2 spawnPosition = GetRandomSpawnPosition();

        // Loop per trovare una posizione valida se troppo vicina al player
        // (Considera un limite di tentativi in un gioco reale per evitare freeze)
        int attempts = 0;
        const int maxAttempts = 5; 
        while (Vector2.Distance(spawnPosition, playerTransform.position) < minSpawnDistanceFromPlayer && attempts < maxAttempts)
        {
            spawnPosition = GetRandomSpawnPosition(); 
            attempts++;
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("EnemySpawner: Impossibile trovare una posizione di spawn sufficientemente lontana dal giocatore dopo " + maxAttempts + " tentativi. Saltando spawn.");
            return; 
        }

        Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }

    /// Calcola una posizione casuale fuori dai bordi della telecamera.
    /// <returns>Una posizione Vector2 valida per lo spawn.</returns>
    private Vector2 GetRandomSpawnPosition()
    {
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector2 spawnPos = Vector2.zero;
        int side = Random.Range(0, 4); // 0: Top, 1: Bottom, 2: Left, 3: Right

        switch (side)
        {
            case 0: // Top
                spawnPos.x = Random.Range(mainCamera.transform.position.x - cameraWidth / 2f, mainCamera.transform.position.x + cameraWidth / 2f);
                spawnPos.y = mainCamera.transform.position.y + cameraHeight / 2f + spawnMargin;
                break;
            case 1: // Bottom
                spawnPos.x = Random.Range(mainCamera.transform.position.x - cameraWidth / 2f, mainCamera.transform.position.x + cameraWidth / 2f);
                spawnPos.y = mainCamera.transform.position.y - cameraHeight / 2f - spawnMargin;
                break;
            case 2: // Left
                spawnPos.x = mainCamera.transform.position.x - cameraWidth / 2f - spawnMargin;
                spawnPos.y = Random.Range(mainCamera.transform.position.y - cameraHeight / 2f, mainCamera.transform.position.y + cameraHeight / 2f);
                break;
            case 3: // Right
                spawnPos.x = mainCamera.transform.position.x + cameraWidth / 2f + spawnMargin;
                spawnPos.y = Random.Range(mainCamera.transform.position.y - cameraHeight / 2f, mainCamera.transform.position.y + cameraHeight / 2f);
                break;
        }
        return spawnPos;
    }

    /// Metodo chiamato quando un nemico viene distrutto (da chiamare dallo script Enemy).
    public void DecrementEnemyCount()
    {
        currentEnemyCount--;
        if (currentEnemyCount < 0) currentEnemyCount = 0; 
    }
}