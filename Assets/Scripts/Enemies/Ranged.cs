using UnityEngine;

public class RangedEnemy : Enemy
{
    public EnemyWeapon weapon;
    public float shootCooldown = 2f;
    public float preferredDistance = 5f;
    public float moveSpeed = 3f;

    private float shootTimer = 0f;

    protected override void FixedUpdate()
    {
        //if (playerTransform == null || weapon == null) return;

        float distance = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 direction = (playerTransform.position - transform.position).normalized;

        // Movimento per mantenere la distanza preferita
        if (distance > preferredDistance)
        {
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        else if (distance < preferredDistance - 1f)
        {
            rb.MovePosition(rb.position - direction * moveSpeed * Time.fixedDeltaTime);
        }



        shootTimer -= Time.fixedDeltaTime;

        
        if (shootTimer <= 0f)
        {
            if (weapon != null)
            {
                weapon.transform.position = transform.position;
            }


            weapon.SetShootDirection(direction);

            weapon.Shoot();
            shootTimer = shootCooldown;
        }

    }
}

