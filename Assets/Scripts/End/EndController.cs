using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndController : MonoBehaviour {
	
	public float interval;
	public Texture[] textures;
	public AudioClip music1,music2;
	public AudioClip[] voiceovers;
	Animator faderAnimator;
	AudioSource audioSource;
	MovieTexture movTerxture;
	bool isVideoComplete = false;

	int curIndex = 0;
	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3(Camera.main.orthographicSize/2 * (Screen.width/Screen.height),Camera.main.orthographicSize/2,1f);


		audioSource = GetComponent<AudioSource> ();



		if (HammerController.gameOverState == 1) {
			//success
			GetComponent<MeshRenderer> ().materials [0].mainTexture = textures [0];
			movTerxture = (MovieTexture)GetComponent<MeshRenderer> ().materials [0].mainTexture;
			movTerxture.Play();
			audioSource.clip = music1;
			audioSource.Play();
			audioSource.PlayOneShot(voiceovers[0]);

		}
		else if(HammerController.gameOverState == 2 )
		{
			GetComponent<MeshRenderer> ().materials [0].mainTexture = textures [1];
			movTerxture = (MovieTexture)GetComponent<MeshRenderer> ().materials [0].mainTexture;
			movTerxture.Play();
			audioSource.clip = music2;
			audioSource.Play();
			audioSource.PlayOneShot(voiceovers[1]);
		
		}



	}


	
	// Update is called once per frame
	void Update () {
		if (movTerxture.isPlaying) {
			Debug.Log ("isPlaying");
		} else {
			Debug.Log ("Video Complete");
			if(isVideoComplete == false)
			{
				isVideoComplete = true;
				loadCredits();
			}
			
		}
	}

	void loadCredits(){
		GetComponent<MeshRenderer> ().materials [0].mainTexture = textures[2];
		movTerxture = (MovieTexture)GetComponent<MeshRenderer> ().materials [0].mainTexture;
		movTerxture.Play();


	}
}
