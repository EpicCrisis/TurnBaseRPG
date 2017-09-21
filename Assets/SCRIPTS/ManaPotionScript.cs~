using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPotionScript : MonoBehaviour
{
	CharacterAttributesScript Player;

	InventorySystem inventorySystem;

	public int restoreAmount = 15;

	void Start ()
	{
		Player = FindObjectOfType<PlayerScript> ();
		inventorySystem = FindObjectOfType<InventorySystem> ();
	}

	void Update ()
	{
		
	}

	public void DrinkManaPotion ()
	{
		if (Player.characterCurrentMana != Player.characterMaxMana) {
			Player.ManaPotion -= 1;
			Player.characterCurrentMana += restoreAmount;
			inventorySystem.Despawn (gameObject);
			inventorySystem.NotifyInventory (InventoryNotification.INV_MANA, ItemNotification.MANA_POTION, Player.gameObject);
		}
	}
}
