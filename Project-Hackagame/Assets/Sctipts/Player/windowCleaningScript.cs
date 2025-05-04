using UnityEngine;

public class windowCleaningScript : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Renderer windowRenderer;

    [Header("Cleaning Texture")]
    public float brushSize = 10f;
    public Color cleanColor = Color.black; // Black reveals the clean window

    public Material material;
    public Texture2D dirtBrush;
    private Texture2D dirtMaskTexture;
    public Texture2D dirtMaskTextureBase;

    private void Start()
    {
        // Create a dynamic texture at runtime
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height, TextureFormat.RGBA32, false);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("_WindowMask", dirtMaskTexture);
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == windowRenderer.gameObject)
            {
                // Convert UV coordinates to pixel coordinates
                Vector2 uv = hit.textureCoord;
                Debug.Log($"UV: {uv}");

                // Flip the Y-axis because UV coordinates are bottom-left origin, but textures are top-left origin
                int pixelX = Mathf.FloorToInt(uv.x * dirtMaskTexture.width);
                int pixelY = Mathf.FloorToInt(uv.y * dirtMaskTexture.height);

                Debug.Log($"PixelX: {pixelX}, PixelY: {pixelY}");

                PaintOnMask(pixelX, pixelY);
            }
        }
    }

    private void PaintOnMask(int pixelX, int pixelY)
    {
        // Define the size of the rectangle to clean
        int rectHalfWidth = Mathf.RoundToInt(brushSize / 2);
        int rectHalfHeight = Mathf.RoundToInt(brushSize / 2);

        // Loop through the rectangle area
        for (int x = -rectHalfWidth; x <= rectHalfWidth; x++)
        {
            for (int y = -rectHalfHeight; y <= rectHalfHeight; y++)
            {
                int targetX = pixelX + x;
                int targetY = pixelY + y;

                // Ensure we don't paint outside the bounds of the mask texture
                if (targetX < 0 || targetX >= dirtMaskTexture.width || targetY < 0 || targetY >= dirtMaskTexture.height)
                    continue;

                // Set the pixel to the clean color
                dirtMaskTexture.SetPixel(targetX, targetY, cleanColor);
            }
        }

        // Apply the changes to the texture
        dirtMaskTexture.Apply();
    }
}