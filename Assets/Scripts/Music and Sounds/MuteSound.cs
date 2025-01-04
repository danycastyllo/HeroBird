using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    public Sprite muteIcon; // Sprite para el estado Mute
    public Sprite unmuteIcon; // Sprite para el estado Unmute

    public SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private bool isMuted = false; // Estado actual del audio

    private void Start()
    {
        isMuted = PlayerPrefs.GetInt ("Sound") == 0 ? true : false; //devuelve el playerprefs del sonido para saber que icono poner
        UpdateIcon(); // Asegúrate de que el sprite inicial sea correcto
    }

    private void OnMouseDown()
    {
        // Alterna el estado del audio al hacer clic
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        PlayerPrefs.SetInt ("Sound", isMuted ? 0 : 1); //guarda la variable de mute en un playerprefs
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        // Cambia el sprite según el estado actual
        spriteRenderer.sprite = isMuted ? muteIcon : unmuteIcon; // actuliza el icono dependiendo si esta mute
    }
}
