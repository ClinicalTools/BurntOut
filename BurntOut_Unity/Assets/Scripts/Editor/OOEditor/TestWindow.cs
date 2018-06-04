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
    IntSlider isdrTest;
    Foldout fldTest;
    EnumPopup epopTest;

    public void OnEnable()
    {
        testLbl = new LabelField("Test1");
        testTxt = new TextField("");
        test2Txt = new TextField("", "Test2", "abccc");
        btnTest = new Button("Test3", "ok");
        tglTest = new Toggle("Test4");
        isdrTest = new IntSlider(5, 0, 10);
        fldTest = new Foldout("Test5:");
        epopTest = new EnumPopup(Day.Sunday, "Test 6");
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
        testLbl.Draw();
        testTxt.Draw();
        btnTest.Draw();
        test2Txt.Draw();
        fldTest.Draw();
        tglTest.Draw();
        isdrTest.Draw();
        epopTest.Draw();
    }
}