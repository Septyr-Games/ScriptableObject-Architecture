using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Septyr.ScriptableObjectArchitecture.Editor
{
    [InitializeOnLoad]
    public class ResetWritableObjects : IPreprocessBuildWithReport
    {
        static ResetWritableObjects()
        {
            EditorApplication.playModeStateChanged += OnStateChange;
        }

        [MenuItem(itemName: "Assets/Reset Writable Objects", priority = 70)]
        public static void Reset()
        {
            string[] variableGuids = AssetDatabase.FindAssets("t:BaseVariable", new string[] { "Assets/Objects" });
            string[] collectionGuids = AssetDatabase.FindAssets("t:BaseCollection", new string[] { "Assets/Objects" });

            if (variableGuids.Length == 0 && collectionGuids.Length == 0)
                return;

            var count = 0;
            foreach (string guid in variableGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                BaseVariable variable = (BaseVariable)AssetDatabase.LoadAssetAtPath(path, typeof(BaseVariable));
                if (!variable.ReadOnly)
                {
                    variable.ResetValue();
                    count++;
                }
            }
            foreach (string guid in collectionGuids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                BaseCollection collection = (BaseCollection)AssetDatabase.LoadAssetAtPath(path, typeof(BaseCollection));
                if (!collection.ReadOnly)
                {
                    collection.Reset();
                    count++;
                }
            }

            if (count > 0)
                Debug.LogFormat("Reset {0} writable variable{1}", count, count == 1 ? "" : "s");
        }

        public int callbackOrder => 0;
        public void OnPreprocessBuild(BuildReport report)
        {
            Reset();
        }

        static void OnStateChange(PlayModeStateChange stateChange)
        {
            if (stateChange == PlayModeStateChange.EnteredEditMode)
                Reset();
        }
    }
}
