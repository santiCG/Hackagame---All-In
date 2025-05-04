using UnityEngine;

public class windowCleaningScript : MonoBehaviour
{
    [Header("Referencias")]
    public Camera playerCamera;
    public Renderer windowRenderer;

    [Header("Textura de limpieza")]
    public int maskResolution = 512;
    public float brushSize = 10f;
    public Color cleanColor = Color.black; // Negro revela ventana limpia

    public Material material;
    public Texture2D dirtBrush;
    private Texture2D dirtMaskTexture;
    public Texture2D dirtMaskTextureBase;

    private void Start()
    {
        // Creamos la textura dinámica en tiempo de ejecución
        dirtMaskTexture = new Texture2D(dirtMaskTextureBase.width, dirtMaskTextureBase.height);
        dirtMaskTexture.SetPixels(dirtMaskTextureBase.GetPixels());
        dirtMaskTexture.Apply();
        material.SetTexture("WindowMask", dirtMaskTexture);
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 50))
        {
            if (hit.collider.gameObject == windowRenderer.gameObject)
            {
                Debug.Log("encontramos ventana");
                Vector2 uv = hit.textureCoord;
                int pixelX = (int)(uv.x * maskResolution);
                int pixelY = (int)(uv.y * maskResolution);

                PaintOnMask(pixelX, pixelY);
            }
        }
    }

    private void PaintOnMask(int pixelX, int pixelY)
    {
        int pixelOffsetX = pixelX - (dirtBrush.width / 2);
        int pixelOffsetY = pixelY - (dirtBrush.height / 2);

        for (int i = 0; i < dirtBrush.width; i++)
        {
            for (int j = 0; j < dirtBrush.height; j++)
            {
                Color pixelDirt = dirtBrush.GetPixel(i, j);
                Color pixelDirtyMask = dirtMaskTexture.GetPixel(pixelOffsetX + i, pixelOffsetY + j);

                dirtMaskTexture.SetPixel(
                    pixelOffsetX + i,
                    pixelOffsetY + j,
                    new Color(0, pixelDirtyMask.g * pixelDirt.g, 0));
            }
        }
    }
}
