using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
	OVERWORLD = 0,
	BATTLE,
	PAUSE,
	CHARACTER_INVENTORY,
	CHARACTER_SELECTION,
	GAMEOVER,
	NONE,
}

public class GameManagerScript : MonoBehaviour
{
	public static GameManagerScript Instance;
	public GameStates curState = GameStates.NONE;

	public GameObject battleScreen;

	public GameObject fightEmpty1;

	public GameObject RestartButton;

	public GameObject characterInventory;

	public GameObject selectionScreen;

	InventorySystem inventorySystem;

	public int enemyCounter = 0;

	public bool doOnce = false;

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (this.gameObject);
		}

		//DontDestroyOnLoad (this.gameObject);
	}

	void Start ()
	{
		curState = GameStates.CHARACTER_SELECTION;

		RestartButton.SetActive (false);

		characterInventory.SetActive (false);

		fightEmpty1.SetActive (false);

		inventorySystem = FindObjectOfType<InventorySystem> ();
	}

	//battlescreen will show sprite of player and enemy
	//player can choose buttons attack, defend, run with mouse button
	//need to find a way to correspond the buttons to functions and states

	void Update ()
	{
		CharacterSelect ();
		EnterCombat ();
		AccessCharacterInventory ();
		VictoryFunction ();
	}

	void EnterCombat ()
	{
		if (curState == GameStates.BATTLE) {
			battleScreen.SetActive (true);
			//Debug.Log ("Check Battle");
		} else {
			battleScreen.SetActive (false);
		}
	}

	void CharacterSelect ()
	{
		if (curState == GameStates.CHARACTER_SELECTION) {
			selectionScreen.SetActive (true);
		} else {
			selectionScreen.SetActive (false);
		}
	}

	void AccessCharacterInventory ()
	{
		if (Input.GetKeyDown (KeyCode.I)) {
			if (curState == GameStates.OVERWORLD) {
				curState = GameStates.CHARACTER_INVENTORY;
				characterInventory.SetActive (true);
			} else if (curState == GameStates.CHARACTER_INVENTORY) {
				curState = GameStates.OVERWORLD;
				characterInventory.SetActive (false);
			}
		}
	}

	void VictoryFunction ()
	{
		if (curState == GameStates.OVERWORLD) {
			if (enemyCounter == 5) {
				RestartButton.SetActive (true);
			} else {
				RestartButton.SetActive (false);
			}
		}
	}
}
