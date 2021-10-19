/*using UnityEditor;
using UnityEngine;
using LostInTransit.ScriptableObjects;

[CustomEditor(typeof(LITSingleItemDisplayRule))]
public class MigratorSIDR : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);
        LITSingleItemDisplayRule sidr = (LITSingleItemDisplayRule)target;

        if(GUILayout.Button("Migrate to new SIDR"))
        {
            SOMigrator.CreateNewSIDR(sidr);
        }
    }
}
*/