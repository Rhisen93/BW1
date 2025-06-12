using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Quanto tempo il numero di danno rimane visibile.")]
    [SerializeField]
    private float lifetime = 1f;
    [Tooltip("Velocità con cui il numero si muove verso l'alto.")]
    [SerializeField]
    private float moveSpeed = 1f;
    [Tooltip("Velocità con cui il numero svanisce.")]
    [SerializeField]
    private float fadeSpeed = 1f;
    [Tooltip("Altezza massima alla quale può arrivare il pop-up.")]
    [SerializeField]
    private float maxHeight = 2f; // Per evitare che vada troppo in alto

    private TextMeshProUGUI damageText;
    private Color startColor;
    private Vector3 initialPosition;
    private float timer;

    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
        if (damageText == null)
        {
            Debug.LogError("DamagePopUp: TextMeshProUGUI non trovato sul GameObject.");
            enabled = false;
        }
        startColor = damageText.color;
        initialPosition = transform.position;
        timer = lifetime;
    }

    private void Update()
    {
        // Movimento verso l'alto
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // Dissolvenza del colore
        timer -= Time.deltaTime;
        float alpha = Mathf.Clamp01(timer / (lifetime / fadeSpeed)); // Controlla la velocità di dissolvenza
        damageText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

        // Distruzione del pop-up quando il tempo è scaduto o ha raggiunto l'altezza massima
        if (timer <= 0f || transform.position.y > initialPosition.y + maxHeight)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Imposta il valore del danno e il colore del testo.
    /// </summary>
    /// <param name="damageAmount">La quantità di danno da mostrare.</param>
    /// <param name="color">Colore opzionale per il testo (es. rosso per danno, verde per cura).</param>
    public void SetDamageAmount(int damageAmount, Color? color = null)
    {
        if (damageText != null)
        {
            damageText.text = damageAmount.ToString();
            if (color.HasValue)
            {
                damageText.color = color.Value;
                startColor = color.Value; // Aggiorna il colore iniziale per la dissolvenza
            }
        }
    }
}