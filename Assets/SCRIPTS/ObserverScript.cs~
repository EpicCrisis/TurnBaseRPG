using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObserverScript : MonoBehaviour
{

	public CharacterTarget characterType;
	public NotificationType notificationType;
	public Image ObjectImage;
	public GameObject IconImage;

	public InventoryNotification inventoryType;
	public ItemNotification itemType;

	public int MaxCounter;
	public int Counter;

	CharacterAttributesScript characterAttributes;

	CombatSystem combatSystem;

	InventorySystem inventorySystem;

	void Start ()
	{
		Initialize ();
	}

	void Update ()
	{
		
	}

	//find the object type being notified
	public void Notify (CharacterTarget characterTarget, NotificationType notificationTarget, GameObject objectTarget)
	{
		if (characterType == characterTarget) {
			
			CharacterAttributesScript target = objectTarget.GetComponent<CharacterAttributesScript> ();

			if (notificationType == notificationTarget) {
				if (notificationType == NotificationType.HEALTH) {
					ObjectImage.fillAmount = (float)target.characterCurrentHealth / (float)target.characterMaxHealth;
				} else if (notificationType == NotificationType.MANA) {
					ObjectImage.fillAmount = (float)target.characterCurrentMana / (float)target.characterMaxMana;
				} else if (notificationType == NotificationType.BURN) {
					IconImage.SetActive (target.isBurn);
				} else if (notificationType == NotificationType.BREAK) {
					IconImage.SetActive (target.isBreak);
				} else if (notificationType == NotificationType.WEAK) {
					IconImage.SetActive (target.isWeak);
				} else if (notificationType == NotificationType.STUN) {
					IconImage.SetActive (target.isStun);
				} else if (notificationType == NotificationType.DEFEND) {
					IconImage.SetActive (target.isDefend);
				}
			}

		}
	}

	public void NotifyInventory (InventoryNotification inventoryTarget, ItemNotification itemTarget, GameObject objectTarget)
	{
		if (inventoryType == inventoryTarget) {
			
			CharacterAttributesScript target = objectTarget.GetComponent<CharacterAttributesScript> ();

			if (itemType == itemTarget) {
				if (itemType == ItemNotification.HEALTH_POTION) {
					ObjectImage.fillAmount = (float)target.characterCurrentHealth / (float)target.characterMaxHealth;
				} else if (itemType == ItemNotification.MANA_POTION) {
					ObjectImage.fillAmount = (float)target.characterCurrentMana / (float)target.characterMaxMana;
				} else if (itemType == ItemNotification.EXP_POTION) {
					ObjectImage.fillAmount = (float)target.characterExperience / (float)target.characterNextLevelExperience;
				} else if (itemType == ItemNotification.WEAPON) {
					IconImage.SetActive (target.isWeapon);
				} else if (itemType == ItemNotification.ARMOUR) {
					IconImage.SetActive (target.isArmour);
				}
			}

			if (inventoryType == InventoryNotification.INV_HEALTH) {
				ObjectImage.fillAmount = (float)target.characterCurrentHealth / (float)target.characterMaxHealth;
			} else if (inventoryType == InventoryNotification.INV_MANA) {
				ObjectImage.fillAmount = (float)target.characterCurrentMana / (float)target.characterMaxMana;
			} else if (inventoryType == InventoryNotification.INV_EXPERIENCE) {
				ObjectImage.fillAmount = (float)target.characterExperience / (float)target.characterNextLevelExperience;
			} else if (inventoryType == InventoryNotification.UPGRADE_BUTTON) {
				IconImage.SetActive (target.hasSkillPoints);
			}
		}
	}

	public void Initialize ()
	{
		CombatSystem.instance.SubscribeObserver (this);
		InventorySystem.instance.SubscribeObserver (this);

		if (notificationType == NotificationType.HEALTH || notificationType == NotificationType.MANA) {
			ObjectImage.fillAmount = 1;
		} else if (inventoryType == InventoryNotification.INV_HEALTH || inventoryType == InventoryNotification.INV_MANA) {
			ObjectImage.fillAmount = 1;
		} else if (inventoryType == InventoryNotification.INV_EXPERIENCE) {
			ObjectImage.fillAmount = 0;
		} else {
			IconImage.SetActive (false);
		}
	}
}
