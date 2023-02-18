#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// This ensures that #NAMESPACE# and other keywords in templates
/// are properly set when creating a script.
/// </summary>
internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor
{
    private static char[] spliters = new char[] { '/', '\\', '.' };
    private static List<string> wordsToDelete = new List<string>() { "Extensions", "Scripts", "Editor" };

    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if (index < 0)
            return;

        string file = path.Substring(index);
        if (file != ".cs" && file != ".js")
            return;

        List<string> namespaces = path.Split(spliters).ToList<string>();
        namespaces = namespaces.GetRange(1, namespaces.Count - 3);
        namespaces = namespaces.Except(wordsToDelete).ToList<string>();

        string namespaceString = "Coots.Globals";
        for (int i = 0; i < namespaces.Count; i++)
        {
            if (i == 0)
                namespaceString = "Coots.";
            namespaceString += namespaces[i];
            if (i < namespaces.Count - 1)
                namespaceString += ".";
        }

        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        if (!System.IO.File.Exists(path))
            return;

        string fileContent = System.IO.File.ReadAllText(path);
        fileContent = fileContent.Replace("#NAMESPACE#", namespaceString);
        System.IO.File.WriteAllText(path, fileContent);
        AssetDatabase.Refresh();
    }
}
#endif