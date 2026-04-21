using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused = false;
    public Sprite pausedIcon;
    public Sprite playIcon;
    public SpriteRenderer spriteRenderer;

    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
        
        spriteRenderer.sprite = isPaused ? playIcon : pausedIcon;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    void OnMouseDown() => TogglePause();
}