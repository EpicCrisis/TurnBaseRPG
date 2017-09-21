using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum CharacterTarget
{
	PLAYER = 0,
	ENEMY,
	NONE,
};

public enum NotificationType
{
	HEALTH,
	MANA,
	DEFEND,
	BURN,
	STUN,
	WEAK,
	BREAK,
	NONE,
};

public enum CharacterStatus
{
	DEFEND = 0,
	//damage over time
	BURN,
	//skip turn
	STUN,
	//decrease attack
	WEAK,
	//decrease defense
	BREAK,
	NONE,
};

/*
public enum ItemType
{
	HEALTH_POTION = 0,
	MANA_POTION,
	DAGGER,
	ARMOUR,
	NONE,
};
*/

public class CombatSystem : MonoBehaviour
{

	public List <ObserverScript> ObserverList = new List<ObserverScript> ();

	public static CombatSystem instance;

	GameManagerScript gameManager;

	public Text battleTextPlayer;
	public Text battleTextEnemy;

	public CharacterAttributesScript Player;

	public CharacterAttributesScript JackScott;
	public CharacterAttributesScript BigMacD;
	public CharacterAttributesScript LilMacD;
	public CharacterAttributesScript Rando;
	public CharacterAttributesScript BossaNova;

	public CharacterAttributesScript CurrentEnemy;

	public GameObject JackScottObject;
	public GameObject BigMacDObject;
	public GameObject LilMacDObject;
	public GameObject RandoObject;
	public GameObject BossaNovaObject;

	public GameObject CurrentEnemyObject;

	public GameObject FightButton;
	public GameObject DefendButton;
	public GameObject RunButton;

	public GameObject WinButton;
	public GameObject LoseButton;

	public Image EnemyButton;

	InventorySystem inventorySystem;

	//turn based combat
	public int playerTurn = 0;
	public int turnCounter = 0;

	public float delayAction = 1f;
	public float delayActionTimer = 0f;

	public bool doOnce = false;

	void Awake ()
	{
		instance = this;

		/*
		if (instance = null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}
		
		DontDestroyOnLoad (this.gameObject);
		*/
	}

	void Start ()
	{
		gameManager = FindObjectOfType <GameManagerScript> ();

		inventorySystem = FindObjectOfType <InventorySystem> ();

		WinButton.SetActive (false);
		LoseButton.SetActive (false);

		battleTextPlayer.text = "";
		battleTextEnemy.text = "";
	}

	void Update ()
	{
		GameAIFunction ();
		playerTurn %= 2;
	}

	public void WinFunction ()
	{
		//press continue and return to overworld
		//if not enemies left, show restart button
		playerTurn = 0;
		ResetStatus ();
		CleanUp ();
		CurrentEnemyObject.SetActive (false);
		WinButton.SetActive (false);
		gameManager.doOnce = false;
		doOnce = false;
		gameManager.curState = GameStates.OVERWORLD;
	}

	public void LoseFunction ()
	{
		//refresh the scene
		Scene loadedLevel = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (loadedLevel.buildIndex);
	}

	void CleanUp ()
	{
		battleTextPlayer.text = "";
		battleTextEnemy.text = "";
		gameManager.fightEmpty1.SetActive (false);
	}

	void EnableButtons ()
	{
		FightButton.SetActive (true);
		DefendButton.SetActive (true);
		RunButton.SetActive (true);
	}

	void DisableButtons ()
	{
		FightButton.SetActive (false);
		DefendButton.SetActive (false);
		RunButton.SetActive (false);
	}

	void ResetStatus ()
	{
		Player.characterDefendCounter = 0;
		Player.characterStunCounter = 0;
		Player.characterBurnCounter = 0;
		Player.characterWeakCounter = 0;
		Player.characterBreakCounter = 0;

		CurrentEnemy.characterDefendCounter = 0;
		CurrentEnemy.characterStunCounter = 0;
		CurrentEnemy.characterBurnCounter = 0;
		CurrentEnemy.characterWeakCounter = 0;
		CurrentEnemy.characterBreakCounter = 0;

		Player.CriticalStrikeCooldown = 0;
		Player.StunBashCooldown = 0;
		Player.FireballCooldown = 0;
		Player.DecayCooldown = 0;

		CurrentEnemy.CriticalStrikeCooldown = 0;
		CurrentEnemy.StunBashCooldown = 0;
		CurrentEnemy.FireballCooldown = 0;
		CurrentEnemy.DecayCooldown = 0;

		CountdownTurnStatus (Player, CurrentEnemy);
		CountdownTurnStatus (CurrentEnemy, Player);
	}

