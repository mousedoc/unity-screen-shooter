using UnityEditor;
using UnityEngine;

namespace ScreenShooter.Window
{
    public class Setup
    {
        private EditorWindow parent = null;

        private readonly string messengerName = "Messenger";

        private int currentTypeIndex = 0;

        private MessengerType currentType { get => (MessengerType)currentTypeIndex; }

        private string[] options = EnumExtension<MessengerType>.Names;
        private int beforeTypeIndex = -1;

        private Setting setting = null;

        public void Draw(EditorWindow parent)
        {
            this.parent = parent;

            DrawMessengerType();
            DrawProperties();
        }

        private void DrawMessengerType()
        {
            int nameWidth = (int)(parent.position.width * 0.2f);

            GUILayout.BeginHorizontal();

            GUILayout.Label(messengerName, GUILayout.Width(nameWidth));
            currentTypeIndex = EditorGUILayout.Popup(currentTypeIndex, options);

            GUILayout.EndHorizontal();
        }

        private void DrawProperties()
        {
            if (beforeTypeIndex != currentTypeIndex)
            {
                setting = SettingHelper.LoadOrCreate(currentType);
                beforeTypeIndex = currentTypeIndex;
            }

            setting.Draw(parent);
        }
    }
}