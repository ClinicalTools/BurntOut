using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class SceneChange : MonoBehaviour {
    
    public string level;
    public Animator screenfade;
    public GameObject player;
    public bool mrJohnsonRoom;
    public GameObject darkParticleEffect;
    public GameObject lightParticleEffect;


    private GlobalStats globalStats;

    private void Start() {

        globalStats = GameObject.FindObjectOfType<GlobalStats>();

        if (globalStats.isMrJohnsonCompleted && mrJohnsonRoom) {
            darkParticleEffect.SetActive(true);
            lightParticleEffect.SetActive(false);
        }


    }

    private void OnTriggerEnter(Collider other) {

        StartCoroutine(Transition());
    }

    public IEnumerator Transition() {

        player.GetComponent<FirstPersonController>().enabled = false;
        screenfade.SetBool("fade", true);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(level);
    }

}