	void CountdownTurnStatus (CharacterAttributesScript target, CharacterAttributesScript other)
	{
		if (other.characterDefendCounter > 0) {
			other.characterDefendCounter -= 1;
			other.isDefend = true;
			Notify (other.myCharacterType, NotificationType.DEFEND, other.gameObject);
		} 
		if (other.characterDefendCounter == 0 && other.isDefend) {
			other.characterCurrentPhysicalDefense -= other.characterBasePhysicalDefense * 1 / 3;
			other.isDefend = false;
			Notify (other.myCharacterType, NotificationType.DEFEND, other.gameObject);
		}

		if (target.characterStunCounter > 0) {
			target.characterStunCounter -= 1;
			target.isStun = true;
			Notify (target.myCharacterType, NotificationType.STUN, target.gameObject);
		}
		if (target.characterStunCounter == 0 && target.isStun) {
			target.isStun = false;
			Notify (target.myCharacterType, NotificationType.STUN, target.gameObject);
			target.characterBattleText.text += " " + target.characterNameString + " Has Recovered From Stun!";
		}

		if (target.characterBurnCounter > 0) {
			target.characterBurnCounter -= 1;
			int tempDamage = other.characterTotalMagicalAttack * 1 / 2;
			target.characterCurrentHealth -= tempDamage;
			target.characterBattleText.text += " " + target.characterNameString + " Is Burning For " + tempDamage + " Damage!";
			target.isBurn = true;
			Notify (target.myCharacterType, NotificationType.HEALTH, target.gameObject);
			Notify (target.myCharacterType, NotificationType.BURN, target.gameObject);
		}
		if (target.characterBurnCounter == 0 && target.isBurn) {
			target.isBurn = false;
			Notify (target.myCharacterType, NotificationType.BURN, target.gameObject);
		}

		if (target.characterWeakCounter > 0) {
			target.characterWeakCounter -= 1;
			target.isWeak = true;
			Notify (target.myCharacterType, NotificationType.WEAK, target.gameObject);
		}
		if (target.characterWeakCounter == 0 && target.isWeak) {
			target.characterCurrentPhysicalAttack += target.characterBasePhysicalAttack * 1 / 4;
			target.isWeak = false;
			Notify (target.myCharacterType, NotificationType.WEAK, target.gameObject);
		}

		if (target.characterBreakCounter > 0) {
			target.characterBreakCounter -= 1;
			target.isBreak = true;
			Notify (target.myCharacterType, NotificationType.BREAK, target.gameObject);
		}
		if (target.characterBreakCounter == 0 && target.isBreak) {
			target.characterCurrentPhysicalDefense += target.characterBasePhysicalDefense * 1 / 4;
			target.isBreak = false;
			Notify (target.myCharacterType, NotificationType.BREAK, target.gameObject);
		}

		if (target.CriticalStrikeCooldown > 0) {
			target.CriticalStrikeCooldown -= 1;
		}
		if (target.StunBashCooldown > 0) {
			target.StunBashCooldown -= 1;
		}
		if (target.FireballCooldown > 0) {
			target.FireballCooldown -= 1;
		}
		if (target.DecayCooldown > 0) {
			target.DecayCooldown -= 1;
		}
	}

