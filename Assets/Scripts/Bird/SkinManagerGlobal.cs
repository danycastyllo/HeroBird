using System.Collections.Generic;
using UnityEngine;

public class SkinManagerGlobal : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────────────────────────
    public static SkinManagerGlobal Instance;

    // ── Datos ─────────────────────────────────────────────────────────────────
    [Header("Todos los personajes del juego")]
    public List<CharacterData> characters;

    // ── Privadas ──────────────────────────────────────────────────────────────
    SpriteRenderer spriteRenderer;
    Animator animator;
    Sprite[] activeSpritesheet;

    const string idleStateName = "Idle";

    // ─────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance       = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator       = GetComponent<Animator>();
    }

    void Start()
    {
        string savedName = PlayerPrefs.GetString(GameKeys.SelectedCharacter, "");
        ApplySkin(savedName);
    }

    void LateUpdate()
    {
        if (activeSpritesheet == null || animator == null) return;
        if (ControlBird.isDead) return;

        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        if (state.IsName(idleStateName))
        {
            spriteRenderer.sprite = activeSpritesheet[0];
            return;
        }

        int frame = Mathf.FloorToInt(state.normalizedTime % 1f * activeSpritesheet.Length);
        frame = Mathf.Clamp(frame, 0, activeSpritesheet.Length - 1);
        spriteRenderer.sprite = activeSpritesheet[frame];
    }

    // ── API pública ───────────────────────────────────────────────────────────
    public void ApplySkin(string skinName)
    {
        if (characters == null || characters.Count == 0)
        {
            Debug.LogWarning("SkinManagerGlobal: la lista de personajes está vacía.");
            return;
        }

        CharacterData found = null;

        if (!string.IsNullOrEmpty(skinName))
            found = characters.Find(c => c.itemName == skinName);

        if (found == null)
            found = characters[0];

        activeSpritesheet = found.spritesheet;
    }
}