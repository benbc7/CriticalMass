/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour, IActivatable {

	private Animator animator;
	private bool _open;

	public bool Activated {
		get {
			return _open;
		}
	}

	private void Start () {
		animator = GetComponent<Animator> ();
	}

	public void Switch () {
		_open = !_open;
		animator.SetBool ("Open", _open);
		AudioManager.instance.PlaySound ("Door");
	}

	public void Activate () {
		_open = true;
		animator.SetBool ("Open", _open);
		AudioManager.instance.PlaySound ("Door");
	}

	public void Deactivate () {
		_open = false;
		animator.SetBool ("Open", _open);
		AudioManager.instance.PlaySound ("Door");
	}
}