using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (SaveManager))]
public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public HubDoor hubDoor;
    private Vector3 nextLevelSpawnPosition;

	public delegate void PausePlayerDelegate ();

	public event PausePlayerDelegate pausePlayerEvent;

	private SaveManager saveManager;

	[HideInInspector]
	public SaveData loadedData;

	private List<int> loadedSceneIndecies = new List<int> ();
	private int activeSceneIndex;

	private void Awake () {
		loadedSceneIndecies.Add (SceneManager.GetActiveScene ().buildIndex);
		activeSceneIndex = loadedSceneIndecies [0];
		if (instance == null) {
			instance = this;
			LoadSaveData ();
		} else {
			Destroy (gameObject);
		}
	}

    public void SaveCurrentData () {
        saveManager.SaveDataToDisk (loadedData);
    }

	private void LoadSaveData () {
		saveManager = GetComponent<SaveManager> ();
		loadedData = saveManager.LoadDataFromDisk ();
	}

	public void PausePlayer () {
		if (pausePlayerEvent != null) {
			pausePlayerEvent ();
		}
	}

	public void LoadLevel (int sceneIndex, Image loadingBarFillImage = null) {
        if (sceneIndex < SceneManager.sceneCountInBuildSettings) {
            activeSceneIndex = sceneIndex;
            loadedSceneIndecies.Add (sceneIndex);
			StartCoroutine (LoadNewAdditiveLevel (sceneIndex, null, loadingBarFillImage));
		}
	}

    public void LoadNextLevel (HubDoor hubDoor) {
        if (activeSceneIndex + 1 < SceneManager.sceneCountInBuildSettings) {
            activeSceneIndex++;
            loadedSceneIndecies.Add (activeSceneIndex);
            StartCoroutine (LoadNewAdditiveLevel (activeSceneIndex, hubDoor));
            if (activeSceneIndex > loadedData.numberOfLevelsUnlocked) {
                loadedData.numberOfLevelsUnlocked++;
                FindObjectOfType<MonitorUIManager> ().UnlockLevel ();
                SaveCurrentData ();
            }
        }
    }

    private IEnumerator LoadNewAdditiveLevel (int sceneIndex, HubDoor _hubDoor = null, Image loadingBarFillImage = null) {
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync (sceneIndex, LoadSceneMode.Additive);

		while (!asyncLoad.isDone) {
			if (loadingBarFillImage != null) {
				loadingBarFillImage.fillAmount = asyncLoad.progress;
			}
			yield return null;
		}

		if (loadingBarFillImage != null) {
			loadingBarFillImage.fillAmount = 1;
		}

        LevelManager levelManager = FindObjectOfType<LevelManager> ();
        nextLevelSpawnPosition = levelManager.InitializeLevel (nextLevelSpawnPosition);
		Destroy (levelManager);

        if (_hubDoor == null) {
            hubDoor.SceneHasLoaded ();
        } else {
            _hubDoor.SceneHasLoaded ();
        }
	}

	private IEnumerator UnloadLevel (int sceneIndex) {
		AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync (sceneIndex);
		hubDoor.SceneUnloaded ();

		while (!asyncUnload.isDone) {
			yield return null;
		}
	}

	public void OnLoadHubWorld () {
		SceneManager.LoadScene (0);
	}

	public void OnExit () {
		Application.Quit ();
	}
}