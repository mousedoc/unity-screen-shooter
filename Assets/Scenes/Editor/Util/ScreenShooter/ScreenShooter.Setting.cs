using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace ScreenShooter
{
    public enum MessengerType
    {
        Slack,
        Discord,
    }

    public abstract class Setting : ScriptableObject
    {
        public MessengerType Type { get; protected set; }

        protected Setting(MessengerType type)
        {
            Type = type;
        }

        public abstract void Draw(EditorWindow parent);

        public abstract UnityWebRequest CreateWebRequest(string title, string comment, Texture2D capture);

        protected void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
    }
}