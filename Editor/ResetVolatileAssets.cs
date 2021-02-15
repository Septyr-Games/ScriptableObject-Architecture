using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Com.Septyr.ScriptableObjectArchitecture.Editor
{
    [InitializeOnLoad]
    public class ResetVolatileAssets : IPreprocessBuildWithReport
    {
        static ResetVolatileAssets()
        {
            EditorApplication.playModeStateChanged += OnStateChange;
        }

        [MenuItem(itemName: "Assets/Reset Volatile Assets", priority = 70)]
        public static void Reset()
        {
            string[] guids = AssetDatabase.FindAssets("t:BaseVariable", new string[] { "Assets/Objects" });

            if (guids.Length == 0)
                return;

            var count = 0;
            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                BaseVariable variable = (BaseVariable)AssetDatabase.LoadAssetAtPath(path, typeof(BaseVariable));
                if (variable.IsVolatile)
                {
                    variable.ResetValue();
                    count++;
                }
            }

            Debug.LogFormat("Reset {0} volatile variable{1}", count, count == 1 ? "" : "s");
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
