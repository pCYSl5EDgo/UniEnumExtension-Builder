using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using PackageInfo = UnityEditor.PackageInfo;

public static class CreateUnityPackage
{
    public static void PrintVersion()
    {
        File.WriteAllText(Application.dataPath + "/../../version.txt", PlayerSettings.bundleVersion);
    }
    
    public static void UpdatePackageVersion()
    {
        var path = Application.dataPath + "/Plugins/UEE/package.json";
        var packageJson = JsonUtility.FromJson<PackageJson>(File.ReadAllText(path));
        packageJson.version = PlayerSettings.bundleVersion;
        File.WriteAllText(path, packageJson.ToString());
    }

    public static void IncrementBuildVersion()
    {
        var bundleVersion = PlayerSettings.bundleVersion.Split('.');
        ref var buildVersion = ref bundleVersion[bundleVersion.Length - 1];
        if (!int.TryParse(buildVersion, out var buildNumber))
        {
            Console.Error.WriteLine(buildVersion + " is not valid build number : " + bundleVersion);
        }
        buildVersion = (buildNumber + 1).ToString();
        bundleVersion.ToBundleVersion();
    }

    private static void ToBundleVersion(this IReadOnlyList<string> bundleVersion)
    {
        PlayerSettings.bundleVersion = bundleVersion.Skip(1).Aggregate(new StringBuilder(bundleVersion[0]), (builder, str) => builder.Append('.').Append(str), builder => builder.ToString());
    }

    public static void IncrementMinorVersionAndResetBuildVersion()
    {
        var bundleVersion = PlayerSettings.bundleVersion.Split('.');
        ref var buildVersion = ref bundleVersion[bundleVersion.Length - 1];
        ref var minorVersion = ref bundleVersion[bundleVersion.Length - 2];
        if (!int.TryParse(minorVersion, out var minorNumber))
        {
            Console.Error.WriteLine(minorNumber + " is not valid " + nameof(minorNumber) + " : " + bundleVersion);
        }
        buildVersion = "0";
        minorVersion = (minorNumber + 1).ToString();
        bundleVersion.ToBundleVersion();
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