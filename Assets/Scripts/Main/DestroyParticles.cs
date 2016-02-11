using UnityEngine;
using System.Collections;

public class DestroyParticles : MonoBehaviour {

	Transform target;
	// Use this for initialization
	void Start () {

		target = GameObject.FindGameObjectWithTag ("ParticleTarget").transform;
		Invoke ("removeParticles", 1);
	}

	void removeParticles(){
		Destroy (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
