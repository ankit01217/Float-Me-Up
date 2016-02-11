using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroController : MonoBehaviour {

	public float interval;
	public Texture[] textures;
	public AudioClip music;
	public AudioClip[] voiceovers;
	AudioSource audioSource;
	bool isVideoComplete = false;
	MovieTexture movTerxture;
	int curIndex = 0;
	// Use this for initialization
	void Start () {

				
		//play music source
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = music;
		audioSource.Play();

		//audioSource.PlayOneShot (voiceovers [curIndex]);
		movTerxture = (GetComponent<MeshRenderer>().materials[0].mainTexture as MovieTexture);
		movTerxture.Play ();

	}

	void changeIntroImage(){
		curIndex++;
		if (curIndex == textures.Length) {
			loadMainScene();
		} else {
			//change image
			Invoke("replaceImage",0.6f);

		}
	}

	void loadMainScene(){
		CancelInvoke("changeIntroImage");
		Application.LoadLevel ("Game");
	}

	void replaceImage(){
		audioSource.PlayOneShot (voiceovers [curIndex]);
		//introImage.texture = textures[curIndex];
		GetComponent<MeshRenderer> ().materials [0].mainTexture = textures [curIndex];

	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.RightArrow) == true) {
			loadMainScene();
		}


		if (movTerxture.isPlaying) {
			Debug.Log ("isPlaying");
		} else {
			Debug.Log ("Video Complete");
			if(isVideoComplete == false)
			{
				isVideoComplete = true;
				loadMainScene();
			}
			
		}



	}
}
