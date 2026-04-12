using System.Collections.Generic;
using UnityEngine;

public class AuraLoader : MonoBehaviour
{
    [Header("Todas las auras del juego")]
    [SerializeField] private List<AuraData> allAuras;

    [Header("Referencias")]
    [SerializeField] private AuraController auraController;

    private void Start()
    {
        ApplySelectedAura();
    }

    private void ApplySelectedAura()
    {
        string selectedName = PlayerPrefs.GetString("SelectedAuraName", "");

        if (string.IsNullOrEmpty(selectedName))
        {
            auraController.StopAura();
            return;
        }

        AuraData selected = allAuras.Find(a => a.itemName == selectedName);

        if (selected == null)
        {
            Debug.LogWarning($"AuraLoader: No se encontró el aura '{selectedName}'");
            auraController.StopAura();
            return;
        }

        auraController.ApplyAura(selected);
    }
}