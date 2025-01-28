using UnityEngine;
using UnityEngine.UI;
using static ShopManager;

public class ShopItem : MonoBehaviour
{
    public Image icon; // Imagen del ítem (se actualizará dinámicamente)
    public Image imgToSelect; // Imagen del ítem (se actualizará dinámicamente)
    public Text priceText; // Precio mostrado
    public Button buyButton; // Botón de compra

    public Sprite originalSprite; // Sprite original sin contorno
    public Sprite selectedSprite; // Sprite con contorno
    private ShopManager shopManager;
    private object itemData; // Puede ser Character o Aura


    public void Setup<T>(T item, ShopManager manager)
    {
        itemData = item; // Aquí se asigna el dato del ítem
        shopManager = manager;

        if (item is CharacterData character)
        {
            icon.sprite = character.icon; // Asignar sprite original

            priceText.text = character.price.ToString();
            buyButton.interactable = !character.isUnlocked;
            buyButton.onClick.AddListener(() => BuyCharacter(character));
        }
        else if (item is AuraData aura)
        {
            icon.sprite = aura.icon;

            priceText.text = aura.price.ToString();
            buyButton.interactable = !aura.isUnlocked;
            buyButton.onClick.AddListener(() => BuyAura(aura));
        }

        // Añade funcionalidad de selección
        imgToSelect.GetComponent<Button>().onClick.AddListener(() => SelectItem());
    }

    private void BuyCharacter(CharacterData character)
    {
        if (shopManager.playerCoins >= character.price && !character.isUnlocked){
            shopManager.TryBuyCharacter(character);
            buyButton.interactable = !character.isUnlocked;
            DisableBuyButton();
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el personaje ya está desbloqueado.");
        }
    }

    private void BuyAura(AuraData aura)
    {
        shopManager.TryBuyAura(aura);
        buyButton.interactable = !aura.isUnlocked;
    }

    // Desactivar el botón de compra
    public void DisableBuyButton()
    {
        if (buyButton != null)
        {
            buyButton.gameObject.SetActive(false);
        }
    }

    // Método llamado al seleccionar el ítem
    private void SelectItem()
    {
        shopManager.SelectShopItem(this); 
    }

    // Cambiar sprite según selección
    public void SetSelectedState(bool isSelected)
    {
        imgToSelect.sprite = isSelected ? selectedSprite : originalSprite;
    }


    public object ItemData // Propiedad pública de solo lectura
    {
        get { return itemData; }
    }
}
