using UnityEngine;
using System.Collections;

public class WaterParticles : MonoBehaviour {

	ParticleSystem ps;
	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
		ps.Stop();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startAnimation(Vector3 newpos){
		ps.transform.position = newpos;
		ps.Play();
	}
}
