using UnityEngine;

public class UI_HealthAlert : MonoBehaviour
{
    public PlayerStats playerstats;
    public GameObject mytext;

    // Update is called once per frame
    void Update()
    {
        if (playerstats.LowHealth())
            mytext.SetActive(true);
        else
            mytext.SetActive(false);
    }
}
