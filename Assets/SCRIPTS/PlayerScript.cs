using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : CharacterAttributesScript
{
	public int xPos = 0;
	public int yPos = 0;

	public bool canMove = false;

	public float moveDelay = 500f;
	public float moveDelayTimer = 0f;

	void Start ()
	{
		characterNameText.text = characterNameString;
		characterNameInventoryText.text = characterNameString;
	}

	void Update ()
	{
		Calculations ();

		if (GameManagerScript.Instance.curState != GameStates.OVERWORLD) {
			return;
		}

		Move ();
	}

	void Calculations ()
	{
		if (characterCurrentHealth > characterMaxHealth) {
			characterCurrentHealth = characterMaxHealth;
		}
		if (characterCurrentMana > characterMaxMana) {
			characterCurrentMana = characterMaxMana;
		}

		characterTotalPhysicalAttack = characterBasePhysicalAttack + characterCurrentPhysicalAttack;
		characterTotalPhysicalDefense = characterBasePhysicalDefense + characterCurrentPhysicalDefense;
		characterTotalMagicalAttack = characterBaseMagicalAttack + characterCurrentMagicalAttack;
		characterTotalMagicalDefense = characterBaseMagicalDefense + characterCurrentMagicalDefense;
	}

	void Move ()
	{
		/*
		if (yPos >= TileManagerScript.Instance.ROW_COUNT - 1) {
			yPos = TileManagerScript.Instance.ROW_COUNT - 2;
		}
		if (yPos < 1) {
			yPos = 1;
		}
		if (xPos >= TileManagerScript.Instance.COL_COUNT - 1) {
			xPos = TileManagerScript.Instance.COL_COUNT - 2;
		}
		if (xPos < 1) {
			xPos = 1;
		}
		*/

		if (canMove) {
			if (moveDelayTimer <= moveDelay) {
				moveDelayTimer += Time.deltaTime * 1000f;
			} else {
				moveDelayTimer = 0f;
				canMove = false;
			}
		} else if (!canMove) {
			if (Input.GetButton ("Vertical")) {
				canMove = true;
				if (Input.GetAxis ("Vertical") > 0) {
					if (yPos < TileManagerScript.Instance.ROW_COUNT - 2) {
						yPos++;
					}
				} else if (Input.GetAxis ("Vertical") < 0) {
					if (yPos > 1) {
						yPos--;
					}
				}
			} else if (Input.GetButton ("Horizontal")) {
				canMove = true;
				if (Input.GetAxis ("Horizontal") > 0) {
					if (xPos < TileManagerScript.Instance.COL_COUNT - 2) {
						xPos++;
					}
				} else if (Input.GetAxis ("Horizontal") < 0) {
					if (xPos > 1) {
						xPos--;
					}
				}
			}
		}
			
		transform.position = TileManagerScript.Instance.posMap [xPos, yPos];
	}
}
