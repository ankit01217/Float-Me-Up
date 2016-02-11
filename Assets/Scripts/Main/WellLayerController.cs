using UnityEngine;
using System.Collections;

public class WellLayerController : MonoBehaviour {

	private GameObject[] layers;
	// Use this for initialization
	void Start () {
		layers = GameObject.FindGameObjectsWithTag("WellLayer");
		randomizeLayerPosition ();
	}

	void randomizeLayerPosition(){

		reshuffle (layers);
		/*
		for (int i=0; i<layers.Length; i++) {
			GameObject layer = layers[i];
			var randomRotation = Quaternion.Euler( 0 , Random.Range(0, 360) , 0);
			layer.transform.rotation = randomRotation;

		}*/


	}

	void reshuffle(GameObject[] arr)
	{
		// Knuth shuffle algorithm :: courtesy of Wikipedia :)
		for (int t = 0; t < arr.Length; t++ )
		{
			GameObject tmp = arr[t];
			int r = Random.Range(t, arr.Length);
			arr[t] = arr[r];
			arr[r] = tmp;
		}
	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
