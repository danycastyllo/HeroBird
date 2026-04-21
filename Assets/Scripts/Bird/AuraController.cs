using UnityEngine;

public class AuraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem auraParticles;

    [Header("Sorting")]
    [SerializeField] private string sortingLayerName = "Default";
    [SerializeField] private int sortingOrder = 5;

    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emission;
    private ParticleSystem.ShapeModule shape;
    private ParticleSystem.VelocityOverLifetimeModule velocity;
    private ParticleSystem.SizeOverLifetimeModule sizeOverLifetime;
    private ParticleSystem.ColorOverLifetimeModule colorOverLifetime;
    private ParticleSystem.NoiseModule noise;
    private ParticleSystem.RotationOverLifetimeModule rotation;
    private ParticleSystemRenderer psRenderer;
    private MaterialPropertyBlock mpb;
    private ParticleSystem.LimitVelocityOverLifetimeModule limitVelocity;

    private void Awake()
    {
        
        if (auraParticles == null)
            auraParticles = GetComponentInChildren<ParticleSystem>();

        main              = auraParticles.main;
        emission          = auraParticles.emission;
        shape             = auraParticles.shape;
        velocity          = auraParticles.velocityOverLifetime;
        sizeOverLifetime  = auraParticles.sizeOverLifetime;
        colorOverLifetime = auraParticles.colorOverLifetime;
        noise             = auraParticles.noise;
        rotation          = auraParticles.rotationOverLifetime;
        psRenderer        = auraParticles.GetComponent<ParticleSystemRenderer>();
        limitVelocity = auraParticles.limitVelocityOverLifetime;
        mpb               = new MaterialPropertyBlock();

        // ✅ Sorting — partículas visibles sobre el fondo
        psRenderer.sortingLayerName = sortingLayerName;
        psRenderer.sortingOrder     = sortingOrder;
    }

    public void ApplyAura(AuraData aura)
    {
        if (aura == null) { StopAura(); return; }

        auraParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ResetModules();
        ApplyVisuals(aura);

        switch (aura.movementType)
        {
            case AuraMovementType.Trail:  ApplyTrail(aura.intensity);  break;
            case AuraMovementType.Burst:  ApplyBurst(aura.intensity);  break;
            case AuraMovementType.Smoke:  ApplySmoke(aura.intensity);  break;
            case AuraMovementType.Pulse:  ApplyPulse(aura.intensity);  break;
            case AuraMovementType.Orbit:  ApplyOrbit(aura.intensity);  break;
            case AuraMovementType.Float:  ApplyFloat(aura.intensity);  break;
            case AuraMovementType.Glitch: ApplyGlitch(aura.intensity); break;
        }
    }

    public void StopAura()
    {
        auraParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // ─── Visual base ─────────────────────────────────────────────

    private void ApplyVisuals(AuraData aura)
    {
        // ── Color ──────────────────────────────────────────────────
        switch (aura.colorMode)
        {
            case AuraColorMode.Constant:
                main.startColor = new ParticleSystem.MinMaxGradient(aura.colorA);
                break;

            case AuraColorMode.RandomBetweenTwo:
                main.startColor = new ParticleSystem.MinMaxGradient(aura.colorA, aura.colorB);
                break;

            case AuraColorMode.Gradient:
                main.startColor = new ParticleSystem.MinMaxGradient(aura.colorGradient);
                break;
        }

        // ── Tamaño ─────────────────────────────────────────────────
        if (aura.sizeVariation > 0f)
        {
            float min = aura.startSize * (1f - aura.sizeVariation);
            float max = aura.startSize;
            main.startSize = new ParticleSystem.MinMaxCurve(min, max);
        }
        else
        {
            main.startSize = new ParticleSystem.MinMaxCurve(aura.startSize);
        }

        // ── Velocidad ──────────────────────────────────────────────
        if (aura.speedVariation > 0f)
        {
            float min = aura.startSpeed * (1f - aura.speedVariation);
            float max = aura.startSpeed;
            main.startSpeed = new ParticleSystem.MinMaxCurve(min, max);
        }
        else
        {
            main.startSpeed = new ParticleSystem.MinMaxCurve(aura.startSpeed);
        }

        // ── Lifetime ───────────────────────────────────────────────
        if (aura.lifetimeVariation > 0f)
        {
            float min = aura.lifetime * (1f - aura.lifetimeVariation);
            float max = aura.lifetime;
            main.startLifetime = new ParticleSystem.MinMaxCurve(min, max);
        }
        else
        {
            main.startLifetime = new ParticleSystem.MinMaxCurve(aura.lifetime);
        }

        // ── Física ─────────────────────────────────────────────────
        main.gravityModifier = aura.gravity;

        var limitVelocity = auraParticles.limitVelocityOverLifetime;
        if (aura.drag > 0f)
        {
            limitVelocity.enabled = true;
            limitVelocity.drag    = new ParticleSystem.MinMaxCurve(aura.drag);
        }
        else
        {
            limitVelocity.enabled = false;
        }

        // ── Rotación ───────────────────────────────────────────────
        if (aura.randomRotation)
            main.startRotation = new ParticleSystem.MinMaxCurve(
                -180f * Mathf.Deg2Rad,
                180f * Mathf.Deg2Rad);
        else
            main.startRotation = new ParticleSystem.MinMaxCurve(0f);

        if (aura.rotationSpeed > 0f)
        {
            rotation.enabled = true;
            rotation.z = new ParticleSystem.MinMaxCurve(
                aura.rotationSpeed * Mathf.Deg2Rad);
        }
        else
        {
            rotation.enabled = false;
        }

        // ── Noise ──────────────────────────────────────────────────
        if (aura.useNoise)
        {
            noise.enabled   = true;
            noise.strength  = aura.noiseStrength;
            noise.frequency = aura.noiseFrequency;
        }
        else
        {
            noise.enabled = false;
        }

        // ── Fade + Size over lifetime ──────────────────────────────
        if (aura.fadeOut || aura.fadeIn || aura.shrinkOverLife || aura.growOverLife)
        {
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(Color.white, 0f),
                    new GradientColorKey(Color.white, 1f)
                },
                new GradientAlphaKey[]
                {
                    new GradientAlphaKey(aura.fadeIn  ? 0f : 1f, 0f),
                    new GradientAlphaKey(aura.fadeOut ? 0f : 1f, 1f)
                }
            );
            colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

            if (aura.shrinkOverLife || aura.growOverLife)
            {
                sizeOverLifetime.enabled = true;
                AnimationCurve sizeCurve = aura.shrinkOverLife
                    ? AnimationCurve.EaseInOut(0f, 1f, 1f, 0f)
                    : AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
                sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);
            }
        }
        else
        {
            colorOverLifetime.enabled = false;
        }

        // ── Sprite ─────────────────────────────────────────────────
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.maxParticles    = 80;

        if (aura.particleSprite != null)
        {
            psRenderer.GetPropertyBlock(mpb);
            mpb.SetTexture("_MainTex", aura.particleSprite.texture);
            psRenderer.SetPropertyBlock(mpb);
        }
    }

    // Llama esto al final de cada ApplyXxx()
    private void ApplyWorldCompensation(float speedMultiplier = 10f)
    {
        velocity.enabled = true;
        // -2f es la velocidad del mundo (viene de ControlBird: -1 * 2f)
        velocity.x = new ParticleSystem.MinMaxCurve(-2f * speedMultiplier);
        velocity.y = new ParticleSystem.MinMaxCurve(0f);
        velocity.z = new ParticleSystem.MinMaxCurve(0f);
    }

    // ─── Reset ────────────────────────────────────────────────────

    private void ResetModules()
    {
        velocity.enabled                                = false;
        sizeOverLifetime.enabled                        = false;
        colorOverLifetime.enabled                       = false;
        noise.enabled                                   = false;
        rotation.enabled                                = false;
        limitVelocity.enabled = false;
        emission.rateOverTime                           = 0;
        emission.SetBursts(new ParticleSystem.Burst[0]);
        main.gravityModifier                            = 0f;
        main.startRotation                              = new ParticleSystem.MinMaxCurve(0f);
    }

    // ─── Comportamientos ──────────────────────────────────────────

    // Estela de fuego/magia que queda atrás
    private void ApplyTrail(float t)
    {
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.3f, 0.6f);
        main.startSpeed    = new ParticleSystem.MinMaxCurve(0.5f * t, 1.5f * t);

        emission.rateOverTime = 30f * t;

        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius    = 0.15f;

        // Fade out
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        // Se encogen al desaparecer
        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        // Al final, reemplaza el bloque velocity por:
        ApplyWorldCompensation(1f); // se queda atrás al ritmo del mundo
        
        // El noise lo añades después de velocity
        noise.enabled   = true;
        noise.strength  = 0.15f;
        noise.frequency = 2f;
    }

    // Explosiones periódicas que emanan del cuerpo
    private void ApplyBurst(float t)
    {
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.4f, 0.7f);
        main.startSpeed    = new ParticleSystem.MinMaxCurve(1f * t, 3f * t);

        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, (short)(8 * t), (short)(12 * t), 999, 0.35f)
        });

        shape.shapeType       = ParticleSystemShapeType.Circle;
        shape.radius          = 0.2f;
        shape.radiusThickness = 1f;

        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        ApplyWorldCompensation(0.8f); // se queda casi estático
    }

    // Nube de humo lenta que se expande
    private void ApplySmoke(float t)
    {
        main.startLifetime   = new ParticleSystem.MinMaxCurve(1f, 2f);
        main.startSpeed      = new ParticleSystem.MinMaxCurve(0.1f, 0.4f * t);
        main.gravityModifier = -0.05f;

        emission.rateOverTime = 8f * t;

        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius    = 0.2f;

        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = AnimationCurve.Linear(0f, 0.5f, 1f, 1.8f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(0.8f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);

        noise.enabled   = true;
        noise.strength  = 0.3f;
        noise.frequency = 0.4f;

        ApplyWorldCompensation(0.6f); // deriva lentamente

    }

    // Ondas de pulso que salen desde el centro
    private void ApplyPulse(float t)
    {
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.4f, 0.6f);
        main.startSpeed    = new ParticleSystem.MinMaxCurve(2f * t, 4f * t);

        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, (short)(16), (short)(16), 999, 0.5f)
        });

        shape.shapeType       = ParticleSystemShapeType.Circle;
        shape.radius          = 0.05f;
        shape.radiusThickness = 0f;

        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = AnimationCurve.EaseInOut(0f, 0.3f, 1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    // Partículas que orbitan alrededor del personaje
    private void ApplyOrbit(float t)
    {
        main.startLifetime   = new ParticleSystem.MinMaxCurve(1.5f, 2f);
        main.startSpeed      = new ParticleSystem.MinMaxCurve(0f);
        main.gravityModifier = 0f;

        emission.rateOverTime = 10f * t;

        shape.shapeType       = ParticleSystemShapeType.Circle;
        shape.radius          = 0.4f;
        shape.radiusThickness = 0f;

        velocity.enabled  = true;
        velocity.x        = new ParticleSystem.MinMaxCurve(0f);
        velocity.y        = new ParticleSystem.MinMaxCurve(0f);
        velocity.z        = new ParticleSystem.MinMaxCurve(0f);
        velocity.orbitalX = new ParticleSystem.MinMaxCurve(0f);
        velocity.orbitalY = new ParticleSystem.MinMaxCurve(0f);
        velocity.orbitalZ = new ParticleSystem.MinMaxCurve(3f * t);

        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(0f, 0f), new GradientAlphaKey(1f, 0.2f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
    }

    // Partículas que flotan hacia arriba suavemente
    private void ApplyFloat(float t)
    {
        main.startLifetime   = new ParticleSystem.MinMaxCurve(0.8f, 1.5f);
        main.startSpeed      = new ParticleSystem.MinMaxCurve(0.3f * t, 0.8f * t);
        main.gravityModifier = -0.15f;
        main.startRotation   = new ParticleSystem.MinMaxCurve(-180f * Mathf.Deg2Rad, 180f * Mathf.Deg2Rad);

        emission.rateOverTime = 15f * t;

        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius    = 0.2f;

        sizeOverLifetime.enabled = true;
        AnimationCurve sizeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, sizeCurve);

        noise.enabled    = true;
        noise.strength   = 0.5f;
        noise.frequency  = 0.8f;
        noise.scrollSpeed = 0.5f;

        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        ApplyWorldCompensation(0.5f); // flota y deriva
    }

    // Caótico, rápido, tipo glitch
    private void ApplyGlitch(float t)
    {
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.1f, 0.3f);
        main.startSpeed    = new ParticleSystem.MinMaxCurve(1f * t, 5f * t);
        main.startRotation = new ParticleSystem.MinMaxCurve(-180f * Mathf.Deg2Rad, 180f * Mathf.Deg2Rad);

        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0f, (short)(8 * t), (short)(15 * t), 999, 0.08f)
        });

        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius    = 0.3f;

        noise.enabled    = true;
        noise.strength   = 2f;
        noise.frequency  = 8f;
        noise.scrollSpeed = 3f;

        rotation.enabled = true;
        rotation.z = new ParticleSystem.MinMaxCurve(-360f * Mathf.Deg2Rad * t, 360f * Mathf.Deg2Rad * t);

        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]  { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
            new GradientAlphaKey[]  { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(0f, 1f) }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        ApplyWorldCompensation(1.2f); // caótico pero se queda atrás
    }
}