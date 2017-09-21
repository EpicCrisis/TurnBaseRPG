using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClassSelect
{
	KNIGHT = 0,
	MAGE,
	ROGUE,
	NONE,
};

public class CharacterSelectionScript : MonoBehaviour
{

	PlayerScript Player;

	GameManagerScript gameManager;

	public static CharacterSelectionScript instance;
	public ClassSelect curClass = ClassSelect.NONE;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}
	}

	void Start ()
	{
		gameManager = FindObjectOfType<GameManagerScript> ();

		Player = FindObjectOfType<PlayerScript> ();
	}

	void Update ()
	{
		
	}

	//	Player.characterMaxHealth = 100;
	//	Player.characterCurrentHealth = 100;
	//	Player.characterMaxMana = 100;
	//	Player.characterCurrentMana = 100;
	//	Player.characterBasePhysicalAttack = 10;
	//	Player.characterBasePhysicalDefense = 10;
	//	Player.characterBaseMagicalAttack = 10;
	//	Player.characterBaseMagicalDefense = 10;

	public void ChooseKnight ()
	{
		curClass = ClassSelect.KNIGHT;
		InitializeCharacter (curClass);
		gameManager.curState = GameStates.OVERWORLD;
	}

	public void ChooseMage ()
	{
		curClass = ClassSelect.MAGE;
		InitializeCharacter (curClass);
		gameManager.curState = GameStates.OVERWORLD;
	}

	public void ChooseRogue ()
	{
		curClass = ClassSelect.ROGUE;
		InitializeCharacter (curClass);
		gameManager.curState = GameStates.OVERWORLD;
	}

	void InitializeCharacter (ClassSelect selectedClass)
	{
		if (selectedClass == ClassSelect.KNIGHT) {

			Player.characterMaxHealth = 120;
			Player.characterCurrentHealth = 120;
			Player.characterMaxMana = 80;
			Player.characterCurrentMana = 80;
			Player.characterBasePhysicalAttack = 12;
			Player.characterBasePhysicalDefense = 14;
			Player.characterBaseMagicalAttack = 8;
			Player.characterBaseMagicalDefense = 6;

		} else if (selectedClass == ClassSelect.MAGE) {

			Player.characterMaxHealth = 80;
			Player.characterCurrentHealth = 80;
			Player.characterMaxMana = 120;
			Player.characterCurrentMana = 120;
			Player.characterBasePhysicalAttack = 8;
			Player.characterBasePhysicalDefense = 6;
			Player.characterBaseMagicalAttack = 12;
			Player.characterBaseMagicalDefense = 14;

		} else if (selectedClass == ClassSelect.ROGUE) {

			Player.characterMaxHealth = 100;
			Player.characterCurrentHealth = 100;
			Player.characterMaxMana = 100;
			Player.characterCurrentMana = 100;
			Player.characterBasePhysicalAttack = 14;
			Player.characterBasePhysicalDefense = 6;
			Player.characterBaseMagicalAttack = 14;
			Player.characterBaseMagicalDefense = 6;

		}
	}
}
