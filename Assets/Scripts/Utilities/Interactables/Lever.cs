/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour, IActivatable {

	public bool Activated {
		get {
			return activated;
		}
	}

	public GameObject targetActivatable;
	private IActivatable _targetIActivatable;

	private Animator animator;
	private bool activated;

	private void Start () {
		animator = GetComponent<Animator> ();
		_targetIActivatable = targetActivatable.GetComponent<IActivatable> ();
	}

	public void Switch () {
		AudioManager.instance.PlaySound ("Switch");
		activated = !activated;
		animator.SetBool ("Activated", activated);
		if (activated) {
			_targetIActivatable.Activate ();
		} else {
			_targetIActivatable.Deactivate ();
		}
	}

	public void Activate () {
		activated = true;
		animator.SetBool ("Activated", activated);
		_targetIActivatable.Activate ();
	}

	public void Deactivate () {
		activated = false;
		animator.SetBool ("Activated", activated);
		_targetIActivatable.Deactivate ();
	}
}