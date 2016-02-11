using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {

	public Texture water1Texture,water2NormalTexture,water1CrackedTexture,water2CrackedTexture, structureTexture, normalTexture,structureCrackedTexture;
	public static int curLayer = 1;
	public static int maxLayers = 15;
	public float swapInterval = 2f;
	float swaptimer = 0;
	AudioSource audioSource;
	public AudioClip levelupSound;
	bool didCrossedLevel1 = false;
	bool didCrossedLevel2 = false;
	bool didCrossedLevel3 = false;

	public AudioClip music1,music2,music3, level1_up,level2_up,level3_up,crackAudio1,crackAudio2;
	GameObject gameMusic;
	AudioSource gameMusicSource;
	public GameObject spotLight;
	public Transform centerPoiint;


	// Use this for initialization
	void Start () {

		gameMusic = GameObject.FindGameObjectWithTag ("GameMusic");
		gameMusicSource = gameMusic.GetComponent<AudioSource> ();
		gameMusicSource.clip = music1;
		gameMusicSource.Play();

		audioSource = GetComponent<AudioSource> ();
		maxLayers = getChildObjectsWithTag (this.gameObject, "WellLayer").Count;

		setObjectsTextureByTag(Brick.STRUCTURE);
		setObjectsTextureByTag(Brick.WATER1);
		setObjectsTextureByTag(Brick.WATER2);


		setFirstLayerBricks ();
		setSecondLayerBricks ();
		setThirdtLayerBricks ();

		updateWellLayer();
	}

	// Update is called once per frame
	void Update () {
		if (LevelController.curLayer >= 5 && didCrossedLevel1 == false) {
			didCrossedLevel1 = true;
			audioSource.PlayOneShot(levelupSound);

			LeanTween.delayedCall (0.8f, () => {
				audioSource.PlayOneShot(level1_up);
			});
			gameMusicSource.clip = music2;
			gameMusicSource.Play ();

		}

		if (LevelController.curLayer >= 10 && didCrossedLevel2 == false) {
			didCrossedLevel2 = true;
			audioSource.PlayOneShot(levelupSound);
			gameMusicSource.clip = music3;
			gameMusicSource.Play ();
			LeanTween.delayedCall (0.8f, () => {
				audioSource.PlayOneShot(level2_up);
			});
		}

		if (LevelController.curLayer >= 15 && didCrossedLevel3 == false) {
			didCrossedLevel3 = true;
			audioSource.PlayOneShot(level3_up);
		}
	}

	public void updateWellLayer(){
		ArrayList layers = getChildObjectsWithTag(gameObject,"WellLayer");
		GameObject curLayerObject = (GameObject)layers [curLayer];

		Light[] curLevelLights = curLayerObject.GetComponentsInChildren<Light> ();
		foreach(Light light in curLevelLights)
		{
			light.enabled = true;
			light.intensity = 0.8f;

		}

		GameObject prevLayerObject = (GameObject)layers [curLayer - 1];
		Light[] prevLevelLights = prevLayerObject.GetComponentsInChildren<Light> ();
		foreach(Light light in prevLevelLights)
		{
			light.enabled = false;

		}



	}

	void setFirstLayerBricks(){
		GameObject firstLevel = GameObject.FindGameObjectWithTag("Level1");
		ArrayList firstLevelStructureBricks = getChildObjectsWithTag(firstLevel,Brick.STRUCTURE);

		foreach (GameObject brick in firstLevelStructureBricks) {
			int prob = Random.Range(0,10);
			if(prob > 7)
			{
				//water brick
				brick.tag = Brick.WATER1;
				setTexture(brick,brick.tag);

			}
			else if(prob > 2)
			{
				//make structure brick

				brick.tag = Brick.STRUCTURE;
				setTexture(brick,brick.tag);

			}
			else{
				brick.tag = Brick.NORMAL;
				setTexture(brick,brick.tag);

			}
		}




	}

	void setSecondLayerBricks(){

		GameObject secondLevel = GameObject.FindGameObjectWithTag("Level2");
		ArrayList structureBricks = getChildObjectsWithTag(secondLevel,Brick.STRUCTURE);

		foreach (GameObject brick in structureBricks) {
			int prob = Random.Range(0,10);
			if(prob > 8)
			{
				//water brick
				brick.tag = Brick.WATER1;
				setTexture(brick,brick.tag);

			}
			else if(prob > 2)
			{
				//make structure brick
				brick.tag = Brick.STRUCTURE;
				setTexture(brick,brick.tag);

			}
			else{
				brick.tag = Brick.NORMAL;
				setTexture(brick,brick.tag);

			}
		}


		RotateLevellayers (2,10f,180f,200f);

	}

	void RotateLevellayers(int level,float maxTime,float minDegree,float maxDegree ){
		GameObject levelContainer = GameObject.FindGameObjectWithTag("Level" + level);

		//rotate each layer with different speed
		ArrayList layers = getChildObjectsWithTag(levelContainer,"WellLayer");
		float rotationTime = maxTime;
		foreach(GameObject layer in layers)
		{
			if(level == 2)
			{
				LeanTween.rotateY (layer, Random.Range(minDegree,maxDegree), rotationTime).setEase(LeanTweenType.easeInOutBack).setLoopPingPong(-1);

			}
			else{
				LeanTween.rotateY (layer, Random.Range(minDegree,maxDegree), rotationTime).setEase(LeanTweenType.easeShake).setLoopPingPong(-1);
			}
			rotationTime -= 0.5f;
		}
	
	}

	void setThirdtLayerBricks(){
		GameObject thirdlevel = GameObject.FindGameObjectWithTag("Level3");
		ArrayList structureBricks = getChildObjectsWithTag(thirdlevel,Brick.STRUCTURE);

		foreach (GameObject brick in structureBricks) {
			int prob = Random.Range(0,10);
			/*if(prob > 9)
			{
				//water brick
				brick.tag = Brick.WATER1;
				setTexture(brick,brick.tag);

			}
			else */if(prob > 8)
			{
				//water brick
				brick.tag = Brick.WATER2;
				setTexture(brick,brick.tag);

			}
			else if(prob > 1)
			{
				//make structure brick
				brick.tag = Brick.STRUCTURE;
				setTexture(brick,brick.tag);

			}
			else{
				brick.tag = Brick.NORMAL;
				setTexture(brick,brick.tag);

			}
		}

		RotateLevellayers (3,10f,150,180f);
	}

	ArrayList getChildObjectsWithTag(GameObject parent , string tag){
		ArrayList list = new ArrayList();

		Transform[] allChildren = parent.GetComponentsInChildren<Transform>();
		foreach (Transform child in allChildren) {
			// do whatever with child transform here
			if (child.tag == tag)
			{
				list.Add(child.gameObject);
			}
		}

		return list;
	}

	void swapRandomLevelBricks(int level){
		GameObject levelContainer = GameObject.FindGameObjectWithTag("Level" + level);
		ArrayList structureBricks = getChildObjectsWithTag(levelContainer,Brick.WATER1);
		ArrayList waterBricks = getChildObjectsWithTag(levelContainer,Brick.STRUCTURE);

		GameObject randWaterbrick = (GameObject) waterBricks [Random.Range (0, waterBricks.Count)];
		GameObject randStructurebrick = (GameObject) structureBricks [Random.Range (0, structureBricks.Count)];

		Vector3 tempPosition = randWaterbrick.transform.position;
		randWaterbrick.transform.position = randStructurebrick.transform.position;
		randStructurebrick.transform.position = tempPosition;

		Quaternion tempRotation = randWaterbrick.transform.rotation;
		randWaterbrick.transform.rotation = randStructurebrick.transform.rotation;
		randStructurebrick.transform.rotation = tempRotation;


	}


	void setObjectsTextureByTag(string tag){

		GameObject[] objs = GameObject.FindGameObjectsWithTag (tag);
		if (objs != null) {
			foreach (GameObject child in objs) {
				// do whatever with child transform here
				setTexture(child,tag);
	
			}
		
		}
	

	}

	public void setTexture(GameObject obj,string tag){
		if(tag == Brick.WATER1)
		{
			obj.GetComponentInChildren<Light>().enabled = false;
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = water1Texture;

		}
		else if(tag == Brick.WATER1_CRACKED)
		{
			//obj.GetComponentInChildren<Light>().enabled = false;
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = water1CrackedTexture;
			audioSource.PlayOneShot(crackAudio1);
		}
		else if(tag == Brick.WATER2)
		{
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = water2NormalTexture;
			obj.GetComponentInChildren<Light>().enabled = false;

		}
		else if(tag == Brick.WATER2_CRACKED)
		{
			//obj.GetComponentInChildren<Light>().enabled = false;
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = water2CrackedTexture;
			audioSource.PlayOneShot(crackAudio1);
		}
		else if(tag == Brick.STRUCTURE)
		{
			obj.GetComponentInChildren<Light>().enabled = false;
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = structureTexture;
		}
		else if(tag == Brick.STRUCTURE_CRACKED)
		{
			//obj.GetComponentInChildren<Light>().enabled = false;
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = structureCrackedTexture;
			audioSource.PlayOneShot(crackAudio2);
		}
		else if(tag == Brick.NORMAL)
		{
			obj.GetComponentInChildren<Light>().enabled = false;
			obj.GetComponent<MeshRenderer> ().materials [0].mainTexture = normalTexture;
		}
	}

}
