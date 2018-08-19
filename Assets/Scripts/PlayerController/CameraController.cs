using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private const float Y_ANGLE_MIN = -15f;
	private const float Y_ANGLE_MAX = 50f;

	[Header ("Camera Behaviour")]
	public GameObject pauseCanvas;
	public LayerMask layerMask;
	public float camHeight = 1f;

	public float sensitivityX = 0f;
	public float sensitivityY = 0f;

	[Header ("Field of View")]
	public float defaultFOV = 68f;

	public float runningFOV = 80f;
	public float fovSmoothTime = 0.2f;

	[Header ("Monitor Mode")]
	public Transform target;
	public Transform canvas;
	public float monitorModeSmoothTime = 0.2f;

	private Vector3 originalPosition;
	private Quaternion originalRotation;
	private Vector3 monitorModeVelocity;
	private bool monitorMode;
	private bool paused;

	private float currentFOV;
	private bool running = false;
	private float smoothVelocity;

	private Transform player;
	private Transform camTransform;

	private Camera cam;

	private float maxDistance = 7.5f;
	private float distance = 0f;
	private float currentX = 0f;
	private float currentY = 0f;

	public void GetPlayer (Transform playerTransform) {
		player = playerTransform;
	}

	private void Start () {
		camTransform = transform;
		cam = GetComponent<Camera> ();
		currentFOV = defaultFOV;
		cam.fieldOfView = currentFOV;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void SetRunning () {
		running = !running;
		StopCoroutine (FOVChange ());
		StartCoroutine (FOVChange ());
	}

	private IEnumerator FOVChange () {
		while (true) {
			currentFOV = Mathf.SmoothDamp (currentFOV, (running) ? runningFOV : defaultFOV, ref smoothVelocity, fovSmoothTime);
			cam.fieldOfView = currentFOV;
			if (running && currentFOV >= runningFOV - 1) {
				break;
			} else if (!running && currentFOV <= defaultFOV + 1) {
				break;
			}
			yield return null;
		}
	}

	public void SetMonitorMode () {
		monitorMode = !monitorMode;
		StopCoroutine (MonitorModeChange ());
		if (monitorMode) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			originalPosition = transform.position;
			originalRotation = transform.rotation;
			StartCoroutine (MonitorModeChange ());
		} else {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	private IEnumerator MonitorModeChange () {
		while (monitorMode && transform.position != target.position) {
			Vector3 newPosition = Vector3.SmoothDamp (transform.position, target.position, ref monitorModeVelocity, monitorModeSmoothTime);
			transform.position = newPosition;
			transform.LookAt (canvas);
			yield return null;
		}
	}

	private void Update () {
		if (player != null && !monitorMode && !paused) {
			CameraInput ();
		} else if (player != null && paused) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				OnResume ();
			}
		}
	}

	public void OnResume () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		paused = false;
		GameManager.instance.PausePlayer ();
		pauseCanvas.SetActive (false);
	}

	private void CameraInput () {
		RaycastHit hit;

		if (Input.GetKeyDown (KeyCode.Escape) && !monitorMode) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			paused = true;
			GameManager.instance.PausePlayer ();
			pauseCanvas.SetActive (true);
		}

		currentX += Input.GetAxis ("Mouse X") * sensitivityX;
		currentY += -Input.GetAxis ("Mouse Y") * sensitivityY;

		currentY = Mathf.Clamp (currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);

		Debug.DrawRay (player.position, transform.TransformDirection (Vector3.back) * maxDistance, Color.red);
		if (Physics.Raycast (player.position, transform.TransformDirection (Vector3.back), out hit, maxDistance, layerMask)) {
			distance = hit.distance;
		} else
			distance = maxDistance;
	}

	private void LateUpdate () {
		if (player != null && !monitorMode) {
			CameraMovement ();
		}
	}

	private void CameraMovement () {
		Vector3 dir = new Vector3 (0, 0, -distance);
		Quaternion rotation = Quaternion.Euler (currentY, currentX, 0);
		camTransform.position = player.position + rotation * dir + Vector3.up * camHeight;

		camTransform.LookAt (player.position + Vector3.up * camHeight);
		player.rotation = Quaternion.Euler (0, currentX, 0);
	}
}