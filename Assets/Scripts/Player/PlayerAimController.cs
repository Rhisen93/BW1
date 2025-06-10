using UnityEngine;

public class PlayerAimController : MonoBehaviour
{
    [SerializeField] private Transform playerFirePoint;     
    [SerializeField] private float rotationOffset = 0f;    
    [SerializeField] private float maxFirePointDistance = 2f;     
    private Transform playerTransform; 

    private void Awake()
    {
        playerTransform = transform; 

        if (playerFirePoint == null)
        {
            Debug.LogError("PlayerFirePoint non assegnato in PlayerAimController! Assicurati di trascinare il GameObject PlayerFirePoint nell'Inspector.");
        }
    }

    private void Update()
    {
        if (playerFirePoint == null || Camera.main == null)
        {            
            return;
        }
        
        Vector3 mouseScreenPosition = Input.mousePosition;
        
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(playerTransform.position).z; 
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);       
        Vector2 directionToMouse = (mouseWorldPosition - playerTransform.position).normalized;        
        Vector3 desiredFirePointPosition = (Vector2)playerTransform.position + directionToMouse * maxFirePointDistance;      
       
        playerFirePoint.position = desiredFirePointPosition;
        
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;
        
        angle += rotationOffset; 
        
        playerFirePoint.rotation = Quaternion.Euler(0f, 0f, angle);
        
    }
   
    public Transform GetPlayerFirePoint()
    {
        return playerFirePoint;
    }
}