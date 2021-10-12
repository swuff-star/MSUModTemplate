using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Moonstorm.EditorUtils
{
    /*public class Cum : MonoBehaviour
    {

        [MenuItem("Tools/MoonstormEditorUtils/Cum")]
        static void Cummies()
        {
            Coom();
        }

        private static void Coom()
        {
            List<Shader> shaders = Util.FindAssetsByType<Shader>()
                                       .Select(shader => AssetDatabase.GetAssetPath(shader))
                                       .Where(path => path.Contains(".asset"))
                                       .Select(path => AssetDatabase.LoadAssetAtPath<Shader>(path))
                                       .ToList();

            var so = ScriptableObject.CreateInstance<ShaderHolder>();

            AssetDatabase.CreateAsset(so, "Assets/ShaderHolder.asset");
            foreach(Shader shader in shaders)
            {
                AssetDatabase.AddObjectToAsset(shader, so);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }*/
}