	public void GameAIFunction ()
	{
		int randomAttack;

		Notify (CharacterTarget.PLAYER, NotificationType.HEALTH, Player.gameObject);
		Notify (CharacterTarget.PLAYER, NotificationType.MANA, Player.gameObject);
		Notify (CharacterTarget.ENEMY, NotificationType.HEALTH, CurrentEnemy.gameObject);
		Notify (CharacterTarget.ENEMY, NotificationType.MANA, CurrentEnemy.gameObject);

		CurrentEnemy.characterNameText.text = CurrentEnemy.characterNameString;
		if (CurrentEnemy == BossaNova) {
			EnemyButton.color = Color.yellow;
		} else {
			EnemyButton.color = Color.red;
		}

		if (Player.characterCurrentHealth <= 0 && !doOnce) {
			//Debug.Log ("PLAYER DIED");
			battleTextPlayer.text += " " + Player.characterNameString + " Has Died!";
			//enable restart button, disable other buttons
			DisableButtons ();
			LoseButton.SetActive (true);
			WinButton.SetActive (false);
			doOnce = true;

		} else if (CurrentEnemy.characterCurrentHealth <= 0 && !doOnce) {
			//Debug.Log ("ENEMY DIED");
			battleTextEnemy.text += " " + CurrentEnemy.characterNameString + " Has Died!";
			//enable continue button, disable other buttons
			DisableButtons ();
			WinButton.SetActive (true);
			doOnce = true;

			if (!gameManager.doOnce) {
				
				gameManager.enemyCounter += 1;

				Player.characterExperience += CurrentEnemy.characterExperience;
				battleTextPlayer.text += " " + Player.characterNameString + " Gained " + CurrentEnemy.characterExperience + " Experience!";

				int randomLoot = Random.Range (0, 100);

				if (randomLoot >= 0 && randomLoot <= 30) {
					Player.HealthPotion += CurrentEnemy.HealthPotion;
					battleTextPlayer.text += " " + "Found " + CurrentEnemy.HealthPotion + " Health Potions!";
				}
				if (randomLoot >= 20 && randomLoot <= 60) {
					Player.ManaPotion += CurrentEnemy.ManaPotion;
					battleTextPlayer.text += " " + "Found " + CurrentEnemy.ManaPotion + " Mana Potions!";
				}
				if (randomLoot >= 50 && randomLoot <= 70) {
					Player.ExperiencePotion += CurrentEnemy.ExperiencePotion;
					battleTextPlayer.text += " " + "Found " + CurrentEnemy.ExperiencePotion + " Experience Potions!";
				}
				if (randomLoot > 70) {
					Debug.Log ("NOTHING");
				}

				gameManager.doOnce = true;
			}
		}

		if (Player.characterCurrentHealth > 0 && CurrentEnemy.characterCurrentHealth > 0) {
			if (playerTurn == 1) {

				//DisableButtons ();

				if (delayActionTimer <= delayAction) {
					delayActionTimer += Time.deltaTime;
				} else {
					delayActionTimer = 0f;

					if (CurrentEnemy.characterStunCounter < 1) {
						do {
							randomAttack = Random.Range (0, 100);

							if (randomAttack >= 0 && randomAttack <= 30) {
								QuickStrikeFunction ();
							} 
							if (randomAttack >= 31 && randomAttack <= 50) {
								if (CurrentEnemy.CriticalStrikeCooldown > 0 || CurrentEnemy.characterCurrentMana < CurrentEnemy.CriticalStrikeMana) {
									randomAttack = 555;
									Debug.Log ("Return To Loop");
								} else {
									CriticalHitFunction ();
								}
							} 
							if (randomAttack >= 51 && randomAttack <= 60) {
								if (CurrentEnemy.StunBashCooldown > 0 || CurrentEnemy.characterCurrentMana < CurrentEnemy.StunBashMana) {
									randomAttack = 555;
									Debug.Log ("Return To Loop");
								} else {
									StunBashFunction ();
								}
							} 
							if (randomAttack >= 61 && randomAttack <= 70) {
								if (CurrentEnemy.FireballCooldown > 0 || CurrentEnemy.characterCurrentMana < CurrentEnemy.FireballMana) {
									randomAttack = 555;
									Debug.Log ("Return To Loop");
								} else {
									FireballFunction ();
								}
							} 
							if (randomAttack >= 71 && randomAttack <= 80) {
								if (CurrentEnemy.DecayCooldown > 0 || CurrentEnemy.characterCurrentMana < CurrentEnemy.DecayMana) {
									randomAttack = 555;
									Debug.Log ("Return To Loop");
								} else {
									DecayFunction ();
								}
							} 
							if (randomAttack > 80) {
								DefendFunction ();
							}
						} while (randomAttack == 555);
					} else {
						battleTextEnemy.text = CurrentEnemy.characterNameString + " Is Stunned!";
					}

					CountdownTurnStatus (CurrentEnemy, Player);

					playerTurn++;
				}
			} else if (playerTurn == 0 && Player.characterCurrentHealth > 0) {

				if (Player.characterStunCounter > 0) {

					DisableButtons ();

					battleTextPlayer.text = Player.characterNameString + " Is Stunned!";

					if (delayActionTimer <= delayAction) {
						
						delayActionTimer += Time.deltaTime;

					} else {
						
						delayActionTimer = 0f;

						CountdownTurnStatus (Player, CurrentEnemy);

						playerTurn++;
					}

				} else {

					EnableButtons ();

				}
			} else {
				
				DisableButtons ();

			}
		} else {
			return;
		}
	}

