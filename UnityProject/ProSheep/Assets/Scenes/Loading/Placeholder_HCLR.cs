using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Scenes.Loading
{
    public class Placeholder_HCLR : MonoBehaviour
    {
        [TextArea] public string originalTypeName; // e.g. "MyNamespace.MyComponent, Assembly-CSharp"

        [TextArea(5, 20)] public string storedJson;

        // ---------------------------
        // Restore during runtime
        // ---------------------------
        public Component Restore()
        {
            if (originalTypeName == "")
            {
                // Skip empty placeholders

                if (Application.isPlaying)
                    Destroy(this);

                return null;
            }

            var targetType = Type.GetType(originalTypeName);
            if (targetType == null)
            {
                Debug.LogWarning($"Restore failed: type not found: {originalTypeName}");
                return null;
            }

            Component newComp = gameObject.AddComponent(targetType);

            try
            {
                JsonUtility.FromJsonOverwrite(storedJson, newComp);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Restore failed for {originalTypeName}: {ex}");
            }

            if (Application.isPlaying)
                Destroy(this);

            return newComp;
        }

        [ContextMenu("Restore All Placeholders in scene")]
        public void TestRestoreAll()
        {
            RestoreAll();
        }

        /// <summary>
        /// Restores ALL Placeholder_HCLR components in ALL loaded scenes.
        /// Works in runtime and editor.
        /// </summary>
        public static int RestoreAll()
        {
            List<Placeholder_HCLR> list = new List<Placeholder_HCLR>();

            // Scan all loaded scenes
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;
                
                foreach (var root in scene.GetRootGameObjects())
                {
                    var comps = root.GetComponentsInChildren<Placeholder_HCLR>(true);
                    list.AddRange(comps);
                    
                    Debug.Log($"Found {comps.Length} placeholders in {root.name}");
                }

            }

            int total = list.Count;

            // Restore in reverse order to avoid hierarchy interference
            for (int i = list.Count - 1; i >= 0; i--)
            {
                list[i].Restore(); // runtime restore function
            }

            Debug.Log($"[PlaceholderRestorer] Restored {total} components across {sceneCount} scenes.");

            return total;
        }

        /// <summary>
        /// Load dll file into HybridCLR
        /// </summary>
        /// <param name="groupName">{groupName}/{groupName}.dll.bytes</param>
        public static async Task LoadDllFile(string groupName)
        {
            var dllKey = $"{groupName}/{groupName}.dll.bytes";
            var pdbKey = $"{groupName}/{groupName}.pdb.bytes";


            var dllBytes = await LoadFileBytes(dllKey);
            var pdbBytes = await LoadFileBytes(pdbKey);

            if (dllBytes == null)
            {
                Debug.LogError($"DLL load failed. \n{dllKey}");
                return;
            }

            Assembly.Load(dllBytes, pdbBytes);
            Debug.Log($"DLL load successfully. \n{dllKey}");
        }

        /// <summary>
        /// Load file bytes through Addressable. 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static async Task<byte[]> LoadFileBytes(string key)
        {
            // Addressables stores text/binary files as TextAsset by default
            var handle = Addressables.LoadAssetAsync<TextAsset>(key);
            await handle.Task;

            if (!handle.IsValid())
            {
                Debug.LogError($"[HotfixLoader] Failed to load '{key}' from Addressables.");
                return null;
            }

            byte[] bytes = handle.Result.bytes;
            return bytes;
        }
    }
}