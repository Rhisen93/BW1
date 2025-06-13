using System.Collections;
using UnityEngine;

public class VisualEffectHandler : MonoBehaviour
{
    public void FlashSprite(Color flashColor, float duration)
    {
        StartCoroutine(FlashColorCoroutine(flashColor, duration));
    }

    private IEnumerator FlashColorCoroutine(Color flashColor, float duration)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        Color originalColor = sr.color;
        sr.color = flashColor;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
    }
}

