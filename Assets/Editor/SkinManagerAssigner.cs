using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SkinManagerAssigner : EditorWindow
{
    private string folderPath = "Assets/Data/Characters";
    private SkinManagerGlobal skinManager;

    [MenuItem("HeroBird/Assign Characters to SkinManager")]
    public static void ShowWindow()
    {
        GetWindow<SkinManagerAssigner>("Assign to SkinManager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Asignar CharacterData a SkinManagerGlobal", EditorStyles.boldLabel);

        folderPath = EditorGUILayout.TextField("Carpeta Characters", folderPath);

        skinManager = (SkinManagerGlobal)EditorGUILayout.ObjectField(
            "SkinManagerGlobal",
            skinManager,
            typeof(SkinManagerGlobal),
            true // permite arrastrar desde la escena
        );

        EditorGUILayout.HelpBox(
            "Arrastra el GameObject que tiene SkinManagerGlobal desde la jerarquía.\n" +
            "Se cargarán todos los CharacterData de la carpeta y se asignarán en orden.",
            MessageType.Info);

        GUI.enabled = skinManager != null;

        if (GUILayout.Button("Asignar"))
            Assign();

        GUI.enabled = true;
    }

    private void Assign()
    {
        // Carga todos los CharacterData de la carpeta
        string[] guids = AssetDatabase.FindAssets("t:CharacterData", new[] { folderPath });

        if (guids.Length == 0)
        {
            EditorUtility.DisplayDialog("Error", $"No se encontraron CharacterData en:\n{folderPath}", "OK");
            return;
        }

        List<CharacterData> characters = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CharacterData data = AssetDatabase.LoadAssetAtPath<CharacterData>(path);
            if (data != null)
                characters.Add(data);
        }

        // Ordena por nombre para que queden bird1, bird2, bird3...
        characters.Sort((a, b) => 
        {
            int numA = ExtractNumber(a.itemName);
            int numB = ExtractNumber(b.itemName);
            return numA.CompareTo(numB);
        });

        // Asigna al SkinManagerGlobal
        Undo.RecordObject(skinManager, "Assign Characters to SkinManager");
        skinManager.characters = characters;
        EditorUtility.SetDirty(skinManager);

        AssetDatabase.SaveAssets();

        Debug.Log($"✅ {characters.Count} CharacterData asignados a SkinManagerGlobal.");
        EditorUtility.DisplayDialog("Listo", $"{characters.Count} personajes asignados correctamente.", "OK");
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