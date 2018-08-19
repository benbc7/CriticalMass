using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

	[Header ("Movement")]
	public float moveSpeed = 5;
	public float runMultiplier = 1.5f;
	public float accelerationTimeGrounded = 0.1f;
	public Transform playerModel;
	public float modelRotationSmoothTime = 0.1f;
	public Transform handAnchor;

	[Header ("Jumping")]
	public float jumpHeight = 4;
	public float timeToJumpApex = 0.4f;
	public float accelerationTimeAirborne = 0.2f;

	private bool paused;
	private PlayerController playerController;
	private CameraController cameraController;
	private Transform cameraTransform;
	private HandController handController;
	private Vector3 moveVelocity;
	private float jumpVelocity;
	private float gravity;
	private float currentRunMultiplier = 1f;
	private float velocityXSmoothing;
	private float velocityZSmoothing;
	private float modelRotationVelocity;

	private void Start () {
		playerController = GetComponent<PlayerController> ();
		cameraController = GameObject.Find ("Main Camera").GetComponent<CameraController> ();
		cameraController.GetPlayer (transform);
		cameraTransform = cameraController.transform;
		handController = handAnchor.Find ("Hand").GetComponent<HandController> ();
		playerController.animator = playerModel.GetComponent<Animator> ();

		if (GameManager.instance != null) {
			GameManager.instance.pausePlayerEvent += PausePlayer;
		}

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
		Physics.gravity = Vector3.up * gravity;
	}

	private void Update () {
		if (!paused) {
			RunInput ();
			MoveInput ();
			LookInput ();
			HandInput ();
		}
	}

	public void PausePlayer () {
		paused = !paused;
		playerController.Move (Vector3.zero);
	}

	private void HandInput () {
		if (Input.GetMouseButtonDown (0)) {
			handController.PickUpObject ();
			playerController.animator.SetBool ("Lifting", true);
		}
		if (Input.GetMouseButtonUp (0)) {
			handController.DropObject ();
			playerController.animator.SetBool ("Lifting", false);
		}
	}

	private void LookInput () {

		//handAnchor.rotation = cameraTransform.rotation;
	}

	private void MoveInput () {
		Vector3 moveInput = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
		currentRunMultiplier = (moveInput.z >= 0) ? currentRunMultiplier : 1;
		Vector3 targetVelocity = transform.TransformDirection (moveInput.normalized) * moveSpeed * currentRunMultiplier;
		moveVelocity.x = Mathf.SmoothDamp (moveVelocity.x, targetVelocity.x, ref velocityXSmoothing, (playerController.grounded) ? accelerationTimeGrounded : accelerationTimeAirborne);
		moveVelocity.z = Mathf.SmoothDamp (moveVelocity.z, targetVelocity.z, ref velocityZSmoothing, (playerController.grounded) ? accelerationTimeGrounded : accelerationTimeAirborne);
		playerController.Move (moveVelocity);

		if (playerController.grounded && Input.GetKeyDown (KeyCode.Space)) {
			playerController.Jump (jumpVelocity);
		}

		float modelLookAngleY;
		if (moveInput != Vector3.zero) {
			playerController.animator.SetBool ("Walking", true);
			modelLookAngleY = Vector2.Angle (Vector2.up, moveVelocity.normalized.XZ ());
			modelLookAngleY *= Mathf.Sign (moveVelocity.x);
		} else {
			playerController.animator.SetBool ("Walking", false);
			modelLookAngleY = cameraTransform.eulerAngles.y;
		}

		playerModel.localPosition = Vector3.down;
		float smoothAngleY = Mathf.SmoothDampAngle (playerModel.eulerAngles.y, modelLookAngleY, ref modelRotationVelocity, modelRotationSmoothTime);
		handAnchor.rotation = Quaternion.Euler (cameraTransform.eulerAngles.x, smoothAngleY, cameraTransform.eulerAngles.z);
		playerModel.rotation = Quaternion.Euler (Vector3.up * smoothAngleY);
	}

	private void RunInput () {
		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			currentRunMultiplier = runMultiplier;
			cameraController.SetRunning ();
		}
		if (Input.GetKeyUp (KeyCode.LeftShift)) {
			currentRunMultiplier = 1;
			cameraController.SetRunning ();
		}
	}
}