using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public List<CharacterData> characters;
    public List<AuraData> auras;

    public Transform characterContentPanel;
    public Transform auraContentPanel;

    public Button characterTabButton;
    public Button auraTabButton;

    public GameObject characterItemPrefab;
    public Text coinsText;
    public int playerCoins = 0;

    public GameObject characterShopPanel;
    public GameObject auraShopPanel;

    private ShopItem selectedShopItem;

    public string defaultCharacterName = "bird1";
    public string defaultAuraName = "DefaultAura";

    private void Start()
    {
        LoadUnlockedItems();
        UpdateCoinsUI();
        GenerateStoreItems();
        SwitchTab("Characters");
    }

    private void GenerateStoreItems()
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
            ShopItem shopItem = newItem.GetComponent<ShopItem>();
            shopItem.Setup(item, this);

            if (item.isUnlocked)
                shopItem.DisableBuyButton();
        }
    }

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

    public void TryBuyCharacter(CharacterData character)
    {
        if (playerCoins >= character.price && !character.isUnlocked)
        {
            playerCoins -= character.price;
            PlayerPrefs.SetInt(GameKeys.PlayerCoins, playerCoins);
            UnlockItem(character, "Character");
            UpdateCoinsUI();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el personaje ya está desbloqueado.");
        }
    }

    public void TryBuyAura(AuraData aura)
    {
        if (playerCoins >= aura.price && !aura.isUnlocked)
        {
            playerCoins -= aura.price;
            PlayerPrefs.SetInt(GameKeys.PlayerCoins, playerCoins);
            UnlockItem(aura, "Aura");
            UpdateCoinsUI();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el aura ya está desbloqueada.");
        }
    }

    private void UnlockItem(ShopItemData item, string prefix)
    {
        item.isUnlocked = true;
        string key = prefix == "Character" 
            ? string.Format(GameKeys.CharacterUnlocked, item.itemName)
            : string.Format(GameKeys.AuraUnlocked, item.itemName);
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
        Debug.Log($"{prefix} {item.itemName} desbloqueado.");
    }

    private void LoadUnlockedItems()
    {
        foreach (var character in characters)
            character.isUnlocked = PlayerPrefs.GetInt(string.Format(GameKeys.CharacterUnlocked, character.itemName), 0) == 1;

        foreach (var aura in auras)
            aura.isUnlocked = PlayerPrefs.GetInt(string.Format(GameKeys.AuraUnlocked, aura.itemName), 0) == 1;
    }

    private void UpdateCoinsUI()
    {
        playerCoins = PlayerPrefs.GetInt(GameKeys.PlayerCoins, 0);
        coinsText.text = playerCoins.ToString();
    }

    public void SwitchTab(string tabName)
    {
        characterShopPanel.SetActive(tabName == "Characters");
        auraShopPanel.SetActive(tabName == "Auras");
        SetActiveTab(tabName == "Characters" ? characterTabButton : auraTabButton);
        LoadSelectedItem();
    }

    private void SetActiveTab(Button selectedTab)
    {
        characterTabButton.image.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        auraTabButton.image.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        selectedTab.image.color = Color.white;
        selectedTab.transform.SetSiblingIndex(selectedTab.transform.parent.childCount - 2);
    }

    private ShopItem FindShopItemByName(Transform contentPanel, string itemName)
    {
        foreach (Transform child in contentPanel)
        {
            ShopItem shopItem = child.GetComponent<ShopItem>();
            if (shopItem != null && shopItem.ItemData is ShopItemData data && data.itemName == itemName)
                return shopItem;
        }
        return null;
    }

    private void LoadSelectedItem()
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