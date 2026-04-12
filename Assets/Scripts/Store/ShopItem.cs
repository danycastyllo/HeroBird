using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public Image icon;
    public Image imgToSelect;
    public Text priceText;
    public Button buyButton;

    public Sprite originalSprite;
    public Sprite selectedSprite;

    private ShopManager shopManager;
    private object itemData;

    public void Setup<T>(T item, ShopManager manager)
    {
        itemData = item;
        shopManager = manager;

        if (item is CharacterData character)
        {
            icon.sprite = character.icon;
            priceText.text = character.price.ToString();
            buyButton.interactable = !character.isUnlocked;
            buyButton.onClick.AddListener(() => BuyCharacter(character));
        }
        else if (item is AuraData aura)
        {
            icon.sprite = aura.icon;
            icon.color  = GetStartColor(aura).Evaluate(0f, Random.value);
            
            // ✅ Fuerza tamaño fijo para el icono del aura
            icon.rectTransform.sizeDelta = new Vector2(100f, 100f);

            priceText.text = aura.price.ToString();
            buyButton.interactable = !aura.isUnlocked;
            buyButton.onClick.AddListener(() => BuyAura(aura));
        }

        imgToSelect.GetComponent<Button>().onClick.AddListener(() => SelectItem());
    }

    private ParticleSystem.MinMaxGradient GetStartColor(AuraData aura)
    {
        switch (aura.colorMode)
        {
            case AuraColorMode.Constant:
                return new ParticleSystem.MinMaxGradient(aura.colorA);
            case AuraColorMode.RandomBetweenTwo:
                return new ParticleSystem.MinMaxGradient(aura.colorA, aura.colorB);
            case AuraColorMode.Gradient:
                return new ParticleSystem.MinMaxGradient(aura.colorGradient);
            default:
                return new ParticleSystem.MinMaxGradient(aura.colorA);
        }
    }

    private void BuyCharacter(CharacterData character)
    {
        if (shopManager.playerCoins >= character.price && !character.isUnlocked)
        {
            shopManager.TryBuyCharacter(character);
            DisableBuyButton();
            SelectItem(); // ✅ selecciona automáticamente al comprar
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el personaje ya está desbloqueado.");
        }
    }

    private void BuyAura(AuraData aura)
    {
        if (shopManager.playerCoins >= aura.price && !aura.isUnlocked)
        {
            shopManager.TryBuyAura(aura);
            DisableBuyButton();
            SelectItem(); // ✅ selecciona automáticamente al comprar
        }
        else
        {
            Debug.Log("No tienes suficientes monedas o el aura ya está desbloqueada.");
        }
    }

    public void DisableBuyButton()
    {
        if (buyButton != null)
            buyButton.gameObject.SetActive(false);
    }

    private void SelectItem()
    {
        shopManager.SelectShopItem(this);
    }

    public void SetSelectedState(bool isSelected)
    {
        imgToSelect.sprite = isSelected ? selectedSprite : originalSprite;
    }

    public object ItemData => itemData;
}