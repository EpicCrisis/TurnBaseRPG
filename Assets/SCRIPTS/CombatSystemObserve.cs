using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatSystemObserve : MonoBehaviour
{
	//observe player and enemy stats
	//observe player and enemy actions
	//observe player and enemy status

	public CharacterTarget characterType;
	public NotificationType notificationType;
	public Text characterTitleText;
	public Text battleText;
	public Image characterHealthBar;
	public Image characterManaBar;

	public string characterNameString;
	public string battleString;

	public int characterMaxHealth = 0;
	public int characterCurrentHealth = 0;
	public int characterMaxMana = 0;
	public int characterCurrentMana = 0;
	public int characterPhysicalAttack = 0;
	public int characterPhysicalDefense = 0;
	public int characterMagicalAttack = 0;
	public int characterMagicalDefense = 0;
	public int characterAgility = 0;
	public int characterExperience = 0;
	public int characterLevel = 0;
	public int characterPoints = 0;

	GameManagerScript gameManager;

	public GameObject JackScott;
	public GameObject BigMacD;
	public GameObject LilMacD;
	public GameObject Rando;
	public GameObject BossaNova;

	//turn based combat
	public int playerTurn = 0;
	public int turnCounter = 0;

	void Start ()
	{
		gameManager = FindObjectOfType <GameManagerScript> ();

		InitializeCharacter ();
	}

	void Update ()
	{
		
	}

	//which character is being targeted
	public void NotifyCharacter (CharacterTarget type)
	{
		if (characterCurrentHealth > characterMaxHealth) {
			characterCurrentHealth = characterMaxHealth;
		}
		if (characterCurrentMana > characterMaxMana) {
			characterCurrentMana = characterMaxMana;
		}
		if (characterCurrentHealth < 1) {
			if (characterType == CharacterTarget.PLAYER) {
				//set active the restart button
				gameManager.curState = GameStates.GAMEOVER;
			}
			/*
			if (characterType == CharacterTarget.JACKSCOTT) {
				JackScott.SetActive (false);
			}
			if (characterType == CharacterTarget.BIGMACD) {
				BigMacD.SetActive (false);
			}
			if (characterType == CharacterTarget.LILMACD) {
				LilMacD.SetActive (false);
			}
			if (characterType == CharacterTarget.RANDO) {
				Rando.SetActive (false);
			}
			if (characterType == CharacterTarget.BOSSANOVA) {
				BossaNova.SetActive (false);
			}
			*/
			battleText.text = characterNameString + " Has Died!";

			characterCurrentHealth = 0;
			characterHealthBar.fillAmount = (float)characterCurrentHealth / (float)characterMaxHealth;
		}
		if (characterCurrentMana < 1) {
			
			Debug.Log ("No Mana");

			characterCurrentMana = 0;
			characterManaBar.fillAmount = (float)characterCurrentMana / (float)characterMaxMana;
		}
		if (characterType == type) {
			characterHealthBar.fillAmount = (float)characterCurrentHealth / (float)characterMaxHealth;
			characterManaBar.fillAmount = (float)characterCurrentMana / (float)characterMaxMana;
		}
	}

	//notify when action is taken
	//	public void NotifyAction ( type)
	//	{
	//		//deal damage formula, return damage values to character
	//		if (characterType == CharacterTarget.PLAYER) {
	//
	//			if (type == ActionType.QUICK_STRIKE) {
	//				//NEED ATTENTION FOR FIX
	//				int tempDamage = characterPhysicalAttack * 4 - characterPhysicalDefense * 3;
	//				characterCurrentHealth -= tempDamage;
	//				battleText.text = characterNameString + " Used Quick Strike On " + characterNameString + " And Dealt " + tempDamage + " Damage!";
	//			}
	//			if (type == ActionType.CRITICAL_HIT) {
	//				Debug.Log ("CRITICAL HIT TO ENEMY");
	//			}
	//			if (type == ActionType.STUN_BASH) {
	//				Debug.Log ("STUN BASH TO ENEMY");
	//			}
	//			if (type == ActionType.FIREBALL) {
	//				Debug.Log ("FIREBALL TO ENEMY");
	//			}
	//			if (type == ActionType.DECAY) {
	//				Debug.Log ("DECAY TO ENEMY");
	//			}
	//			if (type == ActionType.DEFEND) {
	//				Debug.Log ("DEFEND PLAYER");
	//			}
	//		}
	//
	//		gameManager.fightEmpty1.SetActive (false);
	//		gameManager.fightEmpty2.SetActive (false);
	//
	//		playerTurn++;
	//		turnCounter++;
	//		playerTurn %= 2;
	//	}
		
	//show character stats
	public void InitializeCharacter ()
	{
		//CombatSystem.instance.SubscribeObserver (this);
		characterTitleText.text = characterNameString;
		battleText.text = "";
		characterHealthBar.fillAmount = (float)characterCurrentHealth / (float)characterMaxHealth;
		characterManaBar.fillAmount = (float)characterCurrentMana / (float)characterMaxMana;
	}

	public void CheckCharacterStatus ()
	{

	}
}

/*
public class AchievementObserverScript : MonoBehaviour
{

	public AchievementType myType;
	public Text achievementTitleText;
	public Image achievementProgressBar;
	public string achievementName;

	public int achievementMaxCount = 1;
	int achievementCount = 0;

	void Start ()
	{
		InitializeAchievements ();
	}

	void Update ()
	{

	}

	public void Notify (AchievementType type, GameObject )
	{
		if (myType == type) {
			achievementCount++;
			achievementProgressBar.fillAmount = (float)achievementCount / (float)achievementMaxCount;
			if (achievementCount >= achievementMaxCount) {
				AchievementSubjectScript.Instance.UnSubscribeObserver (this);
			}
		}
	}

	public void InitializeAchievements ()
	{
		AchievementSubjectScript.Instance.SubscribeObserver (this);
		achievementTitleText.text = achievementName;
		achievementProgressBar.fillAmount = achievementCount / achievementMaxCount;
	}
}
*/
