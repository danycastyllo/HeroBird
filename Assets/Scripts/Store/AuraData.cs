using UnityEngine;

public enum AuraMovementType
{
    Trail,
    Burst,
    Smoke,
    Pulse,
    Orbit,
    Float,
    Glitch
}

public enum AuraColorMode
{
    Constant,           // un solo color
    Gradient,           // gradiente de inicio a fin
    RandomBetweenTwo    // elige aleatoriamente entre dos colores
}

[CreateAssetMenu(fileName = "NewAura", menuName = "HeroBird/Aura")]
public class AuraData : ShopItemData
{
    [Header("Visual")]
    public Sprite particleSprite;

    [Header("Color")]
    public AuraColorMode colorMode = AuraColorMode.Constant;
    public Color colorA = Color.white;          // color principal o color desde
    public Color colorB = Color.yellow;         // segundo color o color hasta
    public Gradient colorGradient;              // usado si colorMode = Gradient

    [Header("Tamaño")]
    public float startSize      = 0.2f;
    [Range(0f, 1f)]
    public float sizeVariation  = 0f;           // 0 = todos iguales, 1 = muy variado

    [Header("Movimiento")]
    public AuraMovementType movementType = AuraMovementType.Trail;
    [Range(0.1f, 3f)]
    public float intensity      = 1f;
    public float startSpeed     = 1f;
    [Range(0f, 1f)]
    public float speedVariation = 0.3f;         // variación aleatoria de velocidad

    [Header("Vida de partícula")]
    public float lifetime       = 0.5f;
    [Range(0f, 1f)]
    public float lifetimeVariation = 0.3f;      // variación aleatoria de lifetime

    [Header("Emisión")]
    public float emissionRate   = 20f;          // partículas por segundo
    public int   burstCount     = 10;           // partículas por burst (si aplica)

    [Header("Física")]
    [Range(-1f, 1f)]
    public float gravity        = 0f;           // negativo = sube, positivo = baja
    [Range(0f, 1f)]
    public float drag           = 0f;           // resistencia al movimiento

    [Header("Rotación")]
    public bool  randomRotation = false;        // rota cada partícula aleatoriamente
    [Range(0f, 360f)]
    public float rotationSpeed  = 0f;           // velocidad de rotación en vida

    [Header("Noise / Turbulencia")]
    public bool  useNoise       = true;
    [Range(0f, 2f)]
    public float noiseStrength  = 0.2f;
    [Range(0f, 5f)]
    public float noiseFrequency = 1f;

    [Header("Fade")]
    public bool  fadeOut        = true;         // las partículas se desvanecen al morir
    public bool  fadeIn         = false;        // las partículas aparecen gradualmente

    [Header("Tamaño sobre vida")]
    public bool  shrinkOverLife = true;         // se encogen al morir
    public bool  growOverLife   = false;        // crecen durante su vida
}
