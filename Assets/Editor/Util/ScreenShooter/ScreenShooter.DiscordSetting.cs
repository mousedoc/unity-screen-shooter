using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace ScreenShooter
{
    // Discord
    [Serializable]
    public class DiscordSetting : Setting
    {
        public const string ApiUrl = "https://discord.com/api";

        [SerializeField]
        private string token;

        public string Token
        {
            get => token;
            private set => token = value;
        }

        [SerializeField]
        private string channel;

        public string Channel
        {
            get => channel;
            private set => channel = value;
        }

        public DiscordSetting() : base(MessengerType.Discord)
        {
        }

        public override void Draw(EditorWindow parent)
        {
            int nameWidth = (int)(parent.position.width * 0.2f);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Token", GUILayout.Width(nameWidth));
            Token = GUILayout.TextField(Token);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Channel", GUILayout.Width(nameWidth));
            Channel = GUILayout.TextField(Channel);
            GUILayout.EndHorizontal();

            if (GUILayout.Button("Save"))
                Save();
        }

        public override UnityWebRequest CreateWebRequest(string title, string comment, Texture2D capture)
        {
            var formData = new List<IMultipartFormSection>();
            var contents = capture.EncodeToPNG();

            formData.Add(new MultipartFormDataSection("token", Token));
            formData.Add(new MultipartFormDataSection("title", title));
            formData.Add(new MultipartFormDataSection("initial_comment", comment));
            formData.Add(new MultipartFormDataSection("channels", Channel));
            formData.Add(new MultipartFormFileSection("file", contents, capture.name, "image/png"));

            var www = UnityWebRequest.Post($"{ApiUrl}/files.upload", formData);
            return www;
        }
    }
}