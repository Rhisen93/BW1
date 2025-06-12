using UnityEngine;
using System.Collections;

public class PoisonEffect : MonoBehaviour
{
    [SerializeField] private int damagePerSecond = 1;
    [SerializeField] private float duration = 3f;
    private bool isActive = false;

    public void Initialize(int dps, float effectDuration)
    {
        if (isActive) return;

        damagePerSecond = dps;
        duration = effectDuration;
        StartCoroutine(ApplyPoison());
    }

    private IEnumerator ApplyPoison()
    {
        isActive = true;

        float timer = 0f;
        LifeController life = GetComponent<LifeController>();

        while (timer < duration && life != null)
        {
            life.TakeDamage(damagePerSecond);
            yield return new WaitForSeconds(1f);
            timer += 1f;
        }

        Destroy(this);
    }
}
