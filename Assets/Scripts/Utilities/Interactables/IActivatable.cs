/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivatable {

	bool Activated {
		get;
	}

	void Switch ();

	void Activate ();

	void Deactivate ();
}