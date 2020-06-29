using System.IO;
using UnityEditor;
using UnityEngine;

namespace ScreenShooter
{
    public static class SettingHelper
    {
        public static string GetFullPath(MessengerType type)
        {
            return string.Format("{0}/Editor/Util/ScreenShooter/Resources/Setting/{1}.asset", UnityEngine.Application.dataPath, type);
        }

        public static string GetSavePath(MessengerType type)
        {
            return string.Format("Assets/Editor/Util/ScreenShooter/Resources/Setting/{0}.asset", type);
        }

        public static string GetLoadPath(MessengerType type)
        {
            return string.Format("Setting/{0}", type.ToString());
        }

        public static Setting Load(MessengerType type)
        {
            var scriptableObject = Resources.Load(GetLoadPath(type), typeof(ScriptableObject));
            switch (type)
            {
                case MessengerType.Slack:
                    return scriptableObject as SlackSetting;

                case MessengerType.Discord:
                    return scriptableObject as DiscordSetting;
            }

            return null;
        }

        public static Setting LoadOrCreate(MessengerType type)
        {
            var fullPath = GetFullPath(type);

            Setting setting = null;

            if (File.Exists(fullPath))
            {
                setting = Load(type);
            }
            else
            {
                var savePath = GetSavePath(type);

                switch (type)
                {
                    case MessengerType.Slack:
                        setting = ScriptableObject.CreateInstance<SlackSetting>();
                        break;

                    case MessengerType.Discord:
                        setting = ScriptableObject.CreateInstance<DiscordSetting>();
                        break;
                }

                AssetDatabase.CreateAsset(setting, savePath);
                AssetDatabase.SaveAssets();
            }

            return setting;
        }
    }
}