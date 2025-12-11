using Localization;
using ProSheep;
using UnityEditor;
using UnityEngine;

public static class LocalizationContextMenu
{
    [MenuItem("CONTEXT/MonoBehaviour/Add Runtime Localization System")]
    private static void AddLocalizationSystem(MenuCommand command)
    {
        var comp = command.context as MonoBehaviour;
        if (comp != null)
        {
            // Add your LocalizationSystem component to the same GameObject
            comp.gameObject.AddComponent<RuntimeStringLocalization>();
            Debug.Log("LocalizationSystem added to " + comp.gameObject.name);
        }
    }
    [MenuItem("CONTEXT/MonoBehaviour/Refresh Localization System during RUNTIME")]
    
    private static void RefreshLocalizationSystemInRuntime(MenuCommand command)
    {
        var comp = command.context as MonoBehaviour;
        if (comp != null)
        {
            Brightend.DictServiceOnline.RefreshAllString();
        }
    }
}