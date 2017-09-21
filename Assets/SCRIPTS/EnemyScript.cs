using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PatrolDirection
{
	HORIZONTAL = 0,
	VERTICAL,
	NONE,
};

public class EnemyScript : CharacterAttributesScript
{
	CombatSystem combatSystem;

	public PatrolDirection curDirection;
	bool isForward = true;

	public bool canMove = false;
	public float moveTimer = 0f;
	public float moveDelay = 1000f;

	public int xPos = 0;
	public int yPos = 0;

	void Start ()
	{
		combatSystem = FindObjectOfType<CombatSystem> ();

		//int tempX = Random.Range (1, TileManagerScript.Instance.COL_COUNT - 1);
		//int tempY = Random.Range (1, TileManagerScript.Instance.ROW_COUNT - 1);

		//xPos = tempX;
		//yPos = tempY;

		int randForward = Random.Range (0, 2);
		if (randForward == 0) {
			isForward = true;
		} else {
			isForward = false;
		}
		curDirection = (PatrolDirection)Random.Range (0, (int)PatrolDirection.NONE);
	}

	void Update ()
	{

		Calculations ();

		if (GameManagerScript.Instance.curState != GameStates.OVERWORLD) {
			return;
		}

		transform.position = TileManagerScript.Instance.posMap [xPos, yPos];

		//needs to be fixed
		if (!canMove) {
			if (moveTimer <= moveDelay) {
				moveTimer += Time.deltaTime * 1000f;
			} else {
				moveTimer = 0f;
				canMove = true;
			}
		} else if (canMove) {
			if (curDirection == PatrolDirection.HORIZONTAL) {
				if (isForward) {
					xPos++;
				} else {
					xPos--;
				}
			} else if (curDirection == PatrolDirection.VERTICAL) {
				if (isForward) {
					yPos++;
				} else {
					yPos--;
				}
			}
			Move ();
		}
	}

	void Calculations ()
	{
		characterTotalPhysicalAttack = characterBasePhysicalAttack + characterCurrentPhysicalAttack;
		characterTotalPhysicalDefense = characterBasePhysicalDefense + characterCurrentPhysicalDefense;
		characterTotalMagicalAttack = characterBaseMagicalAttack + characterCurrentMagicalAttack;
		characterTotalMagicalDefense = characterBaseMagicalDefense + characterCurrentMagicalDefense;
	}

	void Move ()
	{
		//Debug.Log ("CheckMove");
		if (yPos > TileManagerScript.Instance.ROW_COUNT - 2) {
			yPos = TileManagerScript.Instance.ROW_COUNT - 2;
			isForward = false;
		} else if (yPos < 1) {
			yPos = 1;
			isForward = true;
		}
		if (xPos > TileManagerScript.Instance.COL_COUNT - 2) {
			xPos = TileManagerScript.Instance.COL_COUNT - 2;
			isForward = false;
		} else if (xPos < 1) {
			xPos = 1;
			isForward = true;
		}
		canMove = false;
	}

	void OnTriggerStay2D (Collider2D target)
	{
		if (target.CompareTag ("Player")) {
			if (this.CompareTag ("JackScott")) {
				combatSystem.CurrentEnemy = combatSystem.JackScott;
				combatSystem.CurrentEnemyObject = combatSystem.JackScottObject;
			}
			if (this.CompareTag ("BigMacD")) {
				combatSystem.CurrentEnemy = combatSystem.BigMacD;
				combatSystem.CurrentEnemyObject = combatSystem.BigMacDObject;
			}
			if (this.CompareTag ("LilMacD")) {
				combatSystem.CurrentEnemy = combatSystem.LilMacD;
				combatSystem.CurrentEnemyObject = combatSystem.LilMacDObject;
			}
			if (this.CompareTag ("Rando")) {
				combatSystem.CurrentEnemy = combatSystem.Rando;
				combatSystem.CurrentEnemyObject = combatSystem.RandoObject;
			}
			if (this.CompareTag ("BossaNova")) {
				combatSystem.CurrentEnemy = combatSystem.BossaNova;
				combatSystem.CurrentEnemyObject = combatSystem.BossaNovaObject;
			}
			GameManagerScript.Instance.curState = GameStates.BATTLE;
		}
	}

	/*
	void OnTriggerExit2D (Collider2D target)
	{
		JackScott.SetActive (false);
		BigMacD.SetActive (false);
		LilMacD.SetActive (false);
		Rando.SetActive (false);
		BossaNova.SetActive (false);
	}
	*/
}
