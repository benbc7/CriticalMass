/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryPoint : MonoBehaviour {

	public Mesh arrowMesh;

	private void OnDrawGizmos () {
		Gizmos.color = Color.cyan;
		Vector3 pivot = transform.position;
		Vector3 cubeSize = new Vector3 (2.5f, 5f, 0.4f);
		Vector3 cubeCenter = new Vector3 (pivot.x + 1.25f, pivot.y + 2.5f, pivot.z + 0);
		Gizmos.DrawCube (cubeCenter, cubeSize);
		Gizmos.DrawWireCube (cubeCenter, cubeSize);
		cubeCenter.x = pivot.x - 1.25f;
		Gizmos.DrawCube (cubeCenter, cubeSize);
		Gizmos.DrawWireCube (cubeCenter, cubeSize);
		Gizmos.color = Color.red;
		Gizmos.DrawLine (pivot + Vector3.right * 100, pivot + Vector3.left * 100);
		if (arrowMesh != null) {
			Gizmos.color = Color.green;
			Gizmos.DrawWireMesh (arrowMesh, new Vector3 (pivot.x, pivot.y + 2.5f, pivot.z + 0.25f), Quaternion.identity, Vector3.one);
		}
	}
}