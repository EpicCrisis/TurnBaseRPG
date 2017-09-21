using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{

	public float playerSpeed = 5f;

	void Start ()
	{
		
	}

	void Update ()
	{
		if (Input.GetButton ("Vertical")) {
			if (Input.GetAxis ("Vertical") > 0) {
				transform.Translate (Vector2.up * playerSpeed * Time.deltaTime);
			} else if (Input.GetAxis ("Vertical") < 0) {
				transform.Translate (Vector2.down * playerSpeed * Time.deltaTime);
			}
		} else if (Input.GetButton ("Horizontal")) {
			if (Input.GetAxis ("Horizontal") > 0) {
				transform.Translate (Vector2.right * playerSpeed * Time.deltaTime);
			} else if (Input.GetAxis ("Horizontal") < 0) {
				transform.Translate (Vector2.left * playerSpeed * Time.deltaTime);
			}
		}
	}
}
