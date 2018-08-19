/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MonitorUIManager : MonoBehaviour {

	public Tab[] tabs;
	public LevelButton[] levelButtons;
	public UnityEngine.UI.Button loadButton;
	public Image loadingBar;
	public Color grayBackgroundColor;
    public Slider [] audioSliders;
    public AudioMixer audioMixer;
    public Toggle tutorialToggle;
    public GameObject [] tutorialElements;

	private int selectedLevelIndex = -1;
    private bool tutorialMode = true;

	private void Start () {
		for (int i = 0; i < levelButtons.Length; i++) {
			if (i < GameManager.instance.loadedData.numberOfLevelsUnlocked) {
				levelButtons [i].lockImage.enabled = false;
				levelButtons [i].button.interactable = true;
			}
		}

        audioSliders [0].value = GameManager.instance.loadedData.masterVolume;
        audioSliders [1].value = GameManager.instance.loadedData.musicVolume;
        audioSliders [2].value = GameManager.instance.loadedData.sfxVolume;
        for (int i = 0; i < audioSliders.Length; i++) {
            OnChangeVolume (i);
        }

        if (!GameManager.instance.loadedData.tutorialUI) {
            tutorialToggle.isOn = false;
            //OnToggleTutorialMode ();
        }
	}

    private void SaveSettings () {
        GameManager.instance.loadedData.tutorialUI = tutorialMode;
        GameManager.instance.loadedData.masterVolume = audioSliders [0].value;
        GameManager.instance.loadedData.musicVolume = audioSliders [1].value;
        GameManager.instance.loadedData.sfxVolume = audioSliders [2].value;
        GameManager.instance.SaveCurrentData ();
    }

    public void UnlockLevel () {
        for (int i = 0; i < levelButtons.Length; i++) {
            if (i < GameManager.instance.loadedData.numberOfLevelsUnlocked) {
                levelButtons [i].lockImage.enabled = false;
                levelButtons [i].button.interactable = true;
            }
        }
    }

	public void OnSelectTab (int index) {
		for (int i = 0; i < tabs.Length; i++) {
			tabs [i].tabPanel.SetActive (false);
			tabs [i].backgroundImage.color = grayBackgroundColor;
		}

		tabs [index].tabPanel.SetActive (true);
		tabs [index].backgroundImage.color = Color.clear;
	}

	public void OnSelectLevel (int levelIndex) {
		for (int i = 0; i < levelButtons.Length; i++) {
			levelButtons [i].backgroundImage.color = grayBackgroundColor;
		}

		if (selectedLevelIndex == -1) {
			loadButton.interactable = true;
		}
		levelButtons [levelIndex - 1].backgroundImage.color = Color.clear;
		selectedLevelIndex = levelIndex;
	}

	public void OnLoadSelectedLevel () {
		if (selectedLevelIndex != -1) {
            SaveSettings ();
			GameManager.instance.LoadLevel (selectedLevelIndex, loadingBar);
		}
	}

    public void OnToggleTutorialMode () {
        tutorialMode = !tutorialMode;
        for (int i = 0; i < tutorialElements.Length; i++) {
            tutorialElements [i].SetActive (tutorialMode);
        }
    }

    public void OnChangeVolume (int index) {
        audioMixer.SetFloat ("Volume" + index, audioSliders [index].value * 80 - 80);
	}

	[System.Serializable]
	public class Tab {
		public Image backgroundImage;
		public GameObject tabPanel;
	}

	[System.Serializable]
	public class LevelButton {
		public int sceneIndex;
		public UnityEngine.UI.Button button;
		public Image backgroundImage;
		public Image lockImage;
	}
}