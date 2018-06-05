using UnityEditor;
using OOEditor;
using UnityEngine;

/// <summary>
/// Manages editing the scene's scenarios.
/// </summary>
public class TestWindow2 : EditorWindow
{
    enum Day { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday };
    LabelField testLbl;
    TextField testTxt;
    TextField test2Txt;
    Button btnTest;
    Toggle tglTest;
    ToggleButton tglbtnTest;
    IntSlider isdrTest;
    Foldout fldTest;
    EnumPopup epopTest;

    public void OnEnable()
    {
        testLbl = new LabelField("Test1");
        testTxt = new TextField("");
        test2Txt = new TextField("abc", "Test2", "abccc");
        //test2Txt.LabelStyle.FontStyle = FontStyle.Bold;
        //test2Txt.Style.FontSize = 18;

        btnTest = new Button("Test3", "ok");
        tglTest = new Toggle("Test4");
        isdrTest = new IntSlider(5, 0, 10);
        fldTest = new Foldout("Test5");
        epopTest = new EnumPopup(Day.Sunday, "Test6");
        tglbtnTest = new ToggleButton(false, "Test");
    }

    // Add menu named "Scene Manager" to the Window menu
    [MenuItem("Window/TEST2")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        TestWindow2 window = GetWindow<TestWindow2>("TEST2");
        window.Show();
    }

    void OnGUI()
    {
        using (new Toolbar())
        {
            testLbl.Draw();
            testTxt.Draw();
            btnTest.Draw();
            tglbtnTest.Draw();
            isdrTest.Draw();
        }

        testLbl.Draw();
        EditorGUILayout.LabelField("Abc");
        testTxt.Draw();
        btnTest.Draw();
        fldTest.Draw();
        test2Txt.Draw();
        EditorGUILayout.TextField("Test2", "abc");
        tglTest.Draw();
        EditorGUILayout.Toggle("Test4", false);
        epopTest.Draw();
        EditorGUILayout.EnumPopup("Test6", Day.Monday);
    }
}