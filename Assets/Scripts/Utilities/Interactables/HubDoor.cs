/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubDoor : MonoBehaviour, IActivatable {

	private Animator animator;
	private bool _open;
	private bool levelHasLoaded;

	public bool Activated {
		get {
			return _open;
		}
	}

	private void Start () {
		animator = GetComponent<Animator> ();
	}

	public void SceneHasLoaded () {
		levelHasLoaded = true;
		if (_open) {
			animator.SetBool ("Open", _open);
			AudioManager.instance.PlaySound ("Door");
		}
	}

	public void SceneUnloaded () {
		levelHasLoaded = false;
		if (_open) {
			_open = false;
			animator.SetBool ("Open", _open);
			AudioManager.instance.PlaySound ("Door");
		}
	}

	public void Switch () {
	}

	public void Activate () {
		_open = true;
		if (levelHasLoaded) {
			animator.SetBool ("Open", _open);
			AudioManager.instance.PlaySound ("Door");
		}
	}

	public void Deactivate () {
		_open = false;
		animator.SetBool ("Open", _open);
		AudioManager.instance.PlaySound ("Door");
	}
}