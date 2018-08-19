using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelTrigger : MonoBehaviour {

    public HubDoor hubDoor;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameManager.instance.LoadNextLevel (hubDoor);
            Destroy (gameObject);
        }
    }
}
