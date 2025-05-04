using UnityEngine;

public class HandCleaning : MonoBehaviour
{
    [Header("Referencias")]
    public Camera playerCamera;
    public Renderer windowRenderer;
    public HandMovement handInteraction;
    public string maskPropertyName = "_MaskTexture";

    [Header("Textura de limpieza")]
    public int maskResolution = 512;
    public float brushSize = 10f;
    public Color cleanColor = Color.black; // Negro revela ventana limpia

    public Texture2D dirtBrush;
    public Texture2D dynamicMask;
    private Material windowMaterial;

    private void Start()
    {
        // Creamos la textura dinámica en tiempo de ejecución
        dynamicMask = new Texture2D(maskResolution, maskResolution, TextureFormat.RGBA32, false);
        ClearMask();
        dynamicMask.Apply();

        //// Instanciamos el material para no modificar el original
        windowMaterial = windowRenderer.material;
        windowMaterial.SetTexture(maskPropertyName, dynamicMask);
    }

    private void Update()
    {
        if (!handInteraction.PlayerInteracting) return;
        if (!Input.GetMouseButton(0)) return;

        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.gameObject == windowRenderer.gameObject)
            {
                Vector2 uv = hit.textureCoord;
                int pixelX = (int)(uv.x * maskResolution);
                int pixelY = (int)(uv.y * maskResolution);

                PaintOnMask(pixelX, pixelY);
            }
        }
    }

    private void PaintOnMask(int x, int y)
    {
        //int radius = Mathf.RoundToInt(brushSize / 2f);

        //for (int i = -radius; i <= radius; i++)
        //{
        //    for (int j = -radius; j <= radius; j++)
        //    {
        //        int px = x + i;
        //        int py = y + j;

        //        if (px >= 0 && px < maskResolution && py >= 0 && py < maskResolution)
        //        {
        //            float distance = Vector2.Distance(new Vector2(x, y), new Vector2(px, py));
        //            if (distance <= radius)
        //            {
        //                dynamicMask.SetPixel(px, py, cleanColor);
        //            }
        //        }
        //    }
        //}

        //dynamicMask.Apply();

        int pixelOffsetX = x - (dirtBrush.width / 2);
        int pixelOffsetY = y - (dirtBrush.height / 2);

        for (int i = 0; i < dirtBrush.width; i++)
        {
            for (int j = 0; j < dirtBrush.height; j++)
            {
                Color pixelDirt = dirtBrush.GetPixel(i, j);
                Color pixelDirtyMask = dynamicMask.GetPixel(pixelOffsetX + i, pixelOffsetY + j);

                dynamicMask.SetPixel(
                    pixelOffsetX + i,
                    pixelOffsetY + j,
                    new Color(0, pixelDirtyMask.g * pixelDirt.g, 0));
            }
        }
    }

    private void ClearMask()
    {
        Color fill = Color.green; // Verde: la suciedad
        for (int y = 0; y < maskResolution; y++)
        {
            for (int x = 0; x < maskResolution; x++)
            {
                dynamicMask.SetPixel(x, y, fill);
            }
        }
    }
}
