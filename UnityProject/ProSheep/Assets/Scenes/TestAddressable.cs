using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class TestAddressable : MonoBehaviour
    {
        public string sceneKey = "DLC_TestScene"; // The address you gave the scene in Addressables

        private void Start()
        {
            //LoadScene();
        }

        public async void LoadScene()
        {
            // Load the scene asynchronously
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneKey);

            await handle.Task;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Scene loaded successfully!");
            }
            else
            {
                Debug.LogError("Failed to load scene.");
            }
        }

        // public async void UnloadScene()
        // {
        //     // Unload the scene when needed
        //     AsyncOperationHandle<SceneInstance> handle = Addressables.UnloadSceneAsync(sceneKey);
        //     await handle.Task;
        //     Debug.Log("Scene unloaded.");
        // }
    }
}
