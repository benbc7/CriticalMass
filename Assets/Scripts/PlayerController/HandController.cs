/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour {

	public Joint handJoint;

	private int holdableObjectID;
	private Rigidbody holdableObject;
	private int originalLayer;
	private Rigidbody heldObject;
	private int LeverID;
	private IActivatable lever;

	private Vector3 savedVelocity;
	private Vector3 savedAngularVelocity;

	public void PickUpObject () {
		if (holdableObject != null) {
			handJoint.connectedBody = holdableObject;
			heldObject = holdableObject;
			originalLayer = heldObject.gameObject.layer;
			heldObject.gameObject.layer = 10;
		} else if (lever != null) {
			lever.Switch ();
		}
	}

	public void DropObject () {
		if (heldObject != null) {
			savedVelocity = heldObject.velocity;
			savedAngularVelocity = heldObject.angularVelocity;
			handJoint.connectedBody = null;
			heldObject.gameObject.layer = originalLayer;

			heldObject.velocity = savedVelocity;
			heldObject.angularVelocity = savedAngularVelocity;
			heldObject = null;
		}
	}

	private void OnTriggerEnter (Collider other) {
		if (other.gameObject.layer == 11 || other.gameObject.layer == 17) {
			holdableObjectID = other.GetInstanceID ();
			holdableObject = other.GetComponent<Rigidbody> ();
		} else if (other.gameObject.layer == 16) {
			LeverID = other.GetInstanceID ();
			lever = other.gameObject.GetComponent<IActivatable> ();
		}
	}

	private void OnTriggerExit (Collider other) {
		if (other.GetInstanceID () == holdableObjectID) {
			holdableObject = null;
			holdableObjectID = 0;
		} else if (other.GetInstanceID () == LeverID) {
			lever = null;
			LeverID = 0;
		}
	}
}