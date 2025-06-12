using UnityEngine;
using System.Collections;

public class IceEffect : MonoBehaviour
{
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float duration = 2f;
    private bool isActive = false;
    private float timer = 0f;

    public void Initialize(float multiplier, float effectDuration)
    {
        if (isActive) return;

        slowMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);
        duration = effectDuration;

        isActive = true;
        timer = 0f;
    }

    private void FixedUpdate()
    {
        if (!isActive) return;

        timer += Time.fixedDeltaTime;
        if (timer >= duration)
        {
            Destroy(this);
            return;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Transform player = GameObject.FindWithTag("Player")?.transform;
        if (rb == null || player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * slowMultiplier * Time.fixedDeltaTime);
    }
}
