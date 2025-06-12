using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField]float heal = 10;
    LifeController lifeController;


    private void OnTriggerEnter2D(Collider2D collider)
    {

        if (!collider.CompareTag("Player")) return;
        lifeController = collider.GetComponent<LifeController>();
        lifeController.AddHealth(heal);
        Destroy(gameObject);
    }
}