using UnityEditor;
using UnityEngine;

public class ExtendedEditorWindow : EditorWindow
{
    protected SerializedObject mainSerializedObject;
    protected SerializedProperty mainCurrentProperty;

    private string mainSelectedPropertyPath;
    protected SerializedProperty mainSelectedProperty;

    protected void DrawProperties(SerializedProperty property, bool drawChildren)
    {
        string lastPropPath = string.Empty;
        foreach(SerializedProperty prop in property)
        {
            if(prop.isArray && prop.propertyType == SerializedPropertyType.Generic)
            {
                EditorGUILayout.BeginHorizontal();
                prop.isExpanded = EditorGUILayout.Foldout(prop.isExpanded, prop.displayName);
                EditorGUILayout.EndHorizontal();

                if(prop.isExpanded)
                {
                    EditorGUI.indentLevel++;
                    DrawProperties(prop, drawChildren);
                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                if(!string.IsNullOrEmpty(lastPropPath) && prop.propertyPath.Contains(lastPropPath))
                {
                    continue;
                }
                lastPropPath = prop.propertyPath;
                EditorGUILayout.PropertyField(prop, drawChildren);
            }
        }
    }

    #region Button Sidebars
    protected void DrawButtonSidebar(SerializedProperty property)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        foreach (SerializedProperty prop in property)
        {
            if (GUILayout.Button(prop.displayName))
            {
                mainSelectedPropertyPath = prop.propertyPath;
            }
        }
        if (!string.IsNullOrEmpty(mainSelectedPropertyPath))
        {
            mainSelectedProperty = mainSerializedObject.FindProperty(mainSelectedPropertyPath);
        }
    }

    protected Vector2 DrawScrollableButtonSidebar(SerializedProperty property, Vector2 scrollPosition)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(300));

        if(property.arraySize != 0)
        {
            foreach (SerializedProperty prop in property)
            {
                if (GUILayout.Button(prop.displayName))
                {
                    mainSelectedPropertyPath = prop.propertyPath;
                }
            }
            if (!string.IsNullOrEmpty(mainSelectedPropertyPath))
            {
                mainSelectedProperty = mainSerializedObject.FindProperty(mainSelectedPropertyPath);
            }

        }
        else
        {
            EditorGUILayout.LabelField($"Increase {property.name}'s Size.");
        }
        EditorGUILayout.EndScrollView();
        return scrollPosition;
    }

    protected void DrawButtonSidebar(SerializedProperty property, string buttonName)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        foreach (SerializedProperty prop in property)
        {
            var p = prop.FindPropertyRelative(buttonName);
            if (p != null && p.objectReferenceValue)
            {
                if (p.objectReferenceValue && GUILayout.Button(p.objectReferenceValue.name))
                {
                    mainSelectedPropertyPath = prop.propertyPath;
                }
            }
            else if (GUILayout.Button(prop.displayName))
            {
                mainSelectedPropertyPath = prop.propertyPath;
            }
        }
        if (!string.IsNullOrEmpty(mainSelectedPropertyPath))
        {
            mainSelectedProperty = mainSerializedObject.FindProperty(mainSelectedPropertyPath);
        }
    }

    protected Vector2 DrawScrollableButtonSidebar(SerializedProperty property, Vector2 scrollPosition, string buttonName)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(300));

        if (property.arraySize != 0)
        {
            foreach (SerializedProperty prop in property)
            {
                var p = prop.FindPropertyRelative(buttonName);
                if(p != null && p.objectReferenceValue)
                {
                    if(p.objectReferenceValue && GUILayout.Button(p.objectReferenceValue.name))
                    {
                        mainSelectedPropertyPath = prop.propertyPath;
                    }
                }
                else if(GUILayout.Button(prop.displayName))
                {
                    mainSelectedPropertyPath = prop.propertyPath;
                }
            }
            if (!string.IsNullOrEmpty(mainSelectedPropertyPath))
            {
                mainSelectedProperty = mainSerializedObject.FindProperty(mainSelectedPropertyPath);
            }

        }
        else
        {
            EditorGUILayout.LabelField($"Increase {property.name}'s Size.");
        }
        EditorGUILayout.EndScrollView();
        return scrollPosition;
    }

    protected void DrawButtonSidebar(SerializedProperty property, ref string selectedPropPath, ref SerializedProperty selectedProperty)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        foreach (SerializedProperty prop in property)
        {
            if (GUILayout.Button(prop.displayName))
            {
                selectedPropPath = prop.propertyPath;
            }
        }
        Debug.Log(selectedPropPath);
        if (selectedPropPath != string.Empty)
        {
            selectedProperty = mainSerializedObject.FindProperty(selectedPropPath);
        }
    }

    protected Vector2 DrawScrollableButtonSidebar(SerializedProperty property, Vector2 scrollPosition, ref string selectedPropPath, ref SerializedProperty selectedProperty)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(300));

        foreach (SerializedProperty prop in property)
        {
            if (GUILayout.Button(prop.displayName))
            {
                selectedPropPath = prop.propertyPath;
            }
        }
        if (!string.IsNullOrEmpty(selectedPropPath))
        {
            selectedProperty = mainSerializedObject.FindProperty(selectedPropPath);
        }
        EditorGUILayout.EndScrollView();
        return scrollPosition;
    }

    protected void DrawButtonSidebar(SerializedProperty property, string propertyNameForButton, ref string selectedPropPath, ref SerializedProperty selectedProperty)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        foreach (SerializedProperty prop in property)
        {
            var p = prop.FindPropertyRelative(propertyNameForButton);
            if (p != null && p.objectReferenceValue)
            {
                if (p.objectReferenceValue && GUILayout.Button(p.objectReferenceValue.name))
                {
                    selectedPropPath = prop.propertyPath;
                }
            }
            else if (GUILayout.Button(prop.displayName))
            {
                selectedPropPath = prop.propertyPath;
            }
        }
        Debug.Log(selectedPropPath);
        if (selectedPropPath != string.Empty)
        {
            selectedProperty = mainSerializedObject.FindProperty(selectedPropPath);
        }
    }

    protected Vector2 DrawScrollableButtonSidebar(SerializedProperty property, Vector2 scrollPosition, string propertyNameForButton, ref string selectedPropPath, ref SerializedProperty selectedProperty)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true, GUILayout.Width(300));

        foreach (SerializedProperty prop in property)
        {
            var p = prop.FindPropertyRelative(propertyNameForButton);
            if (p != null && p.objectReferenceValue)
            {
                if (p.objectReferenceValue && GUILayout.Button(p.objectReferenceValue.name))
                {
                    selectedPropPath = prop.propertyPath;
                }
            }
            else if (GUILayout.Button(prop.displayName))
            {
                selectedPropPath = prop.propertyPath;
            }
        }
        if (!string.IsNullOrEmpty(selectedPropPath))
        {
            selectedProperty = mainSerializedObject.FindProperty(selectedPropPath);
        }
        EditorGUILayout.EndScrollView();
        return scrollPosition;
    }
    #endregion

    #region Value Sidebars
    protected void DrawValueSidebar(SerializedProperty property)
    {
        property.arraySize = EditorGUILayout.DelayedIntField($"Array Size", property.arraySize);

        foreach (SerializedProperty prop in property)
        {
            DrawField(prop, true);
        }
    }
    #endregion

    protected void DrawField(string propName, bool relative)
    {
        if(relative && mainCurrentProperty != null)
        {
            EditorGUILayout.PropertyField(mainCurrentProperty.FindPropertyRelative(propName), true);
        }
        else if(mainSerializedObject != null)
        {
            EditorGUILayout.PropertyField(mainSerializedObject.FindProperty(propName), true);
        }
    }
    
    protected void DrawField(SerializedProperty property, bool includeChildren)
    {
        EditorGUILayout.PropertyField(property, includeChildren);
    }

    protected void DrawField(string propName, bool relative, SerializedProperty currentProp, SerializedObject serializedObj)
    {
        if (relative && currentProp != null)
        {
            EditorGUILayout.PropertyField(currentProp.FindPropertyRelative(propName), true);
        }
        else if (mainSerializedObject != null)
        {
            EditorGUILayout.PropertyField(serializedObj.FindProperty(propName), true);
        }
    }

    protected void ApplyChanges()
    {
        mainSerializedObject.ApplyModifiedProperties();
    }
}
