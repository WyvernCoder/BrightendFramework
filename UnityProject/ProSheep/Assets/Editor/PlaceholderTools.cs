#if UNITY_EDITOR
using System;
using Scenes.Loading;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class PlaceholderTools
    {
        [MenuItem("CONTEXT/Component/Convert To Placeholder")]
        private static void ConvertToPlaceholder(MenuCommand cmd)
        {
            if (cmd.context.GetType().AssemblyQualifiedName == typeof(Placeholder_HCLR).AssemblyQualifiedName)
            {
                RestorePlaceholder(cmd);
                return;
            }
            
            Component comp = (Component)cmd.context;
            GameObject go = comp.gameObject;

            var t = comp.GetType();
            string typeName = t.AssemblyQualifiedName;

            // Serialize component
            string json = JsonUtility.ToJson(comp);

            Undo.RecordObject(go, "Convert To Placeholder");

            // Add placeholder
            var placeholder = go.AddComponent<Placeholder_HCLR>();
            placeholder.originalTypeName = typeName;
            placeholder.storedJson = json;

            // Remove original
            Undo.DestroyObjectImmediate(comp);

            Debug.Log($"Converted {t.Name} to Placeholder.");
        }
        
        private static void RestorePlaceholder(MenuCommand cmd)
        {
            var placeholder = cmd.context as Placeholder_HCLR;
            if (placeholder == null)
            {
                Debug.LogError("Context is not a Placeholder_HCLR");
                return;
            }

            Undo.RegisterFullObjectHierarchyUndo(placeholder.gameObject, "Restore Placeholder");

            var restored = placeholder.Restore(); // <--- runtime-safe

            if (restored != null)
            {
                Debug.Log($"[Placeholder] Restored to component: {restored.GetType().Name}");
                Undo.DestroyObjectImmediate(cmd.context);
            }
            else
            {
                Debug.LogError("[Placeholder] Failed to restore. See previous errors.");
            }
        }
    }
}
#endif