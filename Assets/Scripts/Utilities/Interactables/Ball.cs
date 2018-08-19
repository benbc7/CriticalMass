/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	private Rigidbody rb;
	private bool sfxPlaying;

	private void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	private void Update () {
		if (rb.angularVelocity.magnitude > 5f && !sfxPlaying) {
			AudioManager.instance.PlaySound ("Rolling");
			sfxPlaying = true;
		} else if (sfxPlaying) {
			AudioManager.instance.StopSound ("Rolling");
			sfxPlaying = false;
		}
	}
}