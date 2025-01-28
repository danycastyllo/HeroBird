using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class SkinManagerGlobal : MonoBehaviour
{
    public static SkinManagerGlobal Instance;

    [System.Serializable]
    public class SkinData
    {
        public string name = "bird";       // Nombre de la skin (opcional)
        public Sprite[] spritesheet; // Spritesheet seleccionado
    }

    public List<SkinData> SkinBirds; // Lista de personajes
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer
    private string SelectedCharacter;
    public int skinNr; // Índice del skin seleccionado
    private Sprite lastSprite; // Último sprite mostrado para evitar cambios innecesarios

    /// <summary>
    /// Obtiene el spritesheet de un ítem por su nombre.
    /// </summary>
    /// <param name="nombre">El nombre del ítem.</param>
    /// <returns>El array de sprites (spritesheet) o null si no se encuentra.</returns>
    public Sprite[] GetSpriteSheetByName(string name)
    {
        // Buscar el ítem con el nombre especificado
        SkinData SkinBird = SkinBirds.Find(i => i.name == name);

        if (SkinBird != null)
        {
            Debug.Log($"Se encontró el ítem: {name}");
            return SkinBird.spritesheet;
        }
        else
        {
            Debug.LogWarning($"No se encontró ningún ítem con el nombre: {name}");
            return null;
        }
    }

    void Start()
    {
        SelectedCharacter = PlayerPrefs.GetString("SelectedCharacterName"); // Obtiene el personaje seleccionado
        spriteRenderer = GetComponent<SpriteRenderer>();
        skinNr = SkinBirds.FindIndex(item => item.name == SelectedCharacter); // obtiene el indice en la lista de skins con el nombre del personaje seleccionado

        // Asegúrate de que el índice del skin esté dentro del rango válido
        skinNr = Mathf.Clamp(skinNr, 0, SkinBirds.Count - 1);
    }

    void LateUpdate()
    {
        UpdateSkin();
    }

    private void UpdateSkin()
    {
        // Obtén el sprite actual del SpriteRenderer
        Sprite currentSprite = spriteRenderer.sprite;

        // Si el sprite actual es el mismo que el último, no hacemos nada
        if (currentSprite == lastSprite || currentSprite == null) return;

        // Extrae el índice del frame actual desde el nombre del sprite
        int spriteIndex = GetSpriteIndex(currentSprite.name);

        // Si no se puede obtener un índice válido, salimos
        if (spriteIndex < 0 || spriteIndex >= SkinBirds[skinNr].spritesheet.Length) return;

        // Cambia el sprite al correspondiente del skin seleccionado
        spriteRenderer.sprite = SkinBirds[skinNr].spritesheet[spriteIndex];
        lastSprite = spriteRenderer.sprite; // Actualiza el último sprite mostrado
    }

    private int GetSpriteIndex(string spriteName)
    {
        // Intenta extraer el índice del nombre del sprite (formato esperado: "bird (1)_X")
        string[] splitName = spriteName.Split('_');
        if (splitName.Length < 2) return -1; // No hay un formato válido

        if (int.TryParse(splitName[1], out int spriteIndex))
        {
            return spriteIndex; // Devuelve el índice del frame
        }

        return -1; // Retorna -1 si no se pudo obtener el índice
    }

    public void SetSkin(int newSkinNr)
    {
        // Cambia el skin actual y reinicia el último sprite mostrado
        skinNr = Mathf.Clamp(newSkinNr, 0, SkinBirds.Count - 1);
        lastSprite = null; // Forzar actualización en el próximo LateUpdate
    }
}
