// Scripts/UI/ViewportAnchor.cs
using UnityEngine;

/// <summary>
/// Posiciona el GameObject relativo a los bordes de la cámara.
/// viewportX/Y: 0 = izquierda/abajo, 1 = derecha/arriba, 0.5 = centro.
/// offsetX/Y: ajuste fino en world units una vez anclado.
/// </summary>
public class ViewportAnchor : MonoBehaviour
{
    [Header("Punto de anclaje (0=borde izq/inf — 1=borde der/sup)")]
    [Range(0f, 1f)] public float viewportX = 0f;
    [Range(0f, 1f)] public float viewportY = 1f;

    [Header("Ajuste fino en world units")]
    public float offsetX;
    public float offsetY;

    void Awake() => Apply();

    void Apply()
    {
        var cam   = Camera.main;
        float depth = Mathf.Abs(cam.transform.position.z - transform.position.z);

        Vector3 world = cam.ViewportToWorldPoint(
            new Vector3(viewportX, viewportY, depth)
        );

        transform.position = new Vector3(
            world.x + offsetX,
            world.y + offsetY,
            transform.position.z   // respeta el Z original
        );
    }
}