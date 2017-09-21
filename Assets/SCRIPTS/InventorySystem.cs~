using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemNotification
{
	HEALTH_POTION = 0,
	MANA_POTION,
	EXP_POTION,
	WEAPON,
	ARMOUR,
	NONE,
};

public enum InventoryNotification
{
	INV_HEALTH = 0,
	INV_MANA,
	INV_EXPERIENCE,
	UPGRADE_BUTTON,
	EQUIPMENT,
	NONE,
}

public class InventorySystem : MonoBehaviour
{
	public List <ObserverScript> ObserverList = new List<ObserverScript> ();

	public static InventorySystem instance;

	CharacterAttributesScript Player;

	public Text PlayerLevelText;
	public Text PlayerCharacterPoints;
	public Text PlayerMaxHealthText;
	public Text PlayerMaxManaText;
	public Text PlayerBasePhysicalAttackText;
	public Text PlayerBasePhysicalDefenseText;
	public Text PlayerBaseMagicalAttackText;
	public Text PlayerBaseMagicalDefenseText;

	public int inventoryHealthPotion;
	public int inventoryManaPotion;
	public int inventoryExperiencePotion;
	public int inventoryWeapon;
	public int inventoryArmour;
	public int totalInventory;

	//create player stats in the menu that can be leveled up

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}
	}

	public GameObject InventoryEmpty;

	public List<GameObject> itemsToPool;
	public List<int> numberOfItemsToPool;
	Dictionary <string, Stack<GameObject>> poolDictionary;

	GameObject HealthPotion;
	GameObject ManaPotion;
	GameObject ExperiencePotion;
	GameObject Armour;
	GameObject Weapon;

	void Start ()
	{
		InitializeInventoryPoolManager ();

		Player = FindObjectOfType<PlayerScript> ();
	}

	void Update ()
	{
		CheckLevelExperience ();
		CheckInventory ();

		HealthPotion = GameObject.Find ("Health Potion");

		ManaPotion = GameObject.Find ("Mana Potion");

		ExperiencePotion = GameObject.Find ("Experience Potion");

		Armour = GameObject.Find ("Armour");

		Weapon = GameObject.Find ("Weapon");
	}

	public void CheckInventory ()
	{
		totalInventory = inventoryHealthPotion + inventoryManaPotion + inventoryExperiencePotion;

		/*
		if (totalInventory > 49) {
			Player.HealthPotion--;
			Despawn (HealthPotion);
			Player.ManaPotion--;
			Despawn (ManaPotion);
			Player.ExperiencePotion--;
			Despawn (ExperiencePotion);
		}
		*/

		if (inventoryHealthPotion < Player.HealthPotion) {
			inventoryHealthPotion++;
			Spawn ("Health Potion");
		}
		if (inventoryHealthPotion > Player.HealthPotion) {
			inventoryHealthPotion--;
		}
		if (inventoryManaPotion < Player.ManaPotion) {
			inventoryManaPotion++;
			Spawn ("Mana Potion");
		}
		if (inventoryManaPotion > Player.ManaPotion) {
			inventoryManaPotion--;
		}
		if (inventoryExperiencePotion < Player.ExperiencePotion) {
			inventoryExperiencePotion++;
			Spawn ("Experience Potion");
		}
		if (inventoryExperiencePotion > Player.ExperiencePotion) {
			inventoryExperiencePotion--;
		}
		if (inventoryArmour < Player.ArmourCount) {
			inventoryArmour++;
			Spawn ("Armour");
		}
		if (inventoryArmour > Player.ArmourCount) {
			inventoryArmour--;
		}
		if (inventoryWeapon < Player.WeaponCount) {
			inventoryWeapon++;
			Spawn ("Weapon");
		}
		if (inventoryWeapon > Player.WeaponCount) {
			inventoryWeapon--;
		}
	}

	public void CheckLevelExperience ()
	{
		if (Input.GetKeyDown (KeyCode.A)) {
			Player.HealthPotion++;
			Player.ManaPotion++;
			Player.ExperiencePotion++;
		}

		PlayerLevelText.text = "Character Level: " + Player.characterLevel.ToString ();
		PlayerCharacterPoints.text = "Character Points: " + Player.characterPoints.ToString ();
		PlayerMaxHealthText.text = "Health: " + Player.characterCurrentHealth.ToString () + "/" + Player.characterMaxHealth;
		PlayerMaxManaText.text = "Mana: " + Player.characterCurrentMana.ToString () + "/" + Player.characterMaxMana;
		PlayerBasePhysicalAttackText.text = "Physical Attack: " + Player.characterBasePhysicalAttack.ToString () + "+" + Player.characterCurrentPhysicalAttack.ToString ();
		PlayerBasePhysicalDefenseText.text = "Physical Defense: " + Player.characterBasePhysicalDefense.ToString () + "+" + Player.characterCurrentPhysicalDefense.ToString ();
		PlayerBaseMagicalAttackText.text = "Magical Attack: " + Player.characterBaseMagicalAttack.ToString () + "+" + Player.characterCurrentMagicalAttack.ToString ();
		PlayerBaseMagicalDefenseText.text = "Magical Defense: " + Player.characterBaseMagicalDefense.ToString () + "+" + Player.characterCurrentMagicalDefense.ToString ();

		if (Player.characterExperience >= Player.characterNextLevelExperience) {
			Player.characterExperience -= Player.characterNextLevelExperience;
			Player.characterLevel += 1;
			Player.characterPoints += 1;
			Player.characterNextLevelExperience += Player.characterNextLevelExperience * 1 / 5;
		}

		if (Player.characterPoints > 0) {
			Player.hasSkillPoints = true;
			NotifyInventory (InventoryNotification.UPGRADE_BUTTON, ItemNotification.NONE, Player.gameObject);
		}
		if (Player.characterPoints == 0) {
			Player.hasSkillPoints = false;
			NotifyInventory (InventoryNotification.UPGRADE_BUTTON, ItemNotification.NONE, Player.gameObject);
		}

		NotifyInventory (InventoryNotification.INV_HEALTH, ItemNotification.NONE, Player.gameObject);
		NotifyInventory (InventoryNotification.INV_MANA, ItemNotification.NONE, Player.gameObject);
		NotifyInventory (InventoryNotification.INV_EXPERIENCE, ItemNotification.NONE, Player.gameObject);
	}

	public void UpgradeMaxHealth ()
	{
		Player.characterPoints -= 1;
		Player.characterMaxHealth += 10;
		Player.characterCurrentHealth += 10;
	}

	public void UpgradeMaxMana ()
	{
		Player.characterPoints -= 1;
		Player.characterMaxMana += 10;
		Player.characterCurrentMana += 10;
	}

	public void UpgradeBasePhysicalAttack ()
	{
		Player.characterPoints -= 1;
		Player.characterBasePhysicalAttack += 1;
	}

	public void UpgradeBasePhysicalDefense ()
	{
		Player.characterPoints -= 1;
		Player.characterBasePhysicalDefense += 1;
	}

	public void UpgradeBaseMagicalAttack ()
	{
		Player.characterPoints -= 1;
		Player.characterBaseMagicalAttack += 1;
	}

	public void UpgradeBaseMagicalDefense ()
	{
		Player.characterPoints -= 1;
		Player.characterBaseMagicalDefense += 1;
	}

	public void NotifyInventory (InventoryNotification inventoryTarget, ItemNotification itemTarget, GameObject objectTarget)
	{
		for (int i = 0; i < ObserverList.Count; i++) {
			ObserverList [i].NotifyInventory (inventoryTarget, itemTarget, objectTarget);
		}
	}

	public void SubscribeObserver (ObserverScript observerScript)
	{
		ObserverList.Add (observerScript);
	}

	void InitializeInventoryPoolManager ()
	{
		poolDictionary = new Dictionary<string, Stack<GameObject>> ();

		for (int i = 0; i < itemsToPool.Count; i++) {

			poolDictionary.Add (itemsToPool [i].name, new Stack<GameObject> ());

			for (int f = 0; f < numberOfItemsToPool [i]; f++) {
				GameObject item = Instantiate (itemsToPool [i]);
				item.transform.SetParent (InventoryEmpty.transform);
				item.transform.localScale = new Vector2 (1f, 1f);
				item.name = itemsToPool [i].name;
				item.gameObject.SetActive (false);

				poolDictionary [itemsToPool [i].name].Push (item);
			}
		}
	}

	public void Spawn (string objectName)
	{
		//check for anomaly gameobject
		if (!poolDictionary.ContainsKey (objectName)) {
			Debug.LogWarning ("No Pool For " + objectName + " Exists!");
			return;
		}

		if (poolDictionary [objectName].Count > 0) {
			GameObject go = poolDictionary [objectName].Pop ();
			go.SetActive (true);
		} else {
			Debug.LogWarning ("Pool Limit Reached : " + objectName);
		}
	}

	public void Despawn (GameObject objectToDespawn)
	{
		objectToDespawn.SetActive (false);
		poolDictionary [objectToDespawn.name].Push (objectToDespawn);
	}
}
