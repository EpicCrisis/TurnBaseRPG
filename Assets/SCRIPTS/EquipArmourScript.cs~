using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipArmourScript : MonoBehaviour
{
	CharacterAttributesScript Player;

	InventorySystem inventorySystem;

	public Image thisObjectImage;

	public int bonusDefense = 2;

	public bool isEquipThis = false;

	void Start ()
	{
		Player = FindObjectOfType<PlayerScript> ();
		inventorySystem = FindObjectOfType<InventorySystem> ();
	}

	void Update ()
	{
		
	}

	public void EquipArmour ()
	{
		if (!Player.isArmour && !isEquipThis) {
			Player.isArmour = true;
			this.isEquipThis = true;
			Player.characterCurrentPhysicalDefense += bonusDefense;
			thisObjectImage.color = Color.grey;
			inventorySystem.NotifyInventory (InventoryNotification.EQUIPMENT, ItemNotification.ARMOUR, Player.gameObject);
		} else if (Player.isArmour && isEquipThis) {
			Player.isArmour = false;
			this.isEquipThis = false;
			Player.characterCurrentPhysicalDefense -= bonusDefense;
			thisObjectImage.color = Color.white;
			inventorySystem.NotifyInventory (InventoryNotification.EQUIPMENT, ItemNotification.ARMOUR, Player.gameObject);
		}
	}
}
