using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (Text))]
[RequireComponent (typeof (RectTransform))]

public class TextTyper : MonoBehaviour 
{	
    public static TextTyper Instance { get; private set; }

	[SerializeField] private float typeSpeed;
	[SerializeField] private float startDelay;
	//[SerializeField] private float volumeVariation;
	[SerializeField] private bool startOnAwake;

	private int counter;
	private string textToType;
	private bool typing;
	private Text textComp;
    //private AudioSource audioComp;

	void Awake()
	{
        Instance = this;

		textComp = GetComponent<Text>();
		//audioComp = GetComponent<AudioSource>();

		counter = 0;
		textToType = textComp.text;
		textComp.text = "";

		if(startOnAwake)
		{
            StartTyping();
		}
	}


    public void StartTyping()
	{	
		if(!typing)
		{
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
        typing = false;
		CancelInvoke("Type");
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
		typing = true;
		textComp.text = textComp.text + textToType[counter];
		//audioComp.Play();
		counter++;

		//RandomiseVolume();

		if(counter == textToType.Length)
		{	
			typing = false;
			CancelInvoke("Type");
		}
	}

	private void RandomiseVolume()
	{
		//audioComp.volume = Random.Range(1 - volumeVariation, volumeVariation + 1);
	}

    public bool IsTyping() { return typing; }
}
