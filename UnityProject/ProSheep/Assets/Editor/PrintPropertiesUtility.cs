using System;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public static class PrintPropertiesUtility
{
    [MenuItem("CONTEXT/Component/Print Properties&Fields Whitelist Code")]
    private static void PrintPropertiesWhitelistCode(MenuCommand command)
    {
        Component comp = (Component)command.context;
        System.Type type = comp.GetType();

        var names = new List<string>();

        // Collect safe properties
        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!prop.CanRead || prop.GetIndexParameters().Length > 0) continue;
            if (typeof(UnityEngine.Object).IsAssignableFrom(prop.PropertyType)) continue;
            if (Attribute.IsDefined(prop, typeof(System.ObsoleteAttribute)) ||
                Attribute.IsDefined(prop.GetMethod, typeof(System.ObsoleteAttribute)))
                continue;

            names.Add($"\"{prop.Name}\"");
        }

        // Collect public fields
        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType)) continue;
            if (Attribute.IsDefined(field, typeof(System.ObsoleteAttribute))) continue;

            names.Add($"\"{field.Name}\"");
        }

        // Build whitelist entry
        string whitelistEntry =
            $"{{ typeof({type.Name}), new HashSet<string> {{ {string.Join(", ", names)} }} }},";

        Debug.Log(whitelistEntry);
    }
}