using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipWeaponScript : MonoBehaviour
{
	CharacterAttributesScript Player;

	InventorySystem inventorySystem;

	public Image thisObjectImage;

	public int bonusAttack = 2;

	public bool isEquipThis = false;

	void Start ()
	{
		Player = FindObjectOfType<PlayerScript> ();
		inventorySystem = FindObjectOfType<InventorySystem> ();
	}

	void Update ()
	{
		
	}

	public void EquipWeapon ()
	{
		if (!Player.isWeapon && !isEquipThis) {
			Player.isWeapon = true;
			isEquipThis = true;
			Player.characterCurrentPhysicalAttack += bonusAttack;
			thisObjectImage.color = Color.grey;
			inventorySystem.NotifyInventory (InventoryNotification.EQUIPMENT, ItemNotification.WEAPON, Player.gameObject);
		} else if (Player.isWeapon && isEquipThis) {
			Player.isWeapon = false;
			isEquipThis = false;
			Player.characterCurrentPhysicalAttack -= bonusAttack;
			thisObjectImage.color = Color.white;
			inventorySystem.NotifyInventory (InventoryNotification.EQUIPMENT, ItemNotification.WEAPON, Player.gameObject);
		}
	}
}
