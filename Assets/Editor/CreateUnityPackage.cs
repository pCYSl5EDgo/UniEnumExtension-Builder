using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor;
using UnityEditor;
using UnityEngine;

public static class CreateUnityPackage
{
    public static void IncrementBuildVersion()
    {
        var path = Application.dataPath + "/Plugins/UEE/package.json";
        var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(path));
        var bundleVersion = packageJson.version.Split('.');
        ref var buildVersion = ref bundleVersion[bundleVersion.Length - 1];
        if (!int.TryParse(buildVersion, out var buildNumber))
        {
            Console.Error.WriteLine(buildVersion + " is not valid build number : " + bundleVersion);
        }
        buildVersion = (buildNumber + 1).ToString();
        packageJson.ToBundleVersion(bundleVersion);
        File.WriteAllText(path, packageJson.ToString());
    }

    private static void ToBundleVersion(this ref PackageJson packageJson, IReadOnlyList<string> bundleVersion)
    {
        packageJson.version = bundleVersion.Skip(1).Aggregate(new StringBuilder(bundleVersion[0]), (builder, str) => builder.Append('.').Append(str), builder => builder.ToString());
    }

    public static void CreateBOOTH()
    {
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
            .Concat(new[]
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
            .Concat(new[]
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