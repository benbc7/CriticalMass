/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorTrigger : MonoBehaviour {

	public GameObject canvas;

	private bool monitorMode;
	private CameraController cameraController;

	private void Start () {
		cameraController = FindObjectOfType<CameraController> ();
		canvas.SetActive (false);
	}

	private void Update () {

	}

	public void OnExit () {
		monitorMode = false;
		cameraController.SetMonitorMode ();
		GameManager.instance.PausePlayer ();
	}

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			canvas.SetActive (true);
		}
	}

	private void OnTriggerStay (Collider other) {
		if (other.tag == "Player" && !monitorMode) {
			if (Input.GetMouseButtonDown (0)) {
				monitorMode = true;
				cameraController.SetMonitorMode ();
				GameManager.instance.PausePlayer ();
			}
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			canvas.SetActive (false);
		}
	}
}