using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomObjectPoolScript : MonoBehaviour
{

	//create a singleton by getting private static
	private static CustomObjectPoolScript instance;

	public static CustomObjectPoolScript Instance {
		get { return instance; }
	}

	//awake function to make sure it exists
	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (this.gameObject);
		}

		DontDestroyOnLoad (this.gameObject);
	}

	public List<GameObject> objectsToPool;
	public List<int> numberOfObjectsToPool;
	Dictionary <string, Stack<GameObject>> pool;

	void Start ()
	{
		InitializePoolManager ();
	}

	void Update ()
	{
		
	}

	//use for loop to count number of objects in pool
	//instantiate objects
	void InitializePoolManager ()
	{
		pool = new Dictionary<string, Stack<GameObject>> ();

		for (int i = 0; i < objectsToPool.Count; i++) {
			
			pool.Add (objectsToPool [i].name, new Stack<GameObject> ());

			for (int f = 0; f < numberOfObjectsToPool [i]; f++) {
				GameObject go = Instantiate (objectsToPool [i]);
				go.transform.SetParent (this.transform);
				go.name = objectsToPool [i].name;
				go.gameObject.SetActive (false);

				pool [objectsToPool [i].name].Push (go);
			}
		}
	}

	public void Spawn (string objectName, Vector3 newPosition, Quaternion newRotation)
	{
		//check for anomaly gameobject
		if (!pool.ContainsKey (objectName)) {
			Debug.LogWarning ("No Pool For " + objectName + " Exists!");
			return;
		}

		if (pool [objectName].Count > 0) {
			GameObject go = pool [objectName].Pop ();
			go.transform.position = newPosition;
			go.transform.rotation = newRotation;
			go.SetActive (true);
		} else {
			Debug.LogWarning ("Pool Limit Reached : " + objectName);
		}
	}

	public void Despawn (GameObject objectToDespawn)
	{
		objectToDespawn.SetActive (false);
		pool [objectToDespawn.name].Push (objectToDespawn);
	}
}
