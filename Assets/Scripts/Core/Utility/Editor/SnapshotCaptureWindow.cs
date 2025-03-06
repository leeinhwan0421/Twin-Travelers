using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TwinTravelers.Core.Utility.Editor
{
    /// <summary>
    /// 스크린샷을 캡쳐할 수 있는 에디터 클래스
    /// 
    /// [HOW TO USE]
    /// 에디터 상단 탭 Capture > Capture Manager 
    /// 사용하고자 하는 카메라를 넣고, 해상도를 입력한 뒤, 캡쳐 버튼을 클릭한다.
    /// Assets/ScreenShot 디렉토리에 저장된다. (*gitignore에 추가해두었으니 안심하고 마구 사용하세요.)
    /// </summary>
    public class SnapshotCaptureWindow : EditorWindow
    {
        #region Fields
        private Camera camera;
        private bool transparent;
        private Vector2 captureSize = new Vector2(1920, 1080);
        private RenderTexture previewRenderTexture;
        private Texture2D previewTexture;
        #endregion

        [MenuItem("Tools/Capture")]
        public static void ShowWindow()
        {
            EditorWindow window = GetWindow<SnapshotCaptureWindow>("Capture Window");
            window.minSize = new Vector2(350, 250);
        }

        /// <summary>
        /// GUI 출력
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(7);
            Rect controlRect = EditorGUILayout.GetControlRect();
            GUI.Label(controlRect, "Capture Tool", new GUIStyle() { alignment = TextAnchor.MiddleCenter, fontStyle = FontStyle.Bold, fontSize = 16, normal = EditorStyles.label.normal });

            GUILayout.Space(10.0f);

            camera = (Camera)EditorGUILayout.ObjectField("Camera", camera, typeof(Camera), true);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Size");
            captureSize = EditorGUILayout.Vector2Field("", captureSize, GUILayout.Width(EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth));
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Use Current Screen Size"))
            {
                string[] screenres = UnityEditor.UnityStats.screenRes.Split('x');
                captureSize.x = int.Parse(screenres[0]);
                captureSize.y = int.Parse(screenres[1]);
            }

            transparent = EditorGUILayout.Toggle("Transparent", transparent);

            GUILayout.Space(10);

            // 실시간 미리보기 업데이트
            UpdatePreview();

            EditorGUI.BeginDisabledGroup(!camera);
            if (GUILayout.Button("Capture"))
            {
                CaptureScreenshot();
            }
            EditorGUI.EndDisabledGroup();

            // 캡처 미리보기 표시
            GUILayout.Space(15);
            GUILayout.Label("Live Preview", EditorStyles.boldLabel);
            if (previewTexture != null)
            {
                float previewWidth = EditorGUIUtility.currentViewWidth - 40;
                float previewHeight = (previewWidth / captureSize.x) * captureSize.y; // 비율 유지

                GUILayout.Box(previewTexture, GUILayout.Width(previewWidth), GUILayout.Height(previewHeight));
            }
            else
            {
                GUILayout.Label("No Preview Available", EditorStyles.centeredGreyMiniLabel);
            }
        }

        /// <summary>
        /// 실시간 미리보기 업데이트 (성능에 문제 있을 수 있습니다.)
        /// </summary>
        private void UpdatePreview()
        {
            if (camera == null || captureSize.x <= 0 || captureSize.y <= 0)
            {
                return;
            }

            int captureWidth = (int)Mathf.Round(captureSize.x);
            int captureHeight = (int)Mathf.Round(captureSize.y);

            if (previewRenderTexture != null)
            {
                previewRenderTexture.Release();
                previewRenderTexture = null;
            }

            previewRenderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            camera.targetTexture = previewRenderTexture;

            camera.Render();

            if (previewTexture == null || previewTexture.width != captureWidth || previewTexture.height != captureHeight)
            {
                previewTexture = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }

            RenderTexture.active = previewRenderTexture;
            previewTexture.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
            previewTexture.Apply();
            RenderTexture.active = null;

            camera.targetTexture = null;
        }

        /// <summary>
        /// 스크린샷 찍기
        /// </summary>
        private void CaptureScreenshot()
        {
            int captureWidth = (int)Mathf.Round(captureSize.x);
            int captureHeight = (int)Mathf.Round(captureSize.y);

            Debug.Log(captureSize);

            string path = Application.dataPath + "/ScreenShot/";
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(path);
            }

            string name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";

            TextureFormat format = transparent ? TextureFormat.ARGB32 : TextureFormat.RGB24;
            Texture2D screenShot = new Texture2D(captureWidth, captureHeight, format, false);
            RenderTexture rt = new RenderTexture(captureWidth, captureHeight, 24);

            RenderTexture bak_cam_targetTexture = camera.targetTexture;
            RenderTexture bak_RenderTexture_active = RenderTexture.active;
            CameraClearFlags bak_cam_clearFlags = camera.clearFlags;
            Color bak_cam_background = camera.backgroundColor;

            RenderTexture.active = rt;
            camera.targetTexture = rt;
            if (transparent)
            {
                camera.clearFlags = CameraClearFlags.SolidColor;
                camera.backgroundColor = Color.clear;
            }

            camera.Render();
            screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);
            screenShot.Apply();

            camera.targetTexture = bak_cam_targetTexture;
            RenderTexture.active = bak_RenderTexture_active;
            camera.clearFlags = bak_cam_clearFlags;
            camera.backgroundColor = bak_cam_background;

            byte[] bytes = screenShot.EncodeToPNG();
            File.WriteAllBytes(name, bytes);

            AssetDatabase.Refresh();
        }
    }
}