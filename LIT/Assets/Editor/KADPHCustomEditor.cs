using Moonstorm;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CustomEditor(typeof(KeyAssetDisplayPairHolder))]
public class KADPHCustomEditor : Editor
{
    [OnOpenAsset]
    public static bool OpenEditor(int instanceID, int line)
    {
        KeyAssetDisplayPairHolder obj = EditorUtility.InstanceIDToObject(instanceID) as KeyAssetDisplayPairHolder;
        if (obj != null)
        {
            KADPHEditorWindow.Open(obj);
            return true;
        }
        return false;
    }
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Open Editor"))
        {
            KADPHEditorWindow.Open((KeyAssetDisplayPairHolder)target);
        }
    }
}