	//button press to initiate attack skills
	public void FightFunction ()
	{
		if (playerTurn == 0) {
			//Debug.Log ("Click Fight");
			gameManager.fightEmpty1.SetActive (true);
		} else {
			return;
		}
	}

	//button press to exit battle screen
	public void RunFunction ()
	{
		if (playerTurn == 0) {
			//Debug.Log ("ExitBattle");
			ResetStatus ();
			CleanUp ();
			gameManager.curState = GameStates.OVERWORLD;
		} else {
			return;
		}
	}

	public void DefendFunction ()
	{
		if (playerTurn == 0) {
			Player.characterCurrentPhysicalDefense += Player.characterBasePhysicalDefense * 1 / 3;
			Player.characterDefendCounter += 1;

			//Debug.Log ("DEFEND");
			battleTextPlayer.text = Player.characterNameString + " Used Defend & Gained " + Player.characterCurrentPhysicalDefense + " Defense!";

			CountdownTurnStatus (Player, CurrentEnemy);

			gameManager.fightEmpty1.SetActive (false);
			DisableButtons ();

			Player.isDefend = true;

			Notify (CharacterTarget.PLAYER, NotificationType.DEFEND, Player.gameObject);

			playerTurn++;

		} else if (playerTurn == 1) {
			CurrentEnemy.characterCurrentPhysicalDefense += CurrentEnemy.characterBasePhysicalDefense * 1 / 3;
			CurrentEnemy.characterDefendCounter += 1;

			//Debug.Log ("ENEMY DEFEND");
			battleTextEnemy.text = CurrentEnemy.characterNameString + " Used Defend & Gained " + CurrentEnemy.characterCurrentPhysicalDefense + " Defense!";
			//Notify (CharacterTarget.BOSSANOVA, ActionType.DEFEND, BossaNova.gameObject, ItemType.NONE);

			CurrentEnemy.isDefend = true;

			Notify (CharacterTarget.ENEMY, NotificationType.DEFEND, CurrentEnemy.gameObject);
		}
	}

	public void QuickStrikeFunction ()
	{
		if (playerTurn == 0) {
			int tempDamage = Player.characterTotalPhysicalAttack * 4 - CurrentEnemy.characterTotalPhysicalDefense * 3;
			if (tempDamage < 0) {
				tempDamage = 0;
			}
			CurrentEnemy.characterCurrentHealth -= tempDamage;

			battleTextPlayer.text = Player.characterNameString + " Used Quick Strike & Dealt " + tempDamage + " Damage To " + CurrentEnemy.characterNameString + "!";

			CountdownTurnStatus (Player, CurrentEnemy);

			gameManager.fightEmpty1.SetActive (false);
			DisableButtons ();

			Notify (CharacterTarget.ENEMY, NotificationType.HEALTH, CurrentEnemy.gameObject);

			playerTurn++;

		} else if (playerTurn == 1) {
			int tempDamage = CurrentEnemy.characterTotalPhysicalAttack * 4 - Player.characterTotalPhysicalDefense * 3;
			if (tempDamage < 0) {
				tempDamage = 0;
			}
			Player.characterCurrentHealth -= tempDamage;

			battleTextEnemy.text = CurrentEnemy.characterNameString + " Used Quick Strike & Dealt " + tempDamage + " Damage To " + Player.characterNameString + "!";

			Notify (CharacterTarget.PLAYER, NotificationType.HEALTH, Player.gameObject);
		}
	}

