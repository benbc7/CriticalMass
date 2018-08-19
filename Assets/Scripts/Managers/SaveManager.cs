/************************************************
Created By:		Ben Cutler
Company:		Tetricom Studios
Product:
Date:
*************************************************/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour {

	private SaveData saveData;
	protected string savePath;
	public SaveData defaultData;

	public void SaveDataToDisk (SaveData data) {
		saveData = data;
		savePath = Application.persistentDataPath + "/save.dat";
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (savePath);
		bf.Serialize (file, saveData);
		file.Close ();
	}

	public SaveData LoadDataFromDisk () {
		savePath = Application.persistentDataPath + "/save.dat";
		if (File.Exists (savePath)) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (savePath, FileMode.Open);
			saveData = (SaveData) bf.Deserialize (file);
			file.Close ();
			return saveData;
		} else {
			return defaultData;
		}
	}
}

[System.Serializable]
public class SaveData {
	public int numberOfLevelsUnlocked;
	public bool firstPlay;
	public bool tutorialUI;
	public float masterVolume;
	public float musicVolume;
	public float sfxVolume;
}