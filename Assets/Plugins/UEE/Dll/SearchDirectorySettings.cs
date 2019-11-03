using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UniEnumExtension
{
    public sealed class SearchDirectorySettings : ScriptableObject
    {
        [SerializeField] public bool EditorNotIncludeEngineCoreModuleDirectory;
        [SerializeField] public PlatformSpecific EditorSpecificSetting;
        [SerializeField] public PlatformSpecific[] PlatformSpecificSettings;

        private static SearchDirectorySettings instance;

        public string[] GetEditorSearchingDirectories()
        {
            if (EditorNotIncludeEngineCoreModuleDirectory)
                return EditorSpecificSetting.SearchingDirectory;
            return new[]
            {
                Path.GetDirectoryName(InternalEditorUtility.GetEngineCoreModuleAssemblyPath()),
                Path.GetDirectoryName(InternalEditorUtility.GetEngineAssemblyPath()),
                Path.GetDirectoryName(InternalEditorUtility.GetEditorAssemblyPath()),
            }.Concat(EditorSpecificSetting.SearchingDirectory).Distinct().ToArray();
        }

        public static SearchDirectorySettings Instance
        {
            get
            {
                if (instance != null) goto RETURN;
                const string Assets = nameof(Assets);
                const string Plugins = nameof(Plugins);
                const string Assets_Plugins = Assets + "/" + Plugins;
                const string UEE = nameof(UEE);
                const string Assets_Plugins_UEE = Assets_Plugins + "/" + UEE;
                const string Dll = nameof(Dll);
                const string Assets_Plugins_UEE_Dll = Assets_Plugins_UEE + "/" + Dll;
                const string assetPath = Assets_Plugins_UEE_Dll + "/" + nameof(SearchDirectorySettings) + ".asset";
                instance = AssetDatabase.LoadAssetAtPath<SearchDirectorySettings>(assetPath);
                if (instance != null) goto RETURN;

                if (!AssetDatabase.IsValidFolder(Assets_Plugins_UEE_Dll))
                {
                    if (!AssetDatabase.IsValidFolder(Assets_Plugins_UEE))
                    {
                        if (!AssetDatabase.IsValidFolder(Assets_Plugins))
                        {
                            AssetDatabase.CreateFolder(Assets, Plugins);
                        }
                        AssetDatabase.CreateFolder(Assets_Plugins, UEE);
                    }
                    AssetDatabase.CreateFolder(Assets_Plugins_UEE, Dll);
                }

                instance = CreateInstance<SearchDirectorySettings>();
                AssetDatabase.CreateAsset(instance, assetPath);
                RETURN:
                instance.Initialize();
                return instance;
            }
        }

        private void Initialize()
        {
            if (PlatformSpecificSettings == null)
            {
                PlatformSpecificSettings = Array.Empty<PlatformSpecific>();
            }
            if (EditorSpecificSetting.SearchingDirectory == null)
            {
                EditorSpecificSetting.SearchingDirectory = Array.Empty<string>();
            }
            for (var i = 0; i < PlatformSpecificSettings.Length; i++)
            {
                ref var setting = ref PlatformSpecificSettings[i];
                if (setting.SearchingDirectory != null) continue;
                setting.SearchingDirectory = Array.Empty<string>();
            }
        }

        [Serializable]
        public struct PlatformSpecific : IComparable<PlatformSpecific>
        {
            public BuildTarget BuildTarget;
            public string[] SearchingDirectory;

            public int CompareTo(PlatformSpecific other) => BuildTarget.CompareTo(other.BuildTarget);

            public string[] ToArray()
            {
                return SearchingDirectory;
            }
        }
    }
}