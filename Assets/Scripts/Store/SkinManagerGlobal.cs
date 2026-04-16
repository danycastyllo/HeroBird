using System.Collections.Generic;
using UnityEngine;

public class SkinManagerGlobal : MonoBehaviour
{
    public static SkinManagerGlobal Instance;

    [Header("Todos los personajes del juego")]
    public List<CharacterData> characters;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Sprite[] activeSpritesheet;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        string selectedName = PlayerPrefs.GetString(GameKeys.SelectedCharacter, "");
        ApplySkin(selectedName);
    }

    private void LateUpdate()
    {
        if (activeSpritesheet == null || animator == null) return;
        
        // ✅ Deja de sobreescribir cuando el pájaro muere
        if (ControlBird.isDead) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName("idle")) // reemplaza por tu nombre exacto de idle
        {
            spriteRenderer.sprite = activeSpritesheet[0];
            return;
        }

        int frame = Mathf.FloorToInt(state.normalizedTime % 1f * activeSpritesheet.Length);
        frame = Mathf.Clamp(frame, 0, activeSpritesheet.Length - 1);
        spriteRenderer.sprite = activeSpritesheet[frame];
    }
    public void ApplySkin(string skinName)
    {
        CharacterData found = null;

        if (!string.IsNullOrEmpty(skinName))
            found = characters.Find(c => c.itemName == skinName);

        // Si no hay selección guardada, usa el primero sin warning
        if (found == null)
            found = characters[0];

        activeSpritesheet = found.spritesheet;
    }
}