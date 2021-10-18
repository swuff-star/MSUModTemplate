using EntityStates;
using RoR2;
using UnityEditor;
using UnityEngine;

public class EntityStateConfigEditorWindow : ExtendedEditorWindow
{
    Vector2 scrollPos = new Vector2();
    public static void Open(EntityStateConfiguration esc)
    {
        EntityStateConfigEditorWindow window = GetWindow<EntityStateConfigEditorWindow>("Entity State Configuration Editor");
        window.mainSerializedObject = new SerializedObject(esc);
    }

    private void OnGUI()
    {
        var collectionProperty = mainSerializedObject.FindProperty("serializedFieldsCollection");
        var systemTypeProp = mainSerializedObject.FindProperty("targetType");

        mainCurrentProperty = collectionProperty.FindPropertyRelative("serializedFields");

        DrawField(systemTypeProp.FindPropertyRelative("assemblyQualifiedName"), true);

        EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(150), GUILayout.ExpandHeight(true));

        scrollPos = DrawScrollableButtonSidebar(mainCurrentProperty, scrollPos);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));
        if (mainSelectedProperty != null)
        {
            DrawSelectedSerializableFieldPropPanel();
        }
        else
        {
            EditorGUILayout.LabelField("Select a Serializable Field from the List.");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawSelectedSerializableFieldPropPanel()
    {
        mainCurrentProperty = mainSelectedProperty;

        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(500));

        DrawField("fieldName", true);

        GUILayout.Space(30);

        var fieldValueProp = mainSelectedProperty.FindPropertyRelative("fieldValue");

        DrawField(fieldValueProp.FindPropertyRelative("stringValue"), true);
        DrawField(fieldValueProp.FindPropertyRelative("objectValue"), true);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }
}
