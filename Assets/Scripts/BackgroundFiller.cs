// Scripts/World/BackgroundFiller.cs
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFiller : MonoBehaviour
{
    void Awake() => Fit();

    void Fit()
    {
        var sr  = GetComponent<SpriteRenderer>();
        var cam = Camera.main;

        float camH = cam.orthographicSize * 2f;
        float camW = camH * cam.aspect;

        // Tamaño natural del sprite, ignorando escala actual
        float sprW = sr.sprite.bounds.size.x;
        float sprH = sr.sprite.bounds.size.y;

        // Mathf.Max = modo "cover": nunca bandas negras, puede recortar si proporciones
        // son muy distintas. Si prefieres deformar usa scaleX/scaleY separados.
        float scale = Mathf.Max(camW / sprW, camH / sprH);
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}