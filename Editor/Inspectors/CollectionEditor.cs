using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditorInternal;
using UnityEditor.AnimatedValues;

namespace Septyr.ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseCollection), true)]
    public class CollectionEditor : UnityEditor.Editor
    {
        private BaseCollection Target => (BaseCollection)target;
        private SerializedProperty CollectionItemsProperty => serializedObject.FindProperty(LIST_PROPERTY_NAME);

        private SerializedProperty _readOnly;
        private SerializedProperty _raiseWarning;
        private SerializedProperty _isFixedSize;
        private SerializedProperty _fixedSize;
        private AnimBool _fixedSizeAnimation;
        private AnimBool _readOnlyValuesAnimation;
        private AnimBool _raiseWarningAnimation;
        private ReorderableList _reorderableList;

        // UI
        private const bool DISABLE_ELEMENTS = false;
        private const bool ELEMENT_DRAGGABLE = true;
        private const bool LIST_DISPLAY_HEADER = true;
        private const bool LIST_DISPLAY_ADD_BUTTON = false;
        private const bool LIST_DISPLAY_REMOVE_BUTTON = true;

        private GUIContent _titleGUIContent;
        private GUIContent _noPropertyDrawerWarningGUIContent;

        private const string TITLE_FORMAT = "List ({0})";
        private const string NO_PROPERTY_WARNING_FORMAT = "No PropertyDrawer for type [{0}]";

        // Property Names
        private const string LIST_PROPERTY_NAME = "_list";

        private const string READONLY_TOOLTIP = "This allows for a definition in the editor. When disabled (and in \"Objects\" directory), "
            + "it will not store data between playtesting or builds.";

        private void OnEnable()
        {
            _readOnly = serializedObject.FindProperty("_readOnly");
            _raiseWarning = serializedObject.FindProperty("_raiseWarning");
            _isFixedSize = serializedObject.FindProperty("_isFixedSize");
            _fixedSize = serializedObject.FindProperty("_fixedSize");

            _fixedSizeAnimation = new AnimBool(_isFixedSize.boolValue);
            _fixedSizeAnimation.valueChanged.AddListener(Repaint);

            _readOnlyValuesAnimation = new AnimBool(_readOnly.boolValue);
            _readOnlyValuesAnimation.valueChanged.AddListener(Repaint);

            _raiseWarningAnimation = new AnimBool(_readOnly.boolValue);
            _raiseWarningAnimation.valueChanged.AddListener(Repaint);

            _titleGUIContent = new GUIContent(string.Format(TITLE_FORMAT, Target.Type));
            _noPropertyDrawerWarningGUIContent = new GUIContent(string.Format(NO_PROPERTY_WARNING_FORMAT, Target.Type));

            _reorderableList = new ReorderableList(
                serializedObject,
                CollectionItemsProperty,
                ELEMENT_DRAGGABLE,
                LIST_DISPLAY_HEADER,
                LIST_DISPLAY_ADD_BUTTON,
                LIST_DISPLAY_REMOVE_BUTTON)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                elementHeightCallback = GetHeight,
            };
        }
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();

            DrawReadonlyField();
            EditorGUILayout.Space();

            DrawFixedSizeField();

            _readOnlyValuesAnimation.target = _readOnly.boolValue;
            using (var group = new EditorGUILayout.FadeGroupScope(_readOnlyValuesAnimation.faded))
            {
                if (group.visible)
                {
                    if (_isFixedSize.boolValue)
                        CollectionItemsProperty.arraySize = _fixedSize.intValue;
                    else
                        _fixedSize.intValue = CollectionItemsProperty.arraySize;
                    _reorderableList.DoLayoutList();
                    _reorderableList.displayAdd = !_isFixedSize.boolValue;
                    _reorderableList.displayRemove = !_isFixedSize.boolValue;
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
        protected void DrawReadonlyField()
        {
            EditorGUILayout.PropertyField(_readOnly, new GUIContent("Read Only", READONLY_TOOLTIP));

            _raiseWarningAnimation.target = _readOnly.boolValue;
            using (var group = new EditorGUILayout.FadeGroupScope(_raiseWarningAnimation.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_raiseWarning);
                    EditorGUI.indentLevel--;
                }
            }
        }
        private void DrawFixedSizeField()
        {
            EditorGUILayout.PropertyField(_isFixedSize, new GUIContent("Is Fixed Size"));

            _fixedSizeAnimation.target = _isFixedSize.boolValue;
            using (var group = new EditorGUILayout.FadeGroupScope(_fixedSizeAnimation.faded))
            {
                if (group.visible)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(_fixedSize);
                    EditorGUI.indentLevel--;
                }
            }
        }
        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, _titleGUIContent);
        }
        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect = SOArchitecture_EditorUtility.GetReorderableListElementFieldRect(rect);
            SerializedProperty property = CollectionItemsProperty.GetArrayElementAtIndex(index);

            EditorGUI.BeginDisabledGroup(DISABLE_ELEMENTS);

            GenericPropertyDrawer.DrawPropertyDrawer(rect, property, Target.Type);

            EditorGUI.EndDisabledGroup();
        }
        private float GetHeight(int index)
        {
            SerializedProperty property = CollectionItemsProperty.GetArrayElementAtIndex(index);

            return GenericPropertyDrawer.GetHeight(property, Target.Type) + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}