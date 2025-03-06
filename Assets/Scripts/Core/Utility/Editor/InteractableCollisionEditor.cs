#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace TwinTravelers.Core.Utility.Editor
{
    [CustomEditor(typeof(InteractableCollision), true)]
    public class InteractableCollisionEditor : UnityEditor.Editor
    {
        private bool[] tagChecks;

        private void OnEnable()
        {
            string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;
            InteractableCollision triggerScript = (InteractableCollision)target;

            tagChecks = new bool[allTags.Length];

            for (int i = 0; i < allTags.Length; i++)
            {
                tagChecks[i] = triggerScript.selectedTags.Contains(allTags[i]);
            }
        }


        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            InteractableCollision triggerScript = (InteractableCollision)target;
            string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;

            EditorGUILayout.LabelField("Select Tags:", EditorStyles.boldLabel);

            for (int i = 0; i < allTags.Length; i++)
            {
                tagChecks[i] = EditorGUILayout.ToggleLeft(allTags[i], tagChecks[i]);

                if (tagChecks[i] && !triggerScript.selectedTags.Contains(allTags[i]))
                {
                    triggerScript.selectedTags.Add(allTags[i]);
                }
                else if (!tagChecks[i] && triggerScript.selectedTags.Contains(allTags[i]))
                {
                    triggerScript.selectedTags.Remove(allTags[i]);
                }
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(triggerScript);
            }
        }
    }
}
#endif
