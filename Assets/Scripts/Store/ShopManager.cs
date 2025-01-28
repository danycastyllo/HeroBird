using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItemData
    {
        public string name;
        public Sprite icon;
        public int price;
        public bool isUnlocked;
    }
    [System.Serializable]
    public class CharacterData : ShopItemData
    {
        // Propiedades o métodos específicos para personajes (si es necesario)
    }

    [System.Serializable]
    public class AuraData : ShopItemData
    {
        // Propiedades o métodos específicos para auras (si es necesario)
    }

    

    public List<CharacterData> characters; // Lista de personajes
    public List<AuraData> auras; // Lista de auras

    public Transform characterContentPanel; // Panel donde se mostrarán los personajes
    public Transform auraContentPanel; // Panel donde se mostrarán las auras
    private Transform activeContentPanel; // Panel activo dependiendo de la pestaña seleccionada

    public Button characterTabButton; // Botón para la pestaña de personajes
    public Button auraTabButton;     // Botón para la pestaña de auras
    private Button activeTabButton;  // Botón actualmente activo

    public GameObject characterItemPrefab; // Prefab del personaje
    public Text coinsText; // Texto para mostrar las monedas del jugador
    public int playerCoins = 0; // Monedas iniciales del jugador

    public GameObject characterShopPanel; // Panel donde se mostrarán los personajes
    public GameObject auraShopPanel; // Panel donde se mostrarán las auras

    
    private ShopItem selectedShopItem; // Referencia al ítem actualmente seleccionado

    public string defaultCharacterName = "DefaultCharacter"; // Nombre del personaje por defecto
    public string defaultAuraName = "DefaultAura";           // Nombre del aura por defecto


    private void Start()
    {
        LoadUnlockedCharacters();
        LoadUnlockedAuras();
        UpdateCoinsUI();
        GenerateStoreItems();
        SwitchTab("Characters");
    }
    private void GenerateStoreItems(){ // genera todos los items de character en el panel de character y 
        activeContentPanel = characterContentPanel;
        PopulateShop(characters.Cast<ShopItemData>().ToList());
        activeContentPanel = auraContentPanel;
        PopulateShop(auras.Cast<ShopItemData>().ToList());
    }

    public void PopulateShop(List<ShopItemData> items) // genera items de una lista de items dada
    {
        // Limpia el contenido actual
        foreach (Transform child in activeContentPanel)
        {
            Destroy(child.gameObject);
        }

        // Genera los objetos en la tienda
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(characterItemPrefab, activeContentPanel);

            ShopItem shopItem = newItem.GetComponent<ShopItem>();
            shopItem.Setup(item, this);

            // Desactiva el botón si ya fue desbloqueado
            if (item.isUnlocked)
            {
                shopItem.DisableBuyButton();
            }
        }
    }

    // Método para seleccionar un ítem
    public void SelectShopItem(ShopItem shopItem)
    {
        // Deseleccionar el ítem anterior
        if (selectedShopItem != null)
        {
            selectedShopItem.SetSelectedState(false);
        }

        // Actualizar la referencia y establecer el nuevo estado
        selectedShopItem = shopItem;
        selectedShopItem.SetSelectedState(true);


        // Guardar selección en PlayerPrefs
        if (shopItem.ItemData is CharacterData character)
        {
            PlayerPrefs.SetString("SelectedCharacterName", character.name); // Guarda el nombre del character seleccionado
        }
        else if (shopItem.ItemData is AuraData aura)
        {
            PlayerPrefs.SetString("SelectedAuraName", aura.name); // Guarda el nombre del aura seleccionado
        }
        PlayerPrefs.Save();
    }

    public void TryBuyCharacter(CharacterData character)
    {
            playerCoins -= character.price;
            PlayerPrefs.SetInt("PlayerCoins", playerCoins); // Guarda el nombre del aura seleccionado
            UnlockCharacter(character);
            UpdateCoinsUI();
    }

    public void TryBuyAura(AuraData aura)
    {
        if (playerCoins >= aura.price && !aura.isUnlocked)
        {
            playerCoins -= aura.price;
            UnlockAura(aura);
            UpdateCoinsUI();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el personaje ya está desbloqueado.");
        }
    }

    // luego de comprar el item se desbloquea y se guarda
    public void UnlockCharacter(CharacterData character)
    {
        if (!character.isUnlocked)
        {
            character.isUnlocked = true;

            // Guardar estado en PlayerPrefs
            PlayerPrefs.SetInt($"Character_{character.name}_Unlocked", 1);

            // Asegurarse de que los cambios se guardan
            PlayerPrefs.Save();

            Debug.Log($"Personaje {character.name} desbloqueado y guardado.");
        }
    }

    public void UnlockAura(AuraData aura)
    {
        if (!aura.isUnlocked)
        {
            aura.isUnlocked = true;

            // Guardar estado en PlayerPrefs
            PlayerPrefs.SetInt($"Aura_{aura.name}_Unlocked", 1);

            // Asegurarse de que los cambios se guardan
            PlayerPrefs.Save();

            Debug.Log($"Aura {aura.name} desbloqueado y guardado.");
        }
    }



    private void LoadUnlockedCharacters()
    {
        foreach (var character in characters)
        {
            // Recuperar el estado de desbloqueo desde PlayerPrefs
            character.isUnlocked = PlayerPrefs.GetInt($"Character_{character.name}_Unlocked", 0) == 1;
        }
    }

    private void LoadUnlockedAuras()
    {
        foreach (var aura in auras)
        {
            // Recuperar el estado de desbloqueo desde PlayerPrefs
            aura.isUnlocked = PlayerPrefs.GetInt($"Aura_{aura.name}_Unlocked", 0) == 1;
        }
    }

    private void UpdateCoinsUI()
    {
        playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        coinsText.text = playerCoins.ToString();
    }

    public void SwitchTab(string tabName)
    {
        if (tabName == "Characters")
        {
            characterShopPanel.SetActive(true);
            auraShopPanel.SetActive(false);
            SetActiveTab(characterTabButton);
        }
        else if (tabName == "Auras")
        {
            characterShopPanel.SetActive(false);
            auraShopPanel.SetActive(true);
            SetActiveTab(auraTabButton);
        }
        LoadSelectedItem(); // Cargar ítem seleccionado previamente
    }

    // Cambia el color y la posicion de la pestaña seleccionada
    private void SetActiveTab(Button selectedTab)
    {
        // Restaurar el color de todos los botones
        characterTabButton.image.color = new Color(0.9f, 0.9f, 0.9f, 1f);
        auraTabButton.image.color = new Color(0.9f, 0.9f, 0.9f, 1f);

        // Oscurecer el botón seleccionado
        selectedTab.image.color = Color.white;

        // Coloca el botón seleccionado en la penultima posicion en la jerarquia
        selectedTab.transform.SetSiblingIndex(selectedTab.transform.parent.childCount - 2);


        // Actualizar la referencia del botón activo
        activeTabButton = selectedTab;
    }

    private ShopItem FindShopItemByName(Transform contentPanel, string itemName)
    {
        foreach (Transform child in contentPanel)
        {
            ShopItem shopItem = child.GetComponent<ShopItem>();
            if (shopItem != null && shopItem.ItemData is ShopItemData data && data.name == itemName)
            {
                return shopItem;
            }
        }
        return null;
    }

    private void SelectDefaultCharacter()
    {
        // Seleccionar personaje por defecto
        var defaultCharacterItem = FindShopItemByName(characterContentPanel, defaultCharacterName);
        var itemData = defaultCharacterItem.ItemData;
        if (defaultCharacterItem != null)
        {
            defaultCharacterItem.DisableBuyButton();
            SelectShopItem(defaultCharacterItem);    // Seleccionarlo
            if (itemData is CharacterData character){ TryBuyCharacter(character); }
        }
    }

    private void SelectDefaultAura()
    {
        // Seleccionar aura por defecto
        var defaultAuraItem = FindShopItemByName(auraContentPanel, defaultAuraName);
        var itemData = defaultAuraItem.ItemData;
        if (defaultAuraItem != null)
        {
            defaultAuraItem.DisableBuyButton();
            SelectShopItem(defaultAuraItem);    // Seleccionarlo
            if (itemData is AuraData aura){ TryBuyAura(aura); }
        }
    }


    private void LoadSelectedItem()
    {
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacterName", "");
        string selectedAuraName = PlayerPrefs.GetString("SelectedAuraName", "");

        if (characterShopPanel.gameObject.activeSelf)
        {
            if (!string.IsNullOrEmpty(selectedCharacterName))
            {
                var selectedCharacter = characters.FirstOrDefault(c => c.name == selectedCharacterName);
                if (selectedCharacter != null)
                {
                    var shopItem = FindShopItemByName(characterContentPanel, selectedCharacterName);
                    if (shopItem != null)
                    {
                        SelectShopItem(shopItem);
                    }
                }
            }else { SelectDefaultCharacter(); } // Seleccionar y configurar personaje por defecto
        
        } else if (auraShopPanel.gameObject.activeSelf)
        {

            if (!string.IsNullOrEmpty(selectedAuraName))
            {
                var selectedAura = auras.FirstOrDefault(a => a.name == selectedAuraName);
                if (selectedAura != null)
                {
                    var shopItem = FindShopItemByName(auraContentPanel, selectedAuraName);
                    if (shopItem != null)
                    {
                        SelectShopItem(shopItem);
                    }
                }
            }else { SelectDefaultAura(); } // Seleccionar y configurar aura por defecto

        } 
    }
}
