using System;
using System.Collections;
using ProSheep;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scenes.Loading
{
    public class LoadingManager : MonoBehaviour
    {
        private static bool _addressableUrlUpdated = false;
        private static AsyncOperationHandle<SceneInstance> _loadingSceneHandle; // Unload scene with this handle. 
        private static Coroutine _loadingCoroutine;
        public static bool IsLoadingScene => _loadingCoroutine != null;
        public static bool IsNetworkedSceneLoaded => _loadingSceneHandle.IsValid();
        private static string remoteCatalogUrl = $"{FreeDownloadManager.ServerURL}v1/files/download_gameassets/dlcs/catalog_1.0.json";

        private static LoadingManager _instance;

        private void Start()
        {
            // 每次使用 Addressable 下载前去更新 Addressable 后端地址，因此你只需要修改 ServerURL 就可以完成所有事情
            if (_addressableUrlUpdated == false)
            {
                Addressables.LoadContentCatalogAsync(remoteCatalogUrl);
                Addressables.InitializeAsync();
                Debug.Log("Remote catalog loaded from: " + remoteCatalogUrl);
                
                Addressables.InternalIdTransformFunc = (IResourceLocation loc) =>
                {
                    // Only override for remote bundles
                    if (loc.Data is AssetBundleRequestOptions)
                    {
                        try
                        {
                            string original = loc.ToString();
                            string fileName = System.IO.Path.GetFileName(original);
                            var downloadPath = new Uri(new Uri(FreeDownloadManager.ServerDLCDownloadDictionary()), fileName).ToString();
                            Debug.Log($"Addressable trying to start download: \n{downloadPath}");
                            return downloadPath;
                        }
                        catch (Exception e)
                        {
                            Debug.Log($"Addressable Custom Url Error: /n{e}");
                        }
                    }
                    else
                    {
                        // Debug.Log($"TRANSFORM: type={loc.ResourceType}  id={loc.InternalId}");
                        //
                        // if (loc.Data is AssetBundleRequestOptions)
                        // {
                        //     Debug.Log("  → This is a bundle, transforming URL.");
                        // }
                    }
                
                    return loc.InternalId;
                };
                
                _addressableUrlUpdated = true;
            }
            
            
            
            
            
            
            
            // Debug.LogWarning($"TEST");
            //TestLoadNetScene();
        }

        private void OnEnable()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this);
            }
        }

        private static LoadingManager CheckValidate()
        {
            if (_instance == null)
            {
                var newGo = new GameObject("LoadingManager");
                _instance = newGo.AddComponent<LoadingManager>();
            }

            return _instance;
        }

        [ContextMenu("Test Load networked Scene")]
        public void TestLoadNetScene()
        {
            LoadScene("DLC_TestScene");
        }

        [ContextMenu("Test Load local Scene")]
        public void TestLoadLocalScene()
        {
            LoadScene("TestScene");
        }

        /// <summary>
        /// Go to Loading scene and start load logic.
        /// Support both Networked scene and local scene. 
        /// </summary>
        /// <param name="groupNameOrLocalSceneName">{groupName}/{groupName}.unity OR local scene name</param>
        /// <param name="callback"></param>
        public static void LoadScene(string groupNameOrLocalSceneName,
            UnityAction<bool> callback = null, UnityAction<float> callbackProgress = null)
        {
            // Networked Scene is based on additive SE_Loading scene. 
            if (SceneManager.GetActiveScene().name != "SE_Loading")
            {
                SceneManager.LoadScene("SE_Loading");
            }
            
            var isLocalScene = IsSceneInBuild(groupNameOrLocalSceneName);

            if (isLocalScene)
            {
                // Try to unload last networked scene
                CheckValidate().StartCoroutine(IE_UnloadScene((() =>
                {
                    try
                    {
                        SceneManager.LoadScene(groupNameOrLocalSceneName);
                        ToggleLoadingStuffs(false);
                        Debug.Log($"Local scene {groupNameOrLocalSceneName} has been loaded.");
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Load scene {groupNameOrLocalSceneName} failed: \n{e}");
                        callback?.Invoke(false);
                    }

                    Placeholder_HCLR.RestoreAll();
                    callback?.Invoke(true);
                })));
            }
            else
            {
                if (IsLoadingScene)
                {
                    Debug.LogWarning($"Can't load scene {groupNameOrLocalSceneName} because there is already a loading scene. ");
                    return;
                }
                else
                {
                    _loadingCoroutine = CheckValidate()
                        .StartCoroutine(IE_LoadNetworkedScene(groupNameOrLocalSceneName, callback, callbackProgress));
                }
            }
        }

        private static IEnumerator IE_LoadNetworkedScene(string groupName,
            UnityAction<bool> callback = null, UnityAction<float> callbackProgress = null)
        {
            // Show loading stuffs, like loading images or canvas, all the Loading Game Object's children. 
            ToggleLoadingStuffs(true);
            
            if (IsNetworkedSceneLoaded)
            {
                yield return IE_UnloadScene();
            }
            
            
            // Load remote catalog
            yield return Addressables.InitializeAsync();
            var loadCatalog = Addressables.LoadContentCatalogAsync(remoteCatalogUrl);
            yield return loadCatalog;

            if (loadCatalog.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Failed to load remote catalog before change level: " + remoteCatalogUrl);
                yield break;
            }
            
            _loadingSceneHandle =
                Addressables.LoadSceneAsync($"{groupName}/{groupName}.unity",LoadSceneMode.Additive);
            _loadingSceneHandle.Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                    Debug.Log($"Networked scene {groupName} has been loaded.");
                if (handle.Status == AsyncOperationStatus.Failed)
                    Debug.LogWarning($"Load networked scene {groupName} has failed!");
            };
            
            callbackProgress?.Invoke(0f);
            while (!_loadingSceneHandle.IsDone)
            {
                callbackProgress?.Invoke(_loadingSceneHandle.PercentComplete);
                yield return null;
            }
            callbackProgress?.Invoke(1f);

            var isSucceed = _loadingSceneHandle.Status == AsyncOperationStatus.Succeeded;
            if (isSucceed)
            {
                var dllTask = Placeholder_HCLR.LoadDllFile(groupName);
                while (!dllTask.IsCompleted)
                    yield return null;
            
                Placeholder_HCLR.RestoreAll();
            }
            
            _loadingCoroutine = null;
            ToggleLoadingStuffs(false);
            callback?.Invoke(isSucceed);
        }

        private static IEnumerator IE_UnloadScene(UnityAction callback = null)
        {
            if (IsNetworkedSceneLoaded)
            {
                var sceneName = _loadingSceneHandle.Result.Scene.name;
                var h = Addressables.UnloadSceneAsync(_loadingSceneHandle);
                while (!h.IsDone)
                {
                    yield return null;
                }

                Debug.Log($"Networked scene {sceneName} has been unloaded.");
                _loadingSceneHandle = new AsyncOperationHandle<SceneInstance>();
            }

            callback?.Invoke();

            yield break;
        }

        /// <summary>
        /// Detect whether a scene is in Local. 
        /// </summary>
        /// <param name="sceneName"></param>
        /// <returns></returns>
        private static bool IsSceneInBuild(string sceneName)
        {
            int sceneCount = SceneManager.sceneCountInBuildSettings;
            for (int i = 0; i < sceneCount; i++)
            {
                string path = SceneUtility.GetScenePathByBuildIndex(i);
                string name = System.IO.Path.GetFileNameWithoutExtension(path);
                if (name == sceneName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Enable active or Disable active for the children of LoadingManager Game Object.
        /// </summary>
        public static void ToggleLoadingStuffs(bool isActive)
        {
            var go = CheckValidate();
            for (var i = 0; i < go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }
    }
}