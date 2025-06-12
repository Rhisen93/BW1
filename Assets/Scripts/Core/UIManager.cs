using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Necessario per TextMeshProUGUI

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText; // Riferimento al testo del punteggio
    [SerializeField]
    private TextMeshProUGUI gameOverText; // Riferimento al testo di Game Over

    private void Awake()
    {
        // Assicurati che il GameManager esista
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager non trovato! Assicurati che sia presente nella scena.");
            return;
        }
    }

    private void OnEnable()
    {
        GameManager.onScoreChanged += UpdateScoreDisplay; // Iscriviti all'evento di cambiamento del punteggio
    }

    private void OnDisable()
    {
        GameManager.onScoreChanged -= UpdateScoreDisplay; // Disiscriviti dall'evento di cambiamento del punteggio
    }

    private void UpdateScoreDisplay(int newScore)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score: {newScore}"; // Aggiorna il testo del punteggio
        }
    }

}
