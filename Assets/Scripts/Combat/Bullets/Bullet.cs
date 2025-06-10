using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float damage = 10f;
    [SerializeField]
    private float lifeTime = 3f;

    private Vector2 direction;

    
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);  
    }

    private void Update()
    {        
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Enemy")) 
        {                     
            other.GetComponent<LifeController>().TakeDamage(damage);            
            Destroy(gameObject);
        }
        
        else if (other.CompareTag("Obstacle")) 
        {                     
            Destroy(gameObject);
        }
        
    }
}