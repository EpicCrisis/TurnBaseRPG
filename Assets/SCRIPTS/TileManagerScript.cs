using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManagerScript : MonoBehaviour
{
	public static TileManagerScript Instance;

	public List<Sprite> tileSpriteList = new List<Sprite> ();

	public GameObject tilePrefab;

	public int ROW_COUNT = 12;
	public int COL_COUNT = 20;

	float tileSize = 0.64f;

	public Vector2[,] posMap = new Vector2[20, 12];

	public GameObject playerObj;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		GenerateTileMap ();

		//playerObj = Instantiate (playerObj, Vector2.zero, Quaternion.identity);

		playerObj = GameObject.FindGameObjectWithTag ("Player");
		PlayerScript playerScript = playerObj.GetComponent<PlayerScript> ();

		/*
		int tempX = 0;
		int tempY = 0;

		while (true) {
			tempX = Random.Range (1, COL_COUNT - 1);
			tempY = Random.Range (1, ROW_COUNT - 1);
			if (SpawnManagerScript.Instance.isEnemyPresent (tempX, tempY) == false) {
				break;
			}
		}

		playerScript.xPos = tempX;
		playerScript.yPos = tempY;
		*/

		playerObj.transform.position = posMap [playerScript.xPos, playerScript.yPos];

		//SpawnManagerScript.Instance.SpawnEnemies ();
	}

	void GenerateTileMap ()
	{
		for (int j = 0; j < COL_COUNT; j++) {
			for (int i = 0; i < ROW_COUNT; i++) {
				new Vector3 (j * tileSize - COL_COUNT / 2f * tileSize + tileSize / 2f,
					i * tileSize - ROW_COUNT / 2f * tileSize + tileSize / 2f);
				GameObject obj = (GameObject)Instantiate (tilePrefab, new Vector3 (j * tileSize - COL_COUNT / 2f * tileSize + tileSize / 2f,
					                 i * tileSize - ROW_COUNT / 2f * tileSize + tileSize / 2f), Quaternion.identity);
				posMap [j, i] = obj.transform.position;

				TileScript tileScript = obj.GetComponent<TileScript> ();
				if (tileScript != null) {
					//logic to select tile type
					if (i == 0 || i == ROW_COUNT - 1 || j == 0 || j == COL_COUNT - 1) {
						tileScript.type = TileType.WALL;
					}
					tileScript.InitializeTile ();
				}
			}
		}
	}

	void Update ()
	{
		
	}
}
