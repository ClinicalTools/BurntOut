using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalStats : MonoBehaviour {

    public bool isHospitalCompleted;
    public bool isMrJohnsonCompleted;
    public int currentHealth;
    public int GOOD_stars;
    public int BAD_stars;
    public List<GlobalStats> myNodes;

    public static GlobalStats instance = null;

    private void Awake() {

        if (instance == null) {
            instance = this;
        } else if (instance != null) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

    }

}