	public void CriticalHitFunction ()
	{
		if (playerTurn == 0) {
			if (Player.CriticalStrikeCooldown == 0) {
				if (Player.characterCurrentMana >= Player.CriticalStrikeMana) {
					int tempDamage = Player.characterTotalPhysicalAttack * 5 - CurrentEnemy.characterTotalPhysicalDefense * 3;
					if (tempDamage < 0) {
						tempDamage = 0;
					}
					CurrentEnemy.characterCurrentHealth -= tempDamage;

					battleTextPlayer.text = Player.characterNameString + " Used Critical Strike & Dealt " + tempDamage + " Damage To " + CurrentEnemy.characterNameString + "!";

					Player.CriticalStrikeCooldown += 2;
					Player.characterCurrentMana -= Player.CriticalStrikeMana;

					CountdownTurnStatus (Player, CurrentEnemy);

					gameManager.fightEmpty1.SetActive (false);
					DisableButtons ();

					Notify (CharacterTarget.ENEMY, NotificationType.HEALTH, CurrentEnemy.gameObject);
					Notify (CharacterTarget.PLAYER, NotificationType.MANA, Player.gameObject);

					playerTurn++;

				} else {
					battleTextPlayer.text = Player.CriticalStrikeMana + " Mana Required To Use Critical Strike!";
				}
			} else {
				battleTextPlayer.text = "Critical Strike Has " + Player.CriticalStrikeCooldown + " Turns Left Before Usable!";
			}

		} else if (playerTurn == 1) {
			int tempDamage = CurrentEnemy.characterTotalPhysicalAttack * 5 - Player.characterTotalPhysicalDefense * 3;
			if (tempDamage < 0) {
				tempDamage = 0;
			}
			Player.characterCurrentHealth -= tempDamage;

			battleTextEnemy.text = CurrentEnemy.characterNameString + " Used Critical Strike & Dealt " + tempDamage + " Damage To " + Player.characterNameString + "!";

			CurrentEnemy.CriticalStrikeCooldown += 2;
			CurrentEnemy.characterCurrentMana -= CurrentEnemy.CriticalStrikeMana;

			Notify (CharacterTarget.PLAYER, NotificationType.HEALTH, Player.gameObject);
			Notify (CharacterTarget.ENEMY, NotificationType.MANA, CurrentEnemy.gameObject);
		}
		//Notify (CharacterTarget.PLAYER, ActionType.CRITICAL_HIT, Player.gameObject, ItemType.NONE);
	}

	public void StunBashFunction ()
	{
		if (playerTurn == 0) {
			if (Player.StunBashCooldown == 0) {
				if (Player.characterCurrentMana >= Player.StunBashMana) {
					int tempDamage = Player.characterTotalPhysicalAttack * 5 - CurrentEnemy.characterTotalPhysicalDefense * 4;
					if (tempDamage < 0) {
						tempDamage = 0;
					}
					CurrentEnemy.characterCurrentHealth -= tempDamage;

					battleTextPlayer.text = Player.characterNameString + " Used Stun Bash & Dealt " + tempDamage + " Damage To " + CurrentEnemy.characterNameString + "!";

					Player.StunBashCooldown += 3;
					Player.characterCurrentMana -= Player.StunBashMana;
					CurrentEnemy.characterStunCounter += 1;

					CountdownTurnStatus (Player, CurrentEnemy);

					gameManager.fightEmpty1.SetActive (false);
					DisableButtons ();

					CurrentEnemy.isStun = true;

					Notify (CharacterTarget.ENEMY, NotificationType.HEALTH, CurrentEnemy.gameObject);
					Notify (CharacterTarget.ENEMY, NotificationType.STUN, CurrentEnemy.gameObject);
					Notify (CharacterTarget.PLAYER, NotificationType.MANA, Player.gameObject);

					playerTurn++;

				} else {
					battleTextPlayer.text = Player.StunBashMana + " Mana Required To Use Stun Bash!";
				}
			} else {
				battleTextPlayer.text = "Stun Bash Has " + Player.StunBashCooldown + " Turns Left Before Usable!";
			}
		} else if (playerTurn == 1) {
			int tempDamage = CurrentEnemy.characterTotalPhysicalAttack * 5 - Player.characterTotalPhysicalDefense * 4;
			if (tempDamage < 0) {
				tempDamage = 0;
			}
			Player.characterCurrentHealth -= tempDamage;

			battleTextEnemy.text = CurrentEnemy.characterNameString + " Used Stun Bash & Dealt " + tempDamage + " Damage To " + Player.characterNameString + "!";

			CurrentEnemy.StunBashCooldown += 3;
			CurrentEnemy.characterCurrentMana -= CurrentEnemy.StunBashMana;
			Player.characterStunCounter += 1;

			Player.isStun = true;

			Notify (CharacterTarget.PLAYER, NotificationType.HEALTH, Player.gameObject);
			Notify (CharacterTarget.PLAYER, NotificationType.STUN, Player.gameObject);
			Notify (CharacterTarget.ENEMY, NotificationType.MANA, CurrentEnemy.gameObject);
		}
		//Notify (CharacterTarget.PLAYER, ActionType.STUN_BASH, Player.gameObject, ItemType.NONE);
	}

