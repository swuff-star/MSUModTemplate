/*using UnityEditor;
using UnityEngine;
using LostInTransit.ScriptableObjects;

[CustomEditor(typeof(LITAspectAbility))]
public class MigratorAspectAbility : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(10);
        LITAspectAbility ability = (LITAspectAbility)target;

        if(GUILayout.Button("Migrate to new SIDR"))
        {
            SOMigrator.CreateNewAspectAbility(ability);
        }
    }

}
*/