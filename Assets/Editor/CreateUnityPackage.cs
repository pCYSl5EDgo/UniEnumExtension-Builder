using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CreateUnityPackage
{
    public static void CreateBOOTH()
    {
        // configure
        const string root = "Plugins/UEE";
        var exportPath = Path.Combine(Application.dataPath + "/", "../../artifact_booth/UniEnumExtension.unitypackage");

        var path = Path.Combine(Application.dataPath, root);
        var dataPath = Application.dataPath + "/Plugins/UEE/";
        var assets = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
            .Where(x =>
            {
                var extension = Path.GetExtension(x);
                return extension == ".cs" || extension == ".asset" || extension == ".bytes" || extension == ".asmdef";
            })
            .Select(x => "Assets" + x.Replace(Application.dataPath, "").Replace(@"\", "/"))
            .Concat(new []
            {
                dataPath + "LICENSE-Commercial",
                dataPath + "README",
                dataPath + "README.jp",
            })
            .ToArray();

        Debug.Log("Export below files" + Environment.NewLine + string.Join(Environment.NewLine, assets));

        AssetDatabase.ExportPackage(
            assets,
            exportPath,
            ExportPackageOptions.Default);

        Debug.Log("Export complete: " + Path.GetFullPath(exportPath));
    }

    public static void CreateGplV3()
    {
        // configure
        const string root = "Plugins/UEE";
        var exportPath = Path.Combine(Application.dataPath + "/", "../../artifact_gplv3/UniEnumExtension.unitypackage");

        var path = Path.Combine(Application.dataPath, root);
        var dataPath = Application.dataPath + "/Plugins/UEE/";
        var assets = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)
            .Where(x =>
            {
                var extension = Path.GetExtension(x);
                return extension == ".cs" || extension == ".asset" || extension == ".bytes" || extension == ".asmdef";
            })
            .Select(x => "Assets" + x.Replace(Application.dataPath, "").Replace(@"\", "/"))
            .Concat(new []
            {
                dataPath + "LICENSE-GPLv3",
                dataPath + "package.json",
                dataPath + "README",
                dataPath + "README.jp",
            })
            .ToArray();

        Debug.Log("Export below files" + Environment.NewLine + string.Join(Environment.NewLine, assets));

        AssetDatabase.ExportPackage(
            assets,
            exportPath,
            ExportPackageOptions.Default);

        Debug.Log("Export complete: " + Path.GetFullPath(exportPath));
    }
}