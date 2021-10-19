/*using UnityEditor;
using UnityEngine;
using LostInTransit.ScriptableObjects;

[CustomEditor(typeof(LITEliteDef))]
public class MigratorLITEliteDef : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);
        LITEliteDef eliteDef = (LITEliteDef)target;

        if(GUILayout.Button("Migrate to new SIDR"))
        {
            SOMigrator.CreateNewEliteDef(eliteDef);
        }
    }
}
*/