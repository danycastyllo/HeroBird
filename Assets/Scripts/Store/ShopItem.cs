using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // ── Referencias UI ────────────────────────────────────────────────────────
    public Image icon;
    public Image imgToSelect;
    public Text priceText;
    public Button buyButton;

    // ── Sprites de selección ──────────────────────────────────────────────────
    public Sprite originalSprite;
    public Sprite selectedSprite;

    // ── Privadas ──────────────────────────────────────────────────────────────
    ShopManager shopManager;
    ShopItemData itemData;
    Button selectButton;

    // ── API pública ───────────────────────────────────────────────────────────
    public ShopItemData ItemData => itemData;

    public void Setup<T>(T item, ShopManager manager) where T : ShopItemData
    {
        itemData    = item;
        shopManager = manager;
        selectButton = imgToSelect.GetComponent<Button>();

        priceText.text         = item.price.ToString();
        buyButton.interactable = !item.isUnlocked;

        if (item is CharacterData character)
        {
            icon.sprite = character.icon;
            buyButton.onClick.AddListener(() => BuyCharacter(character));
        }
        else if (item is AuraData aura)
        {
            icon.sprite  = aura.icon;
            icon.color   = GetAuraStartColor(aura).Evaluate(0f, Random.value);
            icon.rectTransform.sizeDelta = new Vector2(100f, 100f);
            buyButton.onClick.AddListener(() => BuyAura(aura));
        }

        selectButton.onClick.AddListener(SelectItem);
    }

    public void DisableBuyButton()
    {
        if (buyButton != null)
            buyButton.gameObject.SetActive(false);
    }

    public void SetSelectedState(bool isSelected)
    {
        imgToSelect.sprite = isSelected ? selectedSprite : originalSprite;
    }

    // ── Privadas ──────────────────────────────────────────────────────────────
    void BuyCharacter(CharacterData character)
    {
        shopManager.TryBuyCharacter(character);
        DisableBuyButton();
        SelectItem();
    }

    void BuyAura(AuraData aura)
    {
        shopManager.TryBuyAura(aura);
        DisableBuyButton();
        SelectItem();
    }

    void SelectItem() => shopManager.SelectShopItem(this);

    ParticleSystem.MinMaxGradient GetAuraStartColor(AuraData aura)
    {
        switch (aura.colorMode)
        {
            case AuraColorMode.Constant:          return new ParticleSystem.MinMaxGradient(aura.colorA);
            case AuraColorMode.RandomBetweenTwo:  return new ParticleSystem.MinMaxGradient(aura.colorA, aura.colorB);
            case AuraColorMode.Gradient:          return new ParticleSystem.MinMaxGradient(aura.colorGradient);
            default:                              return new ParticleSystem.MinMaxGradient(aura.colorA);
        }
    }
}