using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class SceneChange : MonoBehaviour {


    public string level;
    public Animator screenfade;
    public GameObject player;

    private void OnTriggerEnter(Collider other) {


        if (other.name == "Player") {
            StartCoroutine(Transition());
        }

    }

    public IEnumerator Transition() {

        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        screenfade.SetBool("fade", true);
        yield return new WaitForSeconds(0.5f);
        Application.LoadLevel(level);
    }

}
