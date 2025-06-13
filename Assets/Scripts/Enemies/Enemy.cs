using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LifeController))]
public abstract class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Transform playerTransform;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        playerTransform = GameObject.FindWithTag("Player")?.transform;
    }
    public virtual void Die()
    {
        Destroy(gameObject); // o comportamento base desiderato
    }
    protected abstract void FixedUpdate(); // ogni nemico definisce il proprio movimento
}