	public void FireballFunction ()
	{
		if (playerTurn == 0) {
			if (Player.FireballCooldown == 0) {
				if (Player.characterCurrentMana >= Player.FireballMana) {
					int tempDamage = Player.characterTotalMagicalAttack * 4 - CurrentEnemy.characterTotalMagicalDefense * 2;
					if (tempDamage < 0) {
						tempDamage = 0;
					}
					CurrentEnemy.characterCurrentHealth -= tempDamage;

					battleTextPlayer.text = Player.characterNameString + " Used Fireball & Dealt " + tempDamage + " Damage To " + CurrentEnemy.characterNameString + "!";

					Player.FireballCooldown += 3;
					Player.characterCurrentMana -= Player.FireballMana;
					CurrentEnemy.characterBurnCounter += 3;

					CountdownTurnStatus (Player, CurrentEnemy);

					gameManager.fightEmpty1.SetActive (false);
					DisableButtons ();

					CurrentEnemy.isBurn = true;

					Notify (CharacterTarget.ENEMY, NotificationType.HEALTH, CurrentEnemy.gameObject);
					Notify (CharacterTarget.ENEMY, NotificationType.BURN, CurrentEnemy.gameObject);
					Notify (CharacterTarget.PLAYER, NotificationType.MANA, Player.gameObject);

					playerTurn++;

				} else {
					battleTextPlayer.text = Player.FireballMana + " Mana Required To Use Fireball!";
				}
			} else {
				battleTextPlayer.text = "Fireball Has " + Player.FireballCooldown + " Turns Left Before Usable!";
			}
		} else if (playerTurn == 1) {
			int tempDamage = CurrentEnemy.characterTotalMagicalAttack * 4 - Player.characterTotalMagicalDefense * 2;
			if (tempDamage < 0) {
				tempDamage = 0;
			}
			Player.characterCurrentHealth -= tempDamage;

			battleTextEnemy.text = CurrentEnemy.characterNameString + " Used Fireball & Dealt " + tempDamage + " Damage To " + Player.characterNameString + "!";

			CurrentEnemy.FireballCooldown += 3;
			CurrentEnemy.characterCurrentMana -= CurrentEnemy.FireballMana;
			Player.characterBurnCounter += 3;

			Player.isBurn = true;

			Notify (CharacterTarget.PLAYER, NotificationType.HEALTH, Player.gameObject);
			Notify (CharacterTarget.PLAYER, NotificationType.BURN, Player.gameObject);
			Notify (CharacterTarget.ENEMY, NotificationType.MANA, CurrentEnemy.gameObject);
		}
		//Notify (CharacterTarget.PLAYER, ActionType.FIREBALL, Player.gameObject, ItemType.NONE);
	}

