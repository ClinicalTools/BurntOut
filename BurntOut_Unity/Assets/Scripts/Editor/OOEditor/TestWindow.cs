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
    TextField test2Txt, test3Txt;
    Button btnTest;
    Button btnTest2;
    Toggle tglTest;
    ToggleButton tglbtnTest, tglbtnTest2;
    IntSlider isdrTest, isdrTest2;
    FloatSlider fsdrTest;
    Foldout fldTest;
    EnumPopup epopTest;
    IntField intfieldTest;

    public void OnEnable()
    {
        testLbl = new LabelField("Test1");
        testTxt = new TextField("");
        test2Txt = new TextField("abc");
        test3Txt = new TextField("abc", "abcd", "abcde");
        //test2Txt.LabelStyle.FontStyle = FontStyle.Bold;
        //test2Txt.Style.FontSize = 18;

        btnTest = new Button("Test3");
        btnTest2 = new Button("Test4");
        tglTest = new Toggle("Test4");
        isdrTest = new IntSlider(5, 0, 10, "hello");
        isdrTest2 = new IntSlider(5, 0, 10);
        fsdrTest = new FloatSlider(5, 0, 10, "Test9", "tooltip");
        fldTest = new Foldout("Test5");
        epopTest = new EnumPopup(Day.Sunday, "Test6");
        tglbtnTest = new ToggleButton(false, "Test");
        tglbtnTest2 = new ToggleButton(false, "Test2");
        intfieldTest = new IntField(0);
    }

    // Add menu named "Scene Manager" to the Window menu
    [MenuItem("Window/TEST2")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        TestWindow2 window = GetWindow<TestWindow2>("TEST2");
        window.Show();
    }

    float val;
    void OnGUI()
    {
        using (new Toolbar())
        {
            test2Txt.Draw();
            testTxt.Draw();
            tglbtnTest2.Draw();
            btnTest.Draw();
        }
        btnTest2.Draw();
        fsdrTest.Draw();
        isdrTest.Draw();
        //EditorGUILayout.IntSlider(6, 0, 10);
        fldTest.Draw();
        test3Txt.Draw();
        //EditorGUILayout.TextField("Test2", "abc");
        tglTest.Draw();
        //EditorGUILayout.Toggle("Test4", false);
        epopTest.Draw();
        //EditorGUILayout.EnumPopup("Test6", Day.Monday);
        //*/
    }
}