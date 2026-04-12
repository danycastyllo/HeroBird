using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class ShopManagerAssigner : EditorWindow
{
    private string charactersPath = "Assets/Data/Characters";
    private string aurasPath      = "Assets/Data/Auras";
    private ShopManager shopManager;
    private bool assignCharacters = true;
    private bool assignAuras      = true;

    [MenuItem("HeroBird/Assign Items to ShopManager")]
    public static void ShowWindow()
    {
        GetWindow<ShopManagerAssigner>("Assign to ShopManager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Asignar items a ShopManager", EditorStyles.boldLabel);

        shopManager = (ShopManager)EditorGUILayout.ObjectField(
            "ShopManager",
            shopManager,
            typeof(ShopManager),
            true
        );

        EditorGUILayout.Space();
        GUILayout.Label("¿Qué asignar?", EditorStyles.boldLabel);

        assignCharacters = EditorGUILayout.Toggle("Characters", assignCharacters);
        if (assignCharacters)
            charactersPath = EditorGUILayout.TextField("Carpeta Characters", charactersPath);

        EditorGUILayout.Space();

        assignAuras = EditorGUILayout.Toggle("Auras", assignAuras);
        if (assignAuras)
            aurasPath = EditorGUILayout.TextField("Carpeta Auras", aurasPath);

        EditorGUILayout.Space();

        EditorGUILayout.HelpBox(
            "Arrastra el GameObject con ShopManager desde la jerarquía.\n" +
            "Los items se ordenarán por número automáticamente.",
            MessageType.Info);

        GUI.enabled = shopManager != null && (assignCharacters || assignAuras);

        if (GUILayout.Button("Asignar"))
            Assign();

        GUI.enabled = true;
    }

    private void Assign()
    {
        Undo.RecordObject(shopManager, "Assign Items to ShopManager");

        int totalAssigned = 0;

        if (assignCharacters)
        {
            List<CharacterData> characters = LoadAssets<CharacterData>(charactersPath);
            shopManager.characters = characters;
            totalAssigned += characters.Count;
            Debug.Log($"✅ {characters.Count} CharacterData asignados a ShopManager.");
        }

        if (assignAuras)
        {
            List<AuraData> auras = LoadAssets<AuraData>(aurasPath);
            shopManager.auras = auras;
            totalAssigned += auras.Count;
            Debug.Log($"✅ {auras.Count} AuraData asignados a ShopManager.");
        }

        EditorUtility.SetDirty(shopManager);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Listo",
            $"{totalAssigned} items asignados correctamente.", "OK");
    }

    private List<T> LoadAssets<T>(string path) where T : ScriptableObject
    {
        string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { path });
        List<T> result = new();

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T data = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (data != null)
                result.Add(data);
        }

        result.Sort((a, b) =>
        {
            int numA = ExtractNumber(a.name);
            int numB = ExtractNumber(b.name);
            return numA.CompareTo(numB);
        });

        return result;
    }

    private int ExtractNumber(string name)
    {
        string digits = "";
        foreach (char c in name)
            if (char.IsDigit(c))
                digits += c;

        return int.TryParse(digits, out int result) ? result : 0;
    }
}