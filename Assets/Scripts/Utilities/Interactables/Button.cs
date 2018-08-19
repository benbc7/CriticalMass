/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {

	public Transform buttonBase;
	public LayerMask activatorMask;
	public Color activatedColor;

	public GameObject targetActivatable;
	private IActivatable _targetIActivatable;
	public float minimumMass;

	new private Collider collider;
	private Animator animator;
	private Color defaultColor;
	private MeshRenderer meshRenderer;
	private bool activated;

	private List<Collider> activatingColliders = new List<Collider> ();

	private void Start () {
		animator = transform.parent.GetComponent<Animator> ();
		meshRenderer = buttonBase.GetComponent<MeshRenderer> ();
		collider = GetComponent<Collider> ();
		defaultColor = meshRenderer.material.color;
		_targetIActivatable = targetActivatable.GetComponent<IActivatable> ();
	}

	private void Update () {
		Vector3 origin = new Vector3 (collider.bounds.center.x, collider.bounds.max.y - 0.015f, collider.bounds.center.z);
		Vector3 halfExtends = new Vector3 (collider.bounds.size.x / 2, 0.2f, collider.bounds.size.z / 2);
		Collider [] hitColliders = Physics.OverlapBox (origin, halfExtends, transform.parent.rotation, activatorMask);
		if (hitColliders.Length > 0) {
			if (!activated) {
				SwitchActivated ();
			} else {
				CheckActivation ();
			}
		} else {
			if (activated) {
				SwitchActivated ();
			}
		}
	}

	private void CheckActivation () {
		if (!_targetIActivatable.Activated) {
			_targetIActivatable.Activate ();
		}
	}

	public void SwitchActivated () {
		activated = !activated;
		AudioManager.instance.PlaySound ("Switch");
		if (activated) {
			animator.SetBool ("Activated", true);
			_targetIActivatable.Activate ();
			meshRenderer.material.color = activatedColor;
		} else {
			animator.SetBool ("Activated", false);
			_targetIActivatable.Deactivate ();
			meshRenderer.material.color = defaultColor;
		}
	}
}