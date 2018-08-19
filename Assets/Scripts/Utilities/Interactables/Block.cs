/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

	private void OnCollisionEnter (Collision collision) {
		if (collision.transform.tag == "Environment" || collision.transform.tag == "Button") {
			if (collision.relativeVelocity.magnitude > 2f) {
				AudioManager.instance.PlaySound ("Hit");
			}
		}
	}
}