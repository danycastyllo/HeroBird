using UnityEditor;
using UnityEngine;
using System.IO;

public class CharacterDataGenerator : EditorWindow
{
    private string folderPath   = "Assets/Data/Characters";
    private string spritesPath  = "Assets/Sprites/Birds";
    private string baseName     = "bird";
    private int startIndex      = 4;
    private int endIndex        = 45;
    private int defaultPrice    = 100;

    [MenuItem("HeroBird/Generate Character Assets")]
    public static void ShowWindow()
    {
        GetWindow<CharacterDataGenerator>("Generate Characters");
    }

    private void OnGUI()
    {
        GUILayout.Label("Generador de CharacterData", EditorStyles.boldLabel);

        folderPath   = EditorGUILayout.TextField("Carpeta destino",   folderPath);
        spritesPath  = EditorGUILayout.TextField("Carpeta sprites",   spritesPath);
        baseName     = EditorGUILayout.TextField("Nombre base",       baseName);
        startIndex   = EditorGUILayout.IntField("Desde (número)",     startIndex);
        endIndex     = EditorGUILayout.IntField("Hasta (número)",     endIndex);
        defaultPrice = EditorGUILayout.IntField("Precio por defecto", defaultPrice);

        EditorGUILayout.HelpBox(
            $"Se crearán {endIndex - startIndex + 1} assets.\n" +
            $"Sprites esperados: \"{baseName} (N)_0\" a \"{baseName} (N)_3\"\n" +
            $"Ejemplo: bird (4)_0, bird (4)_1, bird (4)_2, bird (4)_3",
            MessageType.Info);

        if (GUILayout.Button("Generar"))
            Generate();
    }

    private void Generate()
    {
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            AssetDatabase.Refresh();
        }

        int created  = 0;
        int skipped  = 0;
        int noSprite = 0;

        for (int i = startIndex; i <= endIndex; i++)
        {
            string assetName = $"{baseName}{i}";
            string assetPath = $"{folderPath}/{assetName}.asset";

            if (File.Exists(assetPath))
            {
                Debug.LogWarning($"Ya existe: {assetName} — omitido.");
                skipped++;
                continue;
            }

            // Carga los 4 sprites del spritesheet
            // Formato esperado: "bird (4)_0", "bird (4)_1", "bird (4)_2", "bird (4)_3"
            Sprite[] spritesheet = LoadSpritesheet(i);

            if (spritesheet == null || spritesheet.Length == 0)
            {
                Debug.LogWarning($"No se encontraron sprites para {baseName} ({i}) — asset creado sin sprites.");
                noSprite++;
            }

            CharacterData data  = ScriptableObject.CreateInstance<CharacterData>();
            data.itemName       = assetName;
            data.price          = defaultPrice;
            data.isUnlocked     = false;
            data.spritesheet    = spritesheet ?? new Sprite[0];
            data.icon           = (spritesheet != null && spritesheet.Length > 0) ? spritesheet[0] : null;

            AssetDatabase.CreateAsset(data, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        string summary = $"✅ {created} assets creados.\n";
        if (skipped  > 0) summary += $"⚠️ {skipped} omitidos (ya existían).\n";
        if (noSprite > 0) summary += $"❌ {noSprite} sin sprites encontrados.";

        Debug.Log(summary);
        EditorUtility.DisplayDialog("Generación completa", summary, "OK");
    }

    private Sprite[] LoadSpritesheet(int index)
    {
        // Ruta del spritesheet: Assets/Sprites/Birds/bird (N)
        string sheetName = $"{baseName} ({index})";
        string sheetPath = $"{spritesPath}/{sheetName}.png";

        // Carga todos los sub-sprites del asset
        Object[] all = AssetDatabase.LoadAllAssetsAtPath(sheetPath);

        if (all == null || all.Length == 0)
        {
            Debug.LogWarning($"No se encontró spritesheet en: {sheetPath}");
            return null;
        }

        // Filtra solo Sprites y los ordena por nombre (_0, _1, _2, _3)
        System.Collections.Generic.List<Sprite> sprites = new();

        for (int frame = 0; frame < 4; frame++)
        {
            string expectedName = $"{sheetName}_{frame}";
            foreach (Object obj in all)
            {
                if (obj is Sprite s && s.name == expectedName)
                {
                    sprites.Add(s);
                    break;
                }
            }
        }

        if (sprites.Count == 0)
        {
            Debug.LogWarning($"Sprites encontrados pero ninguno con formato '{sheetName}_0'");
            return null;
        }

        return sprites.ToArray();
    }
}