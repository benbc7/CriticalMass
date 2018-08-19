/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IActivatable {

	public bool activated = true;
	private bool initialized;

	public bool Activated {
		get {
			return activated;
		}
	}

	public Vector3[] localWaypoints;
	public float speed = 2f;
	public bool cyclic;
	public float waitTime = 0.5f;

	[Range (0, 2)]
	public float easeAmount = 1f;

	private Vector3 [] globalWaypoints;
	private int fromWaypointIndex;
	private float percentBetweenWaypoints;
	private float nextMoveTime;

	public void Initialize () {
		initialized = true;
		globalWaypoints = new Vector3 [localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++) {
			globalWaypoints [i] = localWaypoints [i] + transform.position;
		}
	}

	private void Update () {
		if (activated && initialized) {
			Vector3 move = CalculatePlatormMovement ();
			transform.Translate (move);
		}
	}

	public void Activate () {
		activated = true;
	}

	public void Deactivate () {
		activated = false;
	}

	public void Switch () {
		activated = !activated;
	}

	private Vector3 CalculatePlatormMovement () {
		if (Time.time < nextMoveTime) {
			return Vector3.zero;
		}

		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01 (percentBetweenWaypoints);
		float easedPercentBetweenWaypoints = Ease (percentBetweenWaypoints);

		Vector3 newPosition = Vector3.Lerp (globalWaypoints [fromWaypointIndex], globalWaypoints [toWaypointIndex], easedPercentBetweenWaypoints);

		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0;
			fromWaypointIndex++;

			if (!cyclic) {
				if (fromWaypointIndex >= globalWaypoints.Length - 1) {
					fromWaypointIndex = 0;
					System.Array.Reverse (globalWaypoints);
				}
			}

			nextMoveTime = Time.time + waitTime;
		}

		return newPosition - transform.position;
	}

	private float Ease (float t) {
		float a = easeAmount + 1;
		return Mathf.Pow (t, a) / (Mathf.Pow (t, a) + Mathf.Pow (1 - t, a));
	}

	private void OnDrawGizmosSelected () {
		if (localWaypoints != null) {
			Gizmos.color = Color.red;

			for (int i = 0; i < localWaypoints.Length; i++) {
				Vector3 globalPosition = (Application.isPlaying) ? globalWaypoints [i] : localWaypoints [i] + transform.position;
				Gizmos.DrawWireCube (globalPosition, transform.localScale);
			}
		}
	}
}