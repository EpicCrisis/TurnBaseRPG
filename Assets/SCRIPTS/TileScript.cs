using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
	SAND = 0,
	BRICK,
	WOOD,
	WALL,
};

public class TileScript : MonoBehaviour
{
	public TileType type;

	void Start ()
	{
		
	}

	public void InitializeTile ()
	{
		GetComponent<SpriteRenderer> ().sprite = TileManagerScript.Instance.tileSpriteList [(int)type];
		if (type != TileType.WALL) {
			GetComponent<BoxCollider2D> ().enabled = false;
		}
	}

	void Update ()
	{
		
	}
}