	public void DecayFunction ()
	{
		if (playerTurn == 0) {
			if (Player.DecayCooldown == 0) {
				if (Player.characterCurrentMana >= Player.DecayMana) {
					int tempDamage = Player.characterTotalMagicalAttack * 6 - CurrentEnemy.characterTotalMagicalDefense * 5;
					if (tempDamage < 0) {
						tempDamage = 0;
					}
					CurrentEnemy.characterCurrentHealth -= tempDamage;

					battleTextPlayer.text = Player.characterNameString + " Used Decay & Dealt " + tempDamage + " Damage To " + CurrentEnemy.characterNameString + "!";

					Player.DecayCooldown += 3;
					Player.characterCurrentMana -= Player.DecayMana;
					CurrentEnemy.characterCurrentPhysicalAttack -= CurrentEnemy.characterBasePhysicalAttack * 1 / 4;
					CurrentEnemy.characterCurrentPhysicalDefense -= CurrentEnemy.characterBasePhysicalDefense * 1 / 4;
					CurrentEnemy.characterWeakCounter += 2;
					CurrentEnemy.characterBreakCounter += 2;

					CountdownTurnStatus (Player, CurrentEnemy);

					gameManager.fightEmpty1.SetActive (false);
					DisableButtons ();

					CurrentEnemy.isWeak = true;
					CurrentEnemy.isBreak = true;

					Notify (CharacterTarget.ENEMY, NotificationType.HEALTH, CurrentEnemy.gameObject);
					Notify (CharacterTarget.ENEMY, NotificationType.WEAK, CurrentEnemy.gameObject);
					Notify (CharacterTarget.ENEMY, NotificationType.BREAK, CurrentEnemy.gameObject);
					Notify (CharacterTarget.PLAYER, NotificationType.MANA, Player.gameObject);

					playerTurn++;

				} else {
					battleTextPlayer.text = Player.DecayMana + " Mana Required To Use Decay!";
				}
			} else {
				battleTextPlayer.text = "Decay Has " + Player.DecayCooldown + " Turns Left Before Usable!";
			}
		} else if (playerTurn == 1) {
			int tempDamage = (CurrentEnemy.characterTotalMagicalAttack * 6 - Player.characterTotalMagicalDefense * 5);
			if (tempDamage < 0) {
				tempDamage = 0;
			}
			Player.characterCurrentHealth -= tempDamage;

			battleTextEnemy.text = CurrentEnemy.characterNameString + " Used Decay & Dealt " + tempDamage + " Damage To " + Player.characterNameString + "!";

			CurrentEnemy.DecayCooldown += 3;
			CurrentEnemy.characterCurrentMana -= CurrentEnemy.DecayMana;
			Player.characterCurrentPhysicalAttack -= Player.characterBasePhysicalAttack * 1 / 4;
			Player.characterCurrentPhysicalDefense -= Player.characterBasePhysicalDefense * 1 / 4;
			Player.characterWeakCounter += 2;
			Player.characterBreakCounter += 2;

			Player.isWeak = true;
			Player.isBreak = true;

			Notify (CharacterTarget.PLAYER, NotificationType.HEALTH, Player.gameObject);
			Notify (CharacterTarget.PLAYER, NotificationType.WEAK, Player.gameObject);
			Notify (CharacterTarget.PLAYER, NotificationType.BREAK, Player.gameObject);
			Notify (CharacterTarget.ENEMY, NotificationType.MANA, CurrentEnemy.gameObject);
		}
		//Notify (CharacterTarget.PLAYER, ActionType.DECAY, Player.gameObject, ItemType.NONE);
	}

	public void Notify (CharacterTarget characterTarget, NotificationType notificationTarget, GameObject objectTarget)
	{
		for (int i = 0; i < ObserverList.Count; i++) {
			ObserverList [i].Notify (characterTarget, notificationTarget, objectTarget);
		}
	}

	public void SubscribeObserver (ObserverScript observerScript)
	{
		ObserverList.Add (observerScript);
	}

}

/*
public enum AchievementType
{
	ACHIEVEMENT_1 = 0,
	ACHIEVEMENT_2,
	ACHIEVEMENT_3,
	ACHIEVEMENT_4,
	ACHIEVEMENT_5,
	ACHIEVEMENT_6,
}

public class AchievementSubjectScript : MonoBehaviour
{
	public static AchievementSubjectScript Instance;

	List <AchievementObserverScript> observerList = new List<AchievementObserverScript> ();

	public void Awake ()
	{
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (this.gameObject);
	}

	void Start ()
	{

	}

	void Update ()
	{
		ButtonInteract ();
	}

	public void ButtonInteract ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Notify (AchievementType.ACHIEVEMENT_1);
		}
		if (Input.GetKeyDown (KeyCode.A)) {
			Notify (AchievementType.ACHIEVEMENT_2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha9)) {
			Notify (AchievementType.ACHIEVEMENT_3);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			Notify (AchievementType.ACHIEVEMENT_4);
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			Notify (AchievementType.ACHIEVEMENT_5);
		}
	}

	public void Notify (AchievementType type)
	{
		for (int i = 0; i < observerList.Count; i++) {
			observerList [i].Notify (type);
		}
	}

	public void SubscribeObserver (AchievementObserverScript observerScript)
	{
		observerList.Add (observerScript);
	}

	public void UnSubscribeObserver (AchievementObserverScript observerScript)
	{
		observerList.Remove (observerScript);
		Notify (AchievementType.ACHIEVEMENT_6);
	}


}
*/
