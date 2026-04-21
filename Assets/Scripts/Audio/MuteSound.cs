using UnityEngine;

public class SpriteButton : MonoBehaviour
{
    public Sprite muteIcon;
    public Sprite unmuteIcon;
    public SpriteRenderer spriteRenderer;

    bool isMuted;

    void Start()
    {
        isMuted = PlayerPrefs.GetInt(GameKeys.Sound) == 0;
        UpdateIcon();
    }

    void OnMouseDown()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        PlayerPrefs.SetInt(GameKeys.Sound, isMuted ? 0 : 1);
        UpdateIcon();
    }

    void UpdateIcon()
    {
        spriteRenderer.sprite = isMuted ? muteIcon : unmuteIcon;
    }
}