using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton pattern

    private int score = 0;
    public int Score => score;

    // Evento che puoi sottoscrivere da UI o altri script
    public delegate void OnScoreChanged(int newScore);
    public static event OnScoreChanged onScoreChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Per mantenere il GameManager tra le scene
        }
    }

    // GameManager.Instance.AddScore(scoreValue);
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"Score: {score}");
        onScoreChanged?.Invoke(score); // Notifica del cambiamento del punteggio
    }
}

