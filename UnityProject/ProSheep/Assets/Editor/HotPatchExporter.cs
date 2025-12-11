using System.Collections;
using System.IO;
using System.Linq;
using HybridCLR.Editor.Settings;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
using UnityEngine;

public static class ExportAddressableBundle
{
    [MenuItem("Assets/Export as Addressable Bundle", false, 999)]
    public static void ExportSelectedFolder()
    {
        var obj = Selection.activeObject;
        var path = AssetDatabase.GetAssetPath(obj);

        if (string.IsNullOrEmpty(path) || !AssetDatabase.IsValidFolder(path))
        {
            EditorUtility.DisplayDialog("Export", "Please select a folder.", "OK");
            return;
        }

        var bundleName = Path.GetFileName(path);
        Debug.Log($"[Export] Bundle Folder = {bundleName}");

        try
        {
            EditorUtility.DisplayProgressBar("Exporting...", "Compiling Hotfix DLL...", 0.2f);
            CompileHotfix(path);

            EditorUtility.DisplayProgressBar("Exporting...", "Copy & rename DLL/PDB...", 0.4f);
            MoveHybridClrOutputs(path);

            EditorUtility.DisplayProgressBar("Exporting...", "Preparing Addressable Group...", 0.6f);
            CreateAddressableGroup(path, bundleName);

            EditorUtility.DisplayProgressBar("Exporting...", "Simulating Addressables Build Button...", 0.8f);
            TriggerAddressablesBuild();

            EditorUtility.DisplayProgressBar("Exporting...", "Finished!", 1f);
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }

        Debug.Log("[Export] Completed.");
    }

    private static void CompileHotfix(string bundleFolderPath)
    {
        string scriptsDir = Path.Combine(bundleFolderPath, "Scripts");
        string bundleFolderName = Path.GetFileNameWithoutExtension(bundleFolderPath);
        if (Directory.Exists(scriptsDir))
        {
            string asmdefPath = Path.Combine(scriptsDir, $"{bundleFolderName}.asmdef");
            if (!File.Exists(asmdefPath))
            {
                File.WriteAllText(asmdefPath, $"{{\"name\":\"{bundleFolderName}\",\"autoReferenced\":false}}");
                //File.WriteAllText(asmdefPath, "{  \"name\": \"HotFix\", \"autoReferenced\": false}");
                Debug.Log($"[Export] Auto-created {bundleFolderName}.asmdef");
                AssetDatabase.Refresh();
            }
        }


        HybridCLR.Editor.Commands.CompileDllCommand.CompileDllActiveBuildTarget();
        AddHotUpdateAssemblyToYaml(bundleFolderName);
        Debug.Log($"[Export] HybridCLR hotfix DLL {bundleFolderName} has been compiled.");
    }

    private static void MoveHybridClrOutputs(string bundleFolderPath)
    {
        var settings = HybridCLRSettings.Instance;
        if (settings == null)
        {
            Debug.LogError("HybridCLRSettings asset not found!");
            return;
        }

        string platformFolder = EditorUserBuildSettings.activeBuildTarget switch
        {
            BuildTarget.StandaloneWindows => "StandaloneWindows64",
            BuildTarget.StandaloneWindows64 => "StandaloneWindows64",
            BuildTarget.Android => "Android",
            BuildTarget.iOS => "iOS",
            BuildTarget.StandaloneOSX => "StandaloneOSX",
            BuildTarget.WebGL => "WebGL",
            _ => "UnknownPlatform"
        };

        string bundleFolderName = Path.GetFileNameWithoutExtension(bundleFolderPath);
        string binDir = Path.Combine(settings.hotUpdateDllCompileOutputRootDir, platformFolder);

        string dllSrc = Path.Combine(binDir, $"{bundleFolderName}.dll");
        string pdbSrc = Path.Combine(binDir, $"{bundleFolderName}.pdb");

        string dllDst = Path.Combine(bundleFolderPath, $"{bundleFolderName}.dll.bytes");
        string pdbDst = Path.Combine(bundleFolderPath, $"{bundleFolderName}.pdb.bytes");

        if (File.Exists(dllSrc)) File.Copy(dllSrc, dllDst, true);
        else Debug.LogWarning("[Export] DLL not found: " + dllSrc);

        if (File.Exists(pdbSrc)) File.Copy(pdbSrc, pdbDst, true);
        else Debug.LogWarning("[Export] PDB not found: " + pdbSrc);

        AssetDatabase.Refresh();
    }

    private static void CreateAddressableGroup(string folderPath, string bundleName)
    {
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("[Export] No Addressables settings found!");
            return;
        }

