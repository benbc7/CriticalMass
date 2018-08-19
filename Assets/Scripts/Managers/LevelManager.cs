/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:		Critical Mass
Date:			10/30/2017
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public Transform[] rootObjects;
	public Transform nextLevelEntryPoint;
	public MovingPlatform[] movingPlatforms;

	public Vector3 InitializeLevel (Vector3 position) {
		for (int i = 0; i < rootObjects.Length; i++) {
			rootObjects [i].parent = transform;
		}

		transform.position = position;
		if (movingPlatforms != null) {
			for (int i = 0; i < movingPlatforms.Length; i++) {
				movingPlatforms [i].Initialize ();
			}
		}

		for (int i = 0; i < rootObjects.Length; i++) {
			rootObjects [i].parent = null;
		}

		return nextLevelEntryPoint.position;
	}
}