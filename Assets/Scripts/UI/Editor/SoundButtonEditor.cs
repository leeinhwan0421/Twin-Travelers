using UnityEditor;
using UnityEditor.UI;
using TwinTravelers.UI;

namespace TwinTravelers.Core.Utility.Editor
{
    [CustomEditor(typeof(SoundButton))]
    public class SoundButtonEditor : ButtonEditor
    {
        SerializedProperty buttonType;

        protected override void OnEnable()
        {
            base.OnEnable();
            buttonType = serializedObject.FindProperty("buttonType");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(buttonType);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
