using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerScript : MonoBehaviour
{

	public static SpawnManagerScript Instance;

	public int enemyTotal = 5;

	public GameObject enemyPrefab;

	public List<EnemyScript> enemyList = new List<EnemyScript> ();

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		//SpawnEnemies ();
	}

	void Update ()
	{
		
	}

	public bool isEnemyPresent (int checkX, int checkY)
	{
		for (int i = 0; i < enemyList.Count; i++) {

			/*
			Debug.Log ("XPOS LIST : " + enemyList [i].xPos + "\t" + "YPOS LIST:" + enemyList [i].yPos
			+ "\n" + "XPOS CHECK: " + checkX + "\t" + "YCHECK:" + checkY);
			*/

			if (enemyList [i].xPos == checkX && enemyList [i].yPos == checkY) {
				Debug.Log ("Enemy Present");
				return true;
			}
		}
		return false;
	}

	public void SpawnEnemies ()
	{
		for (int i = 0; i < enemyTotal; i++) {
			int tempX = Random.Range (1, TileManagerScript.Instance.COL_COUNT - 1);
			int tempY = Random.Range (1, TileManagerScript.Instance.ROW_COUNT - 1);
			GameObject enemyObj = (GameObject)Instantiate (enemyPrefab, TileManagerScript.Instance.posMap [tempX, tempY], Quaternion.identity);
			//Debug.Log (enemyObj.transform.position);
			EnemyScript enemyScript = enemyObj.GetComponent<EnemyScript> ();

			//set enemy pos
			enemyScript.xPos = tempX;
			enemyScript.yPos = tempY;
			//make sure pos does not have enemy or player

			//save enemy in list
			enemyList.Add (enemyScript);
		}
	}
}
