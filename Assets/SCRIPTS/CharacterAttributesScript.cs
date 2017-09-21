using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAttributesScript : MonoBehaviour
{
	public CharacterTarget myCharacterType;
	public Text characterNameText;
	public Text characterNameInventoryText;
	public Text characterBattleText;

	public string characterNameString;

	public int characterMaxHealth = 0;
	public int characterCurrentHealth = 0;

	public int characterMaxMana = 0;
	public int characterCurrentMana = 0;

	public int characterTotalPhysicalAttack = 0;
	public int characterBasePhysicalAttack = 0;
	public int characterCurrentPhysicalAttack = 0;

	public int characterTotalPhysicalDefense = 0;
	public int characterBasePhysicalDefense = 0;
	public int characterCurrentPhysicalDefense = 0;

	public int characterTotalMagicalAttack = 0;
	public int characterBaseMagicalAttack = 0;
	public int characterCurrentMagicalAttack = 0;

	public int characterTotalMagicalDefense = 0;
	public int characterBaseMagicalDefense = 0;
	public int characterCurrentMagicalDefense = 0;

	public int characterAgility = 0;

	public int characterExperience = 0;
	public int characterNextLevelExperience = 0;

	public int characterLevel = 0;

	public int characterPoints = 0;

	public int characterDefendCounter = 0;
	public int characterStunCounter = 0;
	public int characterBurnCounter = 0;
	public int characterWeakCounter = 0;
	public int characterBreakCounter = 0;

	public int CriticalStrikeCooldown = 0;
	public int StunBashCooldown = 0;
	public int FireballCooldown = 0;
	public int DecayCooldown = 0;

	public int CriticalStrikeMana = 0;
	public int StunBashMana = 0;
	public int FireballMana = 0;
	public int DecayMana = 0;

	public bool isDefend = false;
	public bool isStun = false;
	public bool isBurn = false;
	public bool isWeak = false;
	public bool isBreak = false;
	public bool hasSkillPoints = false;

	public int HealthPotion = 0;
	public int ManaPotion = 0;
	public int ExperiencePotion = 0;
	public int ArmourCount = 0;
	public int WeaponCount = 0;

	public bool isArmour = false;
	public bool isWeapon = false;
}