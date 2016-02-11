using UnityEngine;
using System.Collections;

public class Falling : MonoBehaviour {
	public GameObject top;
	public GameObject fallingObject1;
	public GameObject fallingObject2;
	public GameObject fallingObject3;
	public GameObject fallingObject4;
	public GameObject fallingObject5;
	public GameObject fallingStones;
	// Use this for initialization

	private Vector3 topPos;
	private Vector3 topScale;
	private GameObject[] stones;

	void Start () {
		top.gameObject.SetActive(true);
		topPos = new Vector3(top.transform.position.x, top.transform.position.y, top.transform.position.z);
		topScale = new Vector3(top.transform.localScale.x, top.transform.localScale.y, top.transform.localScale.z);
		stones = new GameObject[]{fallingObject1, fallingObject2, fallingObject3, fallingObject4, fallingObject5};

		StartAnimation ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {

		}
	}

	void StartAnimation(){
		GameObject fallingObjects = new GameObject ("fallingObjects");
		for (int i = 0; i < 300; i++) {
			float posx = Random.Range(topPos.x - topScale.x / 3, topPos.x + topScale.x / 3);
			float posz = Random.Range(topPos.z - topScale.z / 3, topPos.z + topScale.z / 3);
			float posy = topPos.y - 0.1f;
			GameObject newObject = (GameObject)Instantiate(stones[Random.Range(0,5)], new Vector3(posx, posy, posz), Quaternion.identity);
			float randomGravity = Random.Range(-5f, 5f);
			newObject.GetComponent<Rigidbody>().velocity = new Vector3(0,randomGravity, 0);
			newObject.transform.parent = fallingObjects.transform;
		}
		for (int i = 0; i < 300; i++) {
			float posx = Random.Range(topPos.x - topScale.x / 3, topPos.x + topScale.x / 3);
			float posz = Random.Range(topPos.z - topScale.z / 3, topPos.z + topScale.z / 3);
			float posy = topPos.y - 0.1f;
			GameObject newObject = (GameObject)Instantiate(fallingStones, new Vector3(posx, posy, posz), Quaternion.identity);
			float randomGravity = Random.Range(-5f, 5f);
			newObject.GetComponent<Rigidbody>().velocity = new Vector3(0,randomGravity, 0);
			newObject.transform.parent = fallingObjects.transform;
		}
		Invoke("StopAnimation",2.0f);
	}

	void StopAnimation(){
		//Time.timeScale = 0.0f;
	}
	
}
