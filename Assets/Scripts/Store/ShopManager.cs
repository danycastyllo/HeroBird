using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    // ── Enums ─────────────────────────────────────────────────────────────────
    public enum ShopTab { Characters, Auras }

    // ── Datos ─────────────────────────────────────────────────────────────────
    public List<CharacterData> characters;
    public List<AuraData> auras;

    // ── Referencias UI ────────────────────────────────────────────────────────
    public Transform characterContentPanel;
    public Transform auraContentPanel;
    public Button characterTabButton;
    public Button auraTabButton;
    public GameObject characterItemPrefab;
    public Text coinsText;
    public GameObject characterShopPanel;
    public GameObject auraShopPanel;

    // ── Configuración ─────────────────────────────────────────────────────────
    public string defaultCharacterName = "bird1";
    public string defaultAuraName      = "DefaultAura";

    // ── Privadas ──────────────────────────────────────────────────────────────
    ShopItem selectedShopItem;
    int playerCoins;
    public int PlayerCoins => playerCoins;

    static readonly Color colorTabActive   = Color.white;
    static readonly Color colorTabInactive = new Color(0.9f, 0.9f, 0.9f, 1f);

    // ─────────────────────────────────────────────────────────────────────────
    void Start()
    {
        LoadUnlockedItems();
        UpdateCoinsUI();
        GenerateStoreItems();
        SwitchTab(ShopTab.Characters);
    }

    // ── Generación de items ───────────────────────────────────────────────────
    void GenerateStoreItems()
    {
        PopulateShop(characters.Cast<ShopItemData>().ToList(), characterContentPanel);
        PopulateShop(auras.Cast<ShopItemData>().ToList(), auraContentPanel);
    }

    public void PopulateShop(List<ShopItemData> items, Transform contentPanel)
    {
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        foreach (var item in items)
        {
            GameObject newItem = Instantiate(characterItemPrefab, contentPanel);
            ShopItem shopItem  = newItem.GetComponent<ShopItem>();
            shopItem.Setup(item, this);

            if (item.isUnlocked)
                shopItem.DisableBuyButton();
        }
    }

    // ── Selección y compra ────────────────────────────────────────────────────
    public void SelectShopItem(ShopItem shopItem)
    {
        if (selectedShopItem != null)
            selectedShopItem.SetSelectedState(false);

        selectedShopItem = shopItem;
        selectedShopItem.SetSelectedState(true);

        if (shopItem.ItemData is CharacterData character)
            PlayerPrefs.SetString(GameKeys.SelectedCharacter, character.itemName);
        else if (shopItem.ItemData is AuraData aura)
            PlayerPrefs.SetString(GameKeys.SelectedAura, aura.itemName);

        PlayerPrefs.Save();
    }

    public void TryBuyCharacter(CharacterData character) => TryBuy(character, "Character");
    public void TryBuyAura(AuraData aura)               => TryBuy(aura, "Aura");

    void TryBuy(ShopItemData item, string prefix)
    {
        if (item.isUnlocked || playerCoins < item.price) return;

        playerCoins -= item.price;
        PlayerPrefs.SetInt(GameKeys.PlayerCoins, playerCoins);
        UnlockItem(item, prefix);
        UpdateCoinsUI();
    }

    void UnlockItem(ShopItemData item, string prefix)
    {
        item.isUnlocked = true;
        string key = prefix == "Character"
            ? string.Format(GameKeys.CharacterUnlocked, item.itemName)
            : string.Format(GameKeys.AuraUnlocked, item.itemName);
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    // ── Persistencia ──────────────────────────────────────────────────────────
    void LoadUnlockedItems()
    {
        foreach (var character in characters)
            character.isUnlocked = PlayerPrefs.GetInt(
                string.Format(GameKeys.CharacterUnlocked, character.itemName), 0) == 1;

        foreach (var aura in auras)
            aura.isUnlocked = PlayerPrefs.GetInt(
                string.Format(GameKeys.AuraUnlocked, aura.itemName), 0) == 1;
    }

    void UpdateCoinsUI()
    {
        playerCoins    = PlayerPrefs.GetInt(GameKeys.PlayerCoins, 0);
        coinsText.text = playerCoins.ToString();
    }

    // ── Tabs ──────────────────────────────────────────────────────────────────
    public void SwitchTab(ShopTab tab)
    {
        bool isCharacters = tab == ShopTab.Characters;
        characterShopPanel.SetActive(isCharacters);
        auraShopPanel.SetActive(!isCharacters);
        SetActiveTab(isCharacters ? characterTabButton : auraTabButton);
        LoadSelectedItem();
    }

    // Mantener compatibilidad con botones de UI que llaman por string
    public void SwitchToCharacters() => SwitchTab(ShopTab.Characters);
    public void SwitchToAuras()      => SwitchTab(ShopTab.Auras);

    void SetActiveTab(Button activeTab)
    {
        characterTabButton.image.color = colorTabInactive;
        auraTabButton.image.color      = colorTabInactive;
        activeTab.image.color          = colorTabActive;
        activeTab.transform.SetSiblingIndex(activeTab.transform.parent.childCount - 2);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────
    ShopItem FindShopItemByName(Transform contentPanel, string itemName)
    {
        foreach (Transform child in contentPanel)
        {
            ShopItem shopItem = child.GetComponent<ShopItem>();
            if (shopItem != null && shopItem.ItemData is ShopItemData data && data.itemName == itemName)
                return shopItem;
        }
        return null;
    }

    void LoadSelectedItem()
    {
        if (characterShopPanel.activeSelf)
        {
            string savedName = PlayerPrefs.GetString(GameKeys.SelectedCharacter, "");
            var item = FindShopItemByName(characterContentPanel,
                string.IsNullOrEmpty(savedName) ? defaultCharacterName : savedName);
            if (item != null) SelectShopItem(item);
        }
        else if (auraShopPanel.activeSelf)
        {
            string savedName = PlayerPrefs.GetString(GameKeys.SelectedAura, "");
            var item = FindShopItemByName(auraContentPanel,
                string.IsNullOrEmpty(savedName) ? defaultAuraName : savedName);
            if (item != null) SelectShopItem(item);
        }
    }
}