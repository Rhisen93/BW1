using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] List<GameObject> droppedObjects = new List<GameObject>();
    [SerializeField] int chance=15;
    public void Drop (Vector2 position)
    {
        int item=Random.Range(0, droppedObjects.Count);
        int roll=Random.Range(0,100);
        
        if (roll <= chance)
        {
            Instantiate(droppedObjects[item], position, Quaternion.identity);
        }
    }
}
