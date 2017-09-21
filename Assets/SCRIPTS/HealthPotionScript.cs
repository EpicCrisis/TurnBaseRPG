using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionScript : MonoBehaviour
{
	CharacterAttributesScript Player;

	InventorySystem inventorySystem;

	public int healAmount = 15;

	void Start ()
	{
		Player = FindObjectOfType<PlayerScript> ();
		inventorySystem = FindObjectOfType<InventorySystem> ();
	}

	void Update ()
	{
		
	}

	public void DrinkHealthPotion ()
	{
		if (Player.characterCurrentHealth != Player.characterMaxHealth) {
			Player.HealthPotion -= 1;
			Player.characterCurrentHealth += healAmount;
			inventorySystem.Despawn (gameObject);
			inventorySystem.NotifyInventory (InventoryNotification.INV_HEALTH, ItemNotification.HEALTH_POTION, Player.gameObject);
		}
	}
}
