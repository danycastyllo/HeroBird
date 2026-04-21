public static class GameKeys
{
    // Personaje y aura seleccionados
    public const string SelectedCharacter = "SelectedCharacterName";
    public const string SelectedAura      = "SelectedAuraName";

    // Progreso del jugador
    public const string PlayerCoins       = "PlayerCoins";
    public const string Record            = "record";

    // Audio
    public const string Sound             = "Sound";

    // Claves dinámicas — usar con string.Format o interpolación
    // Uso: string.Format(GameKeys.CharacterUnlocked, itemName)
    public const string CharacterUnlocked = "Character_{0}_Unlocked";
    public const string AuraUnlocked      = "Aura_{0}_Unlocked";
}