using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    [SerializeField] List<GameObject> droppedObjects = new List<GameObject>();

    public void Drop (Vector2 position)
    {
        int item=Random.Range(0, droppedObjects.Count);
        int chance=Random.Range(0,101);
        
        if(chance <= 15)
        {
            Instantiate(droppedObjects[item], position, Quaternion.identity);
        }
    }
}
