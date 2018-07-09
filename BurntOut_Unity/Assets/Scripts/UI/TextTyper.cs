using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
[RequireComponent(typeof(RectTransform))]

public class TextTyper : MonoBehaviour
{
    public bool Wait;

    [SerializeField] private float typeSpeed;
    [SerializeField] private float startDelay;
    [SerializeField] private bool startOnAwake;

    private int counter;
    private string textToType;
    private Text textComp;
    public bool Typing { get; private set; }

    void Awake()
    {
        textComp = GetComponent<Text>();

        counter = 0;
        textToType = textComp.text;
        textComp.text = "";

        if (startOnAwake)
            StartTyping();
    }

    public void StartTyping()
    {
        if (!Typing)
        {
            Typing = true;
            InvokeRepeating("Type", startDelay, typeSpeed);
        }
        else
        {
            print(gameObject.name + " : Is already typing!");
        }
    }

    public void StopTyping()
    {
        counter = 0;
        Typing = false;
        CancelInvoke("Type");
    }

    public void FinishTyping()
    {
        textComp.text = textToType;
        StopTyping();
    }

    public void UpdateText(string newText)
    {
        StopTyping();
        textComp.text = "";
        textToType = newText;
        StartTyping();
    }

    private void Type()
    {
        if (Wait || textToType == null)
            return;

        if (counter >= textToType.Length)
        {
            Typing = false;
            CancelInvoke("Type");
            return;
        }

        textComp.text = textComp.text + textToType[counter];
        //audioComp.Play();
        counter++;

        //RandomiseVolume();

        if (counter >= textToType.Length)
        {
            Typing = false;
            CancelInvoke("Type");
        }
    }
}
