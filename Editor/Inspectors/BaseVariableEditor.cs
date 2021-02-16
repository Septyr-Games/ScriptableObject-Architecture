using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace Com.Septyr.ScriptableObjectArchitecture.Editor
{
    [CustomEditor(typeof(BaseVariable<>), true)]
    public class BaseVariableEditor : UnityEditor.Editor
    {
        private BaseVariable Target { get { return (BaseVariable)target; } }
        protected bool IsClampable { get { return Target.Clampable; } }
        protected bool IsClamped { get { return Target.IsClamped; } }

        private SerializedProperty _isVolatile;
        private SerializedProperty _valueProperty;
        private SerializedProperty _readOnly;
        private SerializedProperty _raiseWarning;
        private SerializedProperty _isClamped;
        private SerializedProperty _minValueProperty;
        private SerializedProperty _maxValueProperty;
        private AnimBool _readOnlyValueAnimation;
        private AnimBool _readOnlyClampAnimation;
        private AnimBool _raiseWarningAnimation;
        private AnimBool _isClampedVariableAnimation;
        
        private const string READONLY_TOOLTIP = "This allows for a definition in the editor. When disabled (and in \"Objects\" directory),"
            + "it will not store data between playtesting or builds.";

        protected virtual void OnEnable()
        {
            _readOnly = serializedObject.FindProperty("_readOnly");
            _valueProperty = serializedObject.FindProperty("_value");
            _raiseWarning = serializedObject.FindProperty("_raiseWarning");
            _isClamped = serializedObject.FindProperty("_isClamped");
            _minValueProperty = serializedObject.FindProperty("_minClampedValue");
            _maxValueProperty = serializedObject.FindProperty("_maxClampedValue");

            _raiseWarningAnimation = new AnimBool(_readOnly.boolValue);
            _raiseWarningAnimation.valueChanged.AddListener(Repaint);

            _readOnlyValueAnimation = new AnimBool(_readOnly.boolValue);
            _readOnlyValueAnimation.valueChanged.AddListener(Repaint);

            _readOnlyClampAnimation = new AnimBool(_readOnly.boolValue);
            _readOnlyClampAnimation.valueChanged.AddListener(Repaint);

            _isClampedVariableAnimation = new AnimBool(_isClamped.boolValue);
            _isClampedVariableAnimation.valueChanged.AddListener(Repaint);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawReadonlyField();

            EditorGUILayout.Space();

            DrawValue();

            EditorGUILayout.Space();

            DrawClampedFields();
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
        protected virtual void DrawValue()
        {
            _readOnlyValueAnimation.target = _readOnly.boolValue;
            using (var group = new EditorGUILayout.FadeGroupScope(_readOnlyValueAnimation.faded))
            {
                if (group.visible)
                {
                    GenericPropertyDrawer.DrawPropertyDrawerLayout(_valueProperty, Target.Type);
                }
            }
        }
        protected void DrawClampedFields()
        {
            if (!IsClampable)
                return;

            _readOnlyClampAnimation.target = !_readOnly.boolValue;
            using (var group = new EditorGUILayout.FadeGroupScope(_readOnlyClampAnimation.faded))
            {
                if (group.visible)
                {
                    EditorGUILayout.PropertyField(_isClamped);
                }
            }

            _isClampedVariableAnimation.target = _isClamped.boolValue && !_readOnly.boolValue;
            using (var group = new EditorGUILayout.FadeGroupScope(_isClampedVariableAnimation.faded))
            {
                if (group.visible)
                {
                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorGUILayout.PropertyField(_minValueProperty);
                        EditorGUILayout.PropertyField(_maxValueProperty);
                    }
                }
            }
        }
    }
    [CustomEditor(typeof(BaseVariable<,>), true)]
    public class BaseVariableWithEventEditor : BaseVariableEditor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_event"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}