// ============================================================
//  AuraDataGenerator.cs
//  Coloca este archivo en cualquier carpeta llamada Editor/
//  dentro de tu proyecto Unity.
//
//  Menú: HeroBird > Generate All Auras
// ============================================================

using UnityEngine;
using UnityEditor;
using System.IO;

public static class AuraDataGenerator
{
    private const string SAVE_PATH = "Assets/Data/Auras";

    // ─────────────────────────────────────────────────────────
    //  Entry point
    // ─────────────────────────────────────────────────────────
    [MenuItem("HeroBird/Generate All Auras")]
    public static void GenerateAll()
    {
        EnsureDirectory(SAVE_PATH);

        CreateAura(BuildFireTrail());
        CreateAura(BuildIceShards());
        CreateAura(BuildThunderDash());
        CreateAura(BuildNatureSpirit());
        CreateAura(BuildVoidCorruption());
        CreateAura(BuildDeathHarbinger());
        CreateAura(BuildToxicMiasma());
        CreateAura(BuildBloodCurse());
        CreateAura(BuildDivineBlessing());
        CreateAura(BuildPhantomVeil());
        CreateAura(BuildSakuraDancer());
        CreateAura(BuildGoldRush());
        CreateAura(BuildIceBurst());
        CreateAura(BuildNeonStar());
        CreateAura(BuildShadowPulse());

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[AuraDataGenerator] ✅ Todas las auras generadas en {SAVE_PATH}");
        EditorUtility.FocusProjectWindow();
    }

    // ─────────────────────────────────────────────────────────
    //  Helpers
    // ─────────────────────────────────────────────────────────
    private static void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    /// Crea el asset solo si no existe ya uno con ese fileName.
    /// Si existe, actualiza sus valores y lo marca dirty.
    /// </summary>
    private static void CreateAura(AuraData data)
    {
        string assetPath = $"{SAVE_PATH}/{data.name}.asset";
        AuraData existing = AssetDatabase.LoadAssetAtPath<AuraData>(assetPath);

        if (existing != null)
        {
            // Sobreescribir valores sobre el asset existente
            EditorUtility.CopySerialized(data, existing);
            EditorUtility.SetDirty(existing);
            Debug.Log($"[AuraDataGenerator] ♻️  Actualizado: {data.name}");
        }
        else
        {
            AssetDatabase.CreateAsset(data, assetPath);
            Debug.Log($"[AuraDataGenerator] ✨ Creado: {data.name}");
        }
    }

    /// <summary>
    /// Construye un Gradient de dos colores sólidos con alpha a 0 al final.
    /// </summary>
    private static Gradient MakeGradient(Color from, Color to, bool fadeEnd = true)
    {
        var g = new Gradient();
        g.colorKeys = new[]
        {
            new GradientColorKey(from, 0f),
            new GradientColorKey(to,   1f)
        };
        g.alphaKeys = new[]
        {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(fadeEnd ? 0f : 1f, 1f)
        };
        return g;
    }

    /// <summary>
    /// Gradient de tres colores con alpha a 0 al final.
    /// </summary>
    private static Gradient MakeGradient3(Color a, Color b, Color c)
    {
        var g = new Gradient();
        g.colorKeys = new[]
        {
            new GradientColorKey(a, 0f),
            new GradientColorKey(b, 0.5f),
            new GradientColorKey(c, 1f)
        };
        g.alphaKeys = new[]
        {
            new GradientAlphaKey(1f, 0f),
            new GradientAlphaKey(0.6f, 0.5f),
            new GradientAlphaKey(0f,  1f)
        };
        return g;
    }

    private static AuraData Base(string fileName)
    {
        var d = ScriptableObject.CreateInstance<AuraData>();
        d.name = fileName;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  00 — FIRE TRAIL  (ejemplo de referencia)
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildFireTrail()
    {
        var d = Base("FireTrail");
        // Color
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient3(
            new Color(1f, 0.92f, 0.016f),   // amarillo
            new Color(1f, 0.45f, 0f),        // naranja
            new Color(1f, 0.2f, 0f, 0f)      // transparente
        );
        // Tamaño
        d.startSize      = 0.3f;
        d.sizeVariation  = 0.4f;
        // Movimiento
        d.movementType   = AuraMovementType.Trail;
        d.intensity      = 1.2f;
        d.startSpeed     = 0.5f;
        d.speedVariation = 0.5f;
        // Vida
        d.lifetime       = 0.5f;
        d.lifetimeVariation = 0.3f;
        // Emisión
        d.emissionRate   = 30f;
        // Física
        d.gravity        = -0.1f;
        // Noise
        d.useNoise       = true;
        d.noiseStrength  = 0.25f;
        d.noiseFrequency = 2f;
        // Fade / Size
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  01 — ICE SHARDS
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildIceShards()
    {
        var d = Base("IceShards");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.878f, 0.969f, 1f),
            new Color(0.49f,  0.827f, 0.98f)
        );
        d.startSize      = 0.15f;
        d.sizeVariation  = 0.5f;
        d.movementType   = AuraMovementType.Orbit;
        d.intensity      = 0.8f;
        d.startSpeed     = 0.2f;
        d.speedVariation = 0.3f;
        d.lifetime       = 1.2f;
        d.lifetimeVariation = 0.4f;
        d.emissionRate   = 20f;
        d.gravity        = 0f;
        d.useNoise       = false;
        d.fadeOut        = true;
        d.shrinkOverLife = false;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  02 — THUNDER DASH
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildThunderDash()
    {
        var d = Base("ThunderDash");
        d.colorMode      = AuraColorMode.RandomBetweenTwo;
        d.colorA         = new Color(0.98f, 0.8f, 0.04f);   // amarillo eléctrico
        d.colorB         = Color.white;
        d.startSize      = 0.08f;
        d.sizeVariation  = 0.2f;
        d.movementType   = AuraMovementType.Trail;
        d.intensity      = 2.0f;
        d.startSpeed     = 1.5f;
        d.speedVariation = 0.8f;
        d.lifetime       = 0.15f;
        d.lifetimeVariation = 0.1f;
        d.emissionRate   = 80f;
        d.gravity        = 0f;
        d.useNoise       = true;
        d.noiseStrength  = 0.6f;
        d.noiseFrequency = 8f;
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = false;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  03 — NATURE SPIRIT
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildNatureSpirit()
    {
        var d = Base("NatureSpirit");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.529f, 0.937f, 0.635f),
            new Color(0.086f, 0.635f, 0.29f)
        );
        d.startSize      = 0.2f;
        d.sizeVariation  = 0.6f;
        d.movementType   = AuraMovementType.Float;   // Drift → Float
        d.intensity      = 0.6f;
        d.startSpeed     = 0.3f;
        d.speedVariation = 0.4f;
        d.lifetime       = 2.0f;
        d.lifetimeVariation = 0.8f;
        d.emissionRate   = 12f;
        d.gravity        = -0.05f;
        d.useNoise       = true;
        d.noiseStrength  = 0.4f;
        d.noiseFrequency = 1f;
        d.fadeOut        = true;
        d.shrinkOverLife = false;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  04 — VOID CORRUPTION
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildVoidCorruption()
    {
        var d = Base("VoidCorruption");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.663f, 0.333f, 0.969f),
            new Color(0.231f, 0.043f, 0.392f)
        );
        d.startSize      = 0.35f;
        d.sizeVariation  = 0.5f;
        d.movementType   = AuraMovementType.Pulse;   // Implode → Pulse
        d.intensity      = 1.5f;
        d.startSpeed     = 0.4f;
        d.speedVariation = 0.3f;
        d.lifetime       = 0.8f;
        d.lifetimeVariation = 0.3f;
        d.emissionRate   = 25f;
        d.gravity        = 0.05f;
        d.useNoise       = true;
        d.noiseStrength  = 0.5f;
        d.noiseFrequency = 3f;
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = false;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  05 — DEATH HARBINGER
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildDeathHarbinger()
    {
        var d = Base("DeathHarbinger");
        d.colorMode      = AuraColorMode.RandomBetweenTwo;
        d.colorA         = new Color(0.42f, 0.447f, 0.502f);  // gris oscuro
        d.colorB         = new Color(0.82f, 0.835f, 0.855f);  // gris hueso
        d.startSize      = 0.25f;
        d.sizeVariation  = 0.3f;
        d.movementType   = AuraMovementType.Orbit;
        d.intensity      = 1.0f;
        d.startSpeed     = 0.35f;
        d.speedVariation = 0.2f;
        d.lifetime       = 3.0f;
        d.lifetimeVariation = 1.0f;
        d.emissionRate   = 5f;
        d.gravity        = 0f;
        d.useNoise       = false;
        d.fadeOut        = false;
        d.shrinkOverLife = false;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  06 — TOXIC MIASMA
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildToxicMiasma()
    {
        var d = Base("ToxicMiasma");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.745f, 0.949f, 0.392f),
            new Color(0.396f, 0.639f, 0.051f)
        );
        d.startSize      = 0.4f;
        d.sizeVariation  = 0.7f;
        d.movementType   = AuraMovementType.Float;
        d.intensity      = 0.9f;
        d.startSpeed     = 0.15f;
        d.speedVariation = 0.2f;
        d.lifetime       = 1.8f;
        d.lifetimeVariation = 0.6f;
        d.emissionRate   = 18f;
        d.gravity        = -0.08f;
        d.useNoise       = true;
        d.noiseStrength  = 0.3f;
        d.noiseFrequency = 1.5f;
        d.fadeOut        = true;
        d.shrinkOverLife = false;
        d.randomRotation = false;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  07 — BLOOD CURSE
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildBloodCurse()
    {
        var d = Base("BloodCurse");
        d.colorMode      = AuraColorMode.RandomBetweenTwo;
        d.colorA         = new Color(0.6f, 0.106f, 0.106f);   // rojo sangre
        d.colorB         = new Color(0.271f, 0.039f, 0.039f); // rojo oscuro
        d.startSize      = 0.12f;
        d.sizeVariation  = 0.5f;
        d.movementType   = AuraMovementType.Trail;
        d.intensity      = 1.3f;
        d.startSpeed     = 0.6f;
        d.speedVariation = 0.4f;
        d.lifetime       = 0.6f;
        d.lifetimeVariation = 0.2f;
        d.emissionRate   = 35f;
        d.gravity        = 0.3f;
        d.useNoise       = false;
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = false;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  08 — DIVINE BLESSING
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildDivineBlessing()
    {
        var d = Base("DivineBlessing");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(1f, 0.973f, 0.541f),
            new Color(0.984f, 0.749f, 0.141f)
        );
        d.startSize      = 0.18f;
        d.sizeVariation  = 0.6f;
        d.movementType   = AuraMovementType.Burst;
        d.intensity      = 1.0f;
        d.startSpeed     = 0.4f;
        d.speedVariation = 0.5f;
        d.lifetime       = 1.5f;
        d.lifetimeVariation = 0.5f;
        d.emissionRate   = 22f;
        d.gravity        = -0.06f;
        d.useNoise       = true;
        d.noiseStrength  = 0.15f;
        d.noiseFrequency = 1f;
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  09 — PHANTOM VEIL
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildPhantomVeil()
    {
        var d = Base("PhantomVeil");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.98f, 0.98f, 1f, 0.9f),
            new Color(0.49f, 0.827f, 0.98f)
        );
        d.startSize      = 0.5f;
        d.sizeVariation  = 0.8f;
        d.movementType   = AuraMovementType.Trail;
        d.intensity      = 0.7f;
        d.startSpeed     = 0.1f;
        d.speedVariation = 0.15f;
        d.lifetime       = 1.0f;
        d.lifetimeVariation = 0.4f;
        d.emissionRate   = 40f;
        d.gravity        = 0f;
        d.useNoise       = true;
        d.noiseStrength  = 0.2f;
        d.noiseFrequency = 0.8f;
        d.fadeOut        = true;
        d.shrinkOverLife = false;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  10 — SAKURA DANCER
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildSakuraDancer()
    {
        var d = Base("SakuraDancer");
        d.colorMode      = AuraColorMode.RandomBetweenTwo;
        d.colorA         = new Color(0.992f, 0.643f, 0.686f); // rosa coral
        d.colorB         = new Color(0.984f, 0.812f, 0.914f); // rosa pálido
        d.startSize      = 0.22f;
        d.sizeVariation  = 0.5f;
        d.movementType   = AuraMovementType.Float;   // Drift → Float
        d.intensity      = 0.7f;
        d.startSpeed     = 0.25f;
        d.speedVariation = 0.35f;
        d.lifetime       = 2.5f;
        d.lifetimeVariation = 1.0f;
        d.emissionRate   = 10f;
        d.gravity        = 0.04f;
        d.useNoise       = true;
        d.noiseStrength  = 0.35f;
        d.noiseFrequency = 0.6f;
        d.fadeOut        = true;
        d.shrinkOverLife = false;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  11 — GOLD RUSH
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildGoldRush()
    {
        var d = Base("GoldRush");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.996f, 0.941f, 0.541f),
            new Color(0.851f, 0.459f, 0.024f)
        );
        d.startSize      = 0.28f;
        d.sizeVariation  = 0.45f;
        d.movementType   = AuraMovementType.Trail;
        d.intensity      = 1.4f;
        d.startSpeed     = 0.55f;
        d.speedVariation = 0.45f;
        d.lifetime       = 0.7f;
        d.lifetimeVariation = 0.3f;
        d.emissionRate   = 28f;
        d.gravity        = 0.18f;
        d.useNoise       = true;
        d.noiseStrength  = 0.2f;
        d.noiseFrequency = 3f;
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  12 — ICE BURST
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildIceBurst()
    {
        var d = Base("IceBurst");
        d.colorMode      = AuraColorMode.RandomBetweenTwo;
        d.colorA         = new Color(0.729f, 0.902f, 0.992f); // azul hielo
        d.colorB         = new Color(0.941f, 0.976f, 1f);     // blanco nieve
        d.startSize      = 0.35f;
        d.sizeVariation  = 0.6f;
        d.movementType   = AuraMovementType.Burst;
        d.intensity      = 2.2f;
        d.startSpeed     = 1.8f;
        d.speedVariation = 0.7f;
        d.lifetime       = 0.4f;
        d.lifetimeVariation = 0.15f;
        d.emissionRate   = 60f;
        d.burstCount     = 30;
        d.gravity        = 0f;
        d.useNoise       = false;
        d.fadeOut        = true;
        d.shrinkOverLife = false;
        d.randomRotation = false;   // copos conservan su simetría
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  13 — NEON STAR
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildNeonStar()
    {
        var d = Base("NeonStar");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient3(
            new Color(0.941f, 0.671f, 0.988f),  // rosa neón
            new Color(0.91f,  0.475f, 0.976f),  // magenta
            new Color(0.635f, 0.11f,  0.69f)    // violeta
        );
        d.startSize      = 0.2f;
        d.sizeVariation  = 0.7f;
        d.movementType   = AuraMovementType.Orbit;
        d.intensity      = 1.6f;
        d.startSpeed     = 0.8f;
        d.speedVariation = 0.3f;
        d.lifetime       = 0.9f;
        d.lifetimeVariation = 0.4f;
        d.emissionRate   = 45f;
        d.gravity        = -0.12f;
        d.useNoise       = true;
        d.noiseStrength  = 0.45f;
        d.noiseFrequency = 4f;
        d.fadeOut        = true;
        d.shrinkOverLife = true;
        d.randomRotation = true;
        return d;
    }

    // ─────────────────────────────────────────────────────────
    //  14 — SHADOW PULSE
    // ─────────────────────────────────────────────────────────
    private static AuraData BuildShadowPulse()
    {
        var d = Base("ShadowPulse");
        d.colorMode      = AuraColorMode.Gradient;
        d.colorGradient  = MakeGradient(
            new Color(0.11f, 0.098f, 0.09f),
            new Color(0.267f, 0.251f, 0.235f)
        );
        d.startSize      = 0.6f;
        d.sizeVariation  = 0.9f;
        d.movementType   = AuraMovementType.Pulse;
        d.intensity      = 1.8f;
        d.startSpeed     = 0.05f;  // casi estáticas, emanan desde el cuerpo
        d.speedVariation = 0.1f;
        d.lifetime       = 1.4f;
        d.lifetimeVariation = 0.5f;
        d.emissionRate   = 50f;
        d.gravity        = 0f;
        d.useNoise       = true;
        d.noiseStrength  = 0.55f;
        d.noiseFrequency = 2.5f;
        d.fadeOut        = true;
        d.shrinkOverLife = false;  // mantienen volumen hasta desvanecerse
        d.randomRotation = true;
        return d;
    }
}
