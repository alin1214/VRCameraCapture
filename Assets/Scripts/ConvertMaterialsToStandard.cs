using UnityEditor;
using UnityEngine;

public class ConvertMaterialsToStandard : EditorWindow
{
    [MenuItem("Tools/Convert All Materials To Standard Shader")]
    static void Convert()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        Shader standard = Shader.Find("Standard");

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null)
            {
                mat.shader = standard;
                EditorUtility.SetDirty(mat);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("All materials converted to Standard Shader!");
    }
}

