using UnityEditor;
using UnityEngine;

public class ScreenShoterEditor : EditorWindow
{
    public enum TabType
    {
        Application,
        Setup,
    }

    [MenuItem("Tools/Utility/Screen Shooter", priority = 15)]
    public static void ShowWindow()
    {
        var window = GetWindow<ScreenShoterEditor>();
        window.Show();
    }

    private string[] TabNames = EnumExtension<TabType>.Names;

    private TabType CurrentTab { get; set; }

    private ScreenShooter.Window.Application applicationWindow = new ScreenShooter.Window.Application();

    private ScreenShooter.Window.Setup setupWindow = new ScreenShooter.Window.Setup();

    private void OnGUI()
    {
        DrawTab();

        switch (CurrentTab)
        {
            case TabType.Application:
                applicationWindow.Draw(this);
                break;

            case TabType.Setup:
                setupWindow.Draw(this);
                break;
        }
    }

    private void DrawTab()
    {
        CurrentTab = (TabType)GUILayout.Toolbar((int)CurrentTab, TabNames);
        GUILayout.Space(5);
    }

    private void DrawApplication()
    {
    }

    private void DrawSetup()
    {
    }
}