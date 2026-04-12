using UnityEngine;

public class AuraPreviewController : MonoBehaviour
{
    public ParticleSystem previewParticles;
    private AuraController auraController;

    void Awake()
    {
        auraController = GetComponent<AuraController>();
    }

    public void PreviewAura(AuraData aura)
    {
        auraController.ApplyAura(aura);
    }
}
