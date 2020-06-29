using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using EditorCoroutineExtension;

namespace ScreenShooter.Window
{
    public class Application
    {
        private readonly string typeName = "Messenger";
        private readonly string titleName = "Title";
        private readonly string commentName = "Comment";
        private readonly string captureName = "Capture";
        private readonly string sendButtonText = "Send";
        private readonly string[] options = EnumExtension<MessengerType>.Names;

        private EditorWindow parent = null;

        private int messengerTypeIndex = 0;
        private MessengerType MessengerType { get => (MessengerType)messengerTypeIndex; }
        private string titleInput = "Untitled";
        private string commentInput = "no comment";

        private Texture2D currentCaptureTexture = null;
        private string currentCaptureTexturePath = null;
        private System.Random random = new System.Random();

        public string GeneratedFilePath
        {
            get
            {
                var fileName = string.Format("/{0}_ScreenShot_{1}.png", DateTime.Now.ToString("yyMMddHHmm"), random.Next());
                return UnityEngine.Application.persistentDataPath + fileName;
            }
        }

        public void Draw(EditorWindow parent)
        {
            this.parent = parent;

            DrawFields();
            DrawCaptureAndSend();
        }

        private void DrawCaptureAndSend()
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(captureName))
            {
                currentCaptureTexturePath = GeneratedFilePath;
                ScreenCapture.CaptureScreenshot(currentCaptureTexturePath);
                currentCaptureTexture = null;
            }
            if (GUILayout.Button(sendButtonText))
            {
                Send();
            }

            GUILayout.EndHorizontal();

            if (currentCaptureTexture == null)
            {
                if (string.IsNullOrEmpty(currentCaptureTexturePath) == false && File.Exists(currentCaptureTexturePath))
                {
                    var bytes = File.ReadAllBytes(currentCaptureTexturePath);
                    currentCaptureTexture = new Texture2D(1, 1);
                    currentCaptureTexture.LoadImage(bytes);
                }

                if (string.IsNullOrEmpty(currentCaptureTexturePath))
                    GUILayout.Label("Press capture button");
                else
                    GUILayout.Label("Loading capture texture...");
            }
            else
            {
                float heightOffset = 0.65f;
                float aspect = (float)currentCaptureTexture.width / currentCaptureTexture.height;
                float height = parent.position.height * heightOffset;
                float width = aspect * height;

                if (width >= parent.position.width)
                {
                    width = parent.position.width - 20;
                    height = width / aspect;
                }

                float offsetWidth = parent.position.width / 2f - width / 2f;
                float offsetHeight = 110;

                var rect = new Rect(offsetWidth, offsetHeight, width, height);
                EditorGUI.DrawPreviewTexture(rect, currentCaptureTexture);
            }
        }

        private void DrawFields()
        {
            int nameWidth = (int)(parent.position.width * 0.2f);

            GUILayout.BeginHorizontal();
            GUILayout.Label(typeName, GUILayout.Width(nameWidth));
            messengerTypeIndex = EditorGUILayout.Popup(messengerTypeIndex, options);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(titleName, GUILayout.Width(nameWidth));
            titleInput = GUILayout.TextField(titleInput);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(commentName, GUILayout.Width(nameWidth));
            commentInput = GUILayout.TextField(commentInput);
            GUILayout.EndHorizontal();
        }

        private void Send()
        {
            if (currentCaptureTexture != null)
            {
                var setting = SettingHelper.Load(MessengerType);

                if (setting == null)
                {
                    Debug.LogError("[ScreenShotShooter.Application] Send() - Setup first");
                    return;
                }

                var request = setting.CreateWebRequest(titleInput, commentInput, currentCaptureTexture);

                EditorCoroutines.StartCoroutine(Send(request, () =>
                {
                    currentCaptureTexture = null;
                    currentCaptureTexturePath = null;
                },
                (error) =>
                {
                    Debug.LogError("[ScreenShotShooter.Application] Send() - Fail to send");
                    currentCaptureTexture = null;
                    currentCaptureTexturePath = null;
                }), this);
            }
        }

        private static IEnumerator Send(UnityWebRequest www, Action successCallback, Action<string> errorCallback)
        {
            yield return www.SendWebRequest();

            while (www.isDone == false)
                yield return null;

            var error = www.error;

            Debug.LogFormat("[ScreenShotShooter.Application] Send() response - {0}", www.downloadHandler.text);

            if (string.IsNullOrEmpty(error))
            {
                successCallback?.Invoke();
            }
            else
            {
                errorCallback?.Invoke(error);
            }
        }
    }
}