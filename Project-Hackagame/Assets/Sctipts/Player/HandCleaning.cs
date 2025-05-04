using UnityEngine;

public class HandCleaning : MonoBehaviour
{
    private HandMovement handMovement;

    void Start()
    {
        handMovement = GetComponent<HandMovement>();    
    }

    void Update()
    {
        if( handMovement != null && handMovement.PlayerInteracting) 
        { 
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
            {
                Vector2 textureCord = hit.textureCoord;

                int pixelX = (int)(textureCord.x * 512);
                int pixelY = (int)(textureCord.y * 512);

                Vector2Int paintPixelPos = new Vector2Int(pixelX, pixelY);
            }
        }        
    }
}
