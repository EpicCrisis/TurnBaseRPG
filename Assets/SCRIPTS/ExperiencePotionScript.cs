using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperiencePotionScript : MonoBehaviour
{
	CharacterAttributesScript Player;

	InventorySystem inventorySystem;

	public int gainAmount = 15;

	void Start ()
	{
		Player = FindObjectOfType<PlayerScript> ();
		inventorySystem = FindObjectOfType<InventorySystem> ();
	}

	void Update ()
	{
		
	}

	public void DrinkExperiencePotion ()
	{
		Player.ExperiencePotion -= 1;
		Player.characterExperience += gainAmount;
		inventorySystem.Despawn (gameObject);
		inventorySystem.NotifyInventory (InventoryNotification.INV_EXPERIENCE, ItemNotification.EXP_POTION, Player.gameObject);
	}
}
