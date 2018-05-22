using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu_GameManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Hospital");
    }
}