        var group = settings.FindGroup(bundleName);
        if (group == null)
        {
            group = settings.CreateGroup(
                bundleName,
                false, false, true,
                null,
                typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
                typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema)
            );
        }

        group.Settings.buildSettings.cleanupStreamingAssetsAfterBuilds = true;

        group.Settings.BuildRemoteCatalog = true;
        group.GetSchema<BundledAssetGroupSchema>().IncludeInBuild = true;

        var guids = AssetDatabase.FindAssets("", new[] { folderPath });
        foreach (var guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (Directory.Exists(assetPath)) continue;
            if (assetPath.Contains($"{bundleName}/Scripts")) continue;

            var entry = settings.CreateOrMoveEntry(guid, group);
            string fileName = Path.GetFileName(assetPath);

            if (fileName.EndsWith(".unity"))
                entry.address = $"{bundleName}/{fileName}";
            else
                entry.address = $"{bundleName}/{fileName}";
        }
    }

    private static void TriggerAddressablesBuild()
    {
        Debug.Log("[Export] Triggering Addressables build via API…");

        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null)
        {
            Debug.LogError("[Export] No Addressables settings found!");
            return;
        }

        // This uses the build script configured in Addressables Groups window (“Default Build Script”)
        AddressableAssetSettings.BuildPlayerContent();
    }
    
    public static void AddHotUpdateAssemblyToYaml(string assemblyName)
    {
        string path = "ProjectSettings/HybridCLRSettings.asset";

        if (!File.Exists(path))
        {
            Debug.LogError($"HybridCLRSettings.asset not found at {path}");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        bool inHotUpdateSection = false;
        bool alreadyExists = false;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.StartsWith("hotUpdateAssemblies:"))
            {
                inHotUpdateSection = true;
            }
            else if (inHotUpdateSection)
            {
                // If line is a list item
                if (line.StartsWith("- "))
                {
                    string existing = line.Substring(2).Trim();
                    if (existing == assemblyName)
                    {
                        alreadyExists = true;
                        break;
                    }
                }
                else
                {
                    // End of hotUpdateAssemblies section
                    inHotUpdateSection = false;
                }
            }
        }

        if (alreadyExists)
        {
            Debug.Log($"[HybridCLR] Assembly '{assemblyName}' already exists in YAML.");
            return;
        }

        // Find the last line of the hotUpdateAssemblies section
        int insertIndex = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Trim().StartsWith("hotUpdateAssemblies:"))
            {
                insertIndex = i;
                // Find the last item in the list
                int j = i + 1;
                while (j < lines.Length && lines[j].Trim().StartsWith("- "))
                {
                    insertIndex = j;
                    j++;
                }
                break;
            }
        }

        if (insertIndex != -1)
        {
            // Insert new line after the last item
            string indent = "  "; // maintain YAML indentation (usually two spaces)
            string newLine = $"{indent}- {assemblyName}";
            var linesList = new System.Collections.Generic.List<string>(lines);
            linesList.Insert(insertIndex + 1, newLine);
            File.WriteAllLines(path, linesList);
            Debug.Log($"[HybridCLR] Added HotUpdate assembly '{assemblyName}' to HybridCLRSettings.asset.");
        }
        else
        {
            Debug.LogError("Cannot find hotUpdateAssemblies section in HybridCLRSettings.asset.");
        }
    }

    // /// <summary>
    // /// Add a hot-update assembly (based on folder/assembly name) to HybridCLRSettings.
    // /// </summary>
    // /// <param name="folderName">The folder name / assembly name</param>
    // public static void AddHotUpdateAssembly(string assemblyName)
    // {
    //     var settings = HybridCLRSettings.Instance;
    //     if (settings == null)
    //     {
    //         Debug.LogError("HybridCLRSettings asset not found!");
    //         return;
    //     }
    //
    //     var previousArray = HybridCLRSettings.Instance.hotUpdateAssemblies.ToList();
    //     if (previousArray.Exists(s => s == assemblyName))
    //     {
    //         Debug.Log($"[HybridCLR] HotUpdate assembly already existed: {assemblyName}");
    //     }
    //     else
    //     {
    //         previousArray.Add(assemblyName);
    //         HybridCLRSettings.Instance.hotUpdateAssemblies = previousArray.ToArray();
    //         
    //         // Mark dirty so Unity saves changes
    //         EditorUtility.SetDirty(settings);
    //         AssetDatabase.SaveAssets();
    //         
    //         Debug.Log($"[HybridCLR] Added HotUpdate assembly: {assemblyName}");
    //     }
    // }
}