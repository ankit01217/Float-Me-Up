using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class HammerController : MonoBehaviour {

	AudioSource audioSource;
	public AudioClip boyVO1, brickSmashAudio, waterDropAudio, waterLevelUpAudio, structureParticlesAudio, surpriseParticlesAudio, hammerAudio, fellDownAudio, screamAudio, shakeAudio,positiveGrunt;
	public AudioClip[] waterGruntSoundArr;
	public AudioClip[] floatSounds;
	public AudioClip[] surpriseGruntSoundArr;
	public AudioClip fearGrunt;
	public GameObject gem;
	public GameObject waterLarge;
	public GameObject waterMedium;
	public GameObject waterSmall;
	public GameObject gemPosition;
	public GameObject brickParticle, wellCollapse;
	public int maxWaterBricks;
	public int maxStructureBricks;
	public GameObject sunlight;
	public float gameTimerSpeed;
	public float wellOffset;
	public float hammeroffset = 0.3f;
	public float minShakeOffset = 0f;
	GameObject handController, playerController;

	GameObject well;
	int noOfStructureBricksDestroyed = 0;
	int noOfWaterBricksDestroyed = 0;
	public float hitVelocity = 10f;
	LevelController levelController;
	bool isHammerAnimationEnabled = true;
	bool didHit = false;
	public static int gameOverState = 0; //1 - success, 2- failure - out of time , 3- failure - well fell down
	public Transform particleTarget;

	public AudioClip strange1, strange2, water_hint1, water_hint2, water_hint3;
	public AudioClip[] posSounds,negSounds;
	GameObject plank,fader;

	// Use this for initialization
	void Start () {

		plank = GameObject.FindGameObjectWithTag("Plank");
		fader = GameObject.FindGameObjectWithTag("Fader");
		LeanTween.alpha (fader, 0f, 0.1f);


		levelController = FindObjectOfType<LevelController>();
		handController = GameObject.FindGameObjectWithTag("HandController");
		playerController = GameObject.FindGameObjectWithTag("OVRPlayerController");
		audioSource = GetComponent<AudioSource> ();
		well = GameObject.FindGameObjectWithTag ("Well");
	
		Invoke ("playVoiceOver", 1f);
	}

	void playVoiceOver(){
		audioSource.PlayOneShot(strange1);
		LeanTween.delayedCall (strange1.length + 0.5f, () => {
			audioSource.PlayOneShot(boyVO1);
			LeanTween.delayedCall (boyVO1.length + 0.5f, () => {
				audioSource.PlayOneShot(water_hint2);
			});

		});

	}

	// Update is called once per frame
	void Update () {

		Debug.Log("handle vel "+Mathf.Abs (PSMoveExample.handleVelocity.z) + ", " + hitVelocity );
		if ((Mathf.Abs (PSMoveExample.handleVelocity.z) > hitVelocity || Input.GetKeyDown(KeyCode.H) == true) && didHit == false && isHammerAnimationEnabled == true) {
			isHammerAnimationEnabled = false;
			Invoke ("EnableHammerAnimation", 0.8f);
			Debug.Log("HIT!!!!!!!!!");
			didHit = true;
			startHammerAnimation();
		}


	}
	void EnableHammerAnimation(){
		didHit = false;
		isHammerAnimationEnabled = true;
	}

	void startHammerAnimation(){
		LeanTween.moveLocalZ (handController, handController.transform.localPosition.z + hammeroffset, 0.1f).setLoopPingPong(1).setOnComplete (
			() => {
			//EnableHammerAnimation();
		});


	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log ("Hand OnTriggerEnter " + other.gameObject.tag);

		//get handle velocity in Z diirection
		if (didHit == false) {
			return;
		} else {
			didHit = false;
		}


		switch (other.gameObject.tag) {
		case Brick.STRUCTURE:

			noOfStructureBricksDestroyed++;
			levelController.setTexture(other.gameObject,Brick.STRUCTURE_CRACKED);
			LeanTween.delayedCall (0.1f, () => {
				//Debug.Log("STRUCTURE Brick " + noOfStructureBricksDestroyed + " maxStructureBricks" + maxStructureBricks);
				Destroy(other.gameObject);
				
				//add structure brick particles here
				audioSource.PlayOneShot(brickSmashAudio);
				audioSource.PlayOneShot(waterGruntSoundArr[Random.Range(0,waterGruntSoundArr.Length)]);
			
				addStructureBrickParticlesAnimation(other,brickParticle);
				if(noOfStructureBricksDestroyed == maxStructureBricks && gameOverState == 0)
				{
					//addStructureBrickParticlesAnimation(other,fallingStones);

					wellCollapse.SetActive(true);
					audioSource.PlayOneShot(screamAudio);
					Invoke("playDeathSound",0.4f);
					Invoke("onWellCollapseAnimationEnd",3f);

				}
				else
				{
					LeanTween.delayedCall (1f, () => {
						audioSource.PlayOneShot(negSounds[Random.Range(0,negSounds.Length)]);
						
					});
				}
				
			});




			break;
		case Brick.WATER1:
		case Brick.WATER2_CRACKED:

			LeanTween.cancel(other.gameObject.transform.parent.gameObject);
			
			noOfWaterBricksDestroyed++;

			levelController.setTexture(other.gameObject,Brick.WATER1_CRACKED);
			LeanTween.delayedCall (0.1f, () => {
				Destroy(other.gameObject);
				addWaterAnimation(other);
				Debug.Log("WATER Brick");
				audioSource.PlayOneShot(brickSmashAudio);
				audioSource.PlayOneShot(waterGruntSoundArr[Random.Range(0,waterGruntSoundArr.Length)]);
				LeanTween.delayedCall (brickSmashAudio.length + 0.3f, () => {
					if(noOfWaterBricksDestroyed == 1)
					{
						audioSource.PlayOneShot(water_hint3);
						
					}
					else{
						audioSource.PlayOneShot(posSounds[Random.Range(0,posSounds.Length)]);
						
					}	
				});

			});


			Invoke("increaseWaterLayer",0.5f);

			
			break;

		case Brick.WATER2:


			//Debug.Log("WATER Brick");
			audioSource.PlayOneShot(hammerAudio);
			audioSource.PlayOneShot(waterGruntSoundArr[Random.Range(0,waterGruntSoundArr.Length)]);

			//change tag to normal water brick
			other.gameObject.tag= Brick.WATER1;
			levelController.setTexture(other.gameObject, Brick.WATER2_CRACKED);


			break;


		case Brick.NORMAL:


			//Debug.Log("NORMAL Brick");
			audioSource.PlayOneShot(hammerAudio);
			audioSource.PlayOneShot(surpriseGruntSoundArr[Random.Range(0,surpriseGruntSoundArr.Length)]);

			//add surprise brick particles here

			break;
		
		default:

			Debug.Log("Undefined Brick");
			break;
		}


	}

	void playDeathSound(){
		audioSource.PlayOneShot(fellDownAudio);
		killPlayer();
	}


	
	void onWellCollapseAnimationEnd(){
		onGameEnd(2);
	}


	void addWaterAnimation(Collider other){

		float waterRotationX;
		float waterRotationY;
		float waterRotationZ;
		Vector3 waterRotation;
		float waterPositionX;
		float waterPositionY;
		float waterPositionZ;
		Vector3 waterPosition;

		waterRotationX = gem.transform.rotation.x;
		waterRotationY = gem.transform.rotation.y;
		waterRotationZ = gem.transform.rotation.z;
		
		waterPositionX = gemPosition.transform.position.x;
		waterPositionY = gemPosition.transform.position.y;
		waterPositionZ = gemPosition.transform.position.z;
		
		waterRotation = new Vector3(waterRotationX, waterRotationY, waterRotationZ);
		waterPosition = new Vector3(waterPositionX, waterPositionY, waterPositionZ);

		if (LevelController.curLayer == LevelController.maxLayers) {
			waterPosition = new Vector3(waterPositionX, waterPositionY - 0.3f, waterPositionZ);

		}

		GameObject waterPS = (GameObject)Instantiate(waterLarge, waterPosition, Quaternion.Euler(new Vector3(180,0,0))); //
		audioSource.PlayOneShot(waterDropAudio);
		audioSource.PlayOneShot (positiveGrunt);

		//shakePlayer();
		//Invoke ("shakePlayer", 0.1f);	
	}

	void addStructureBrickParticlesAnimation(Collider other,GameObject particlePrefab){

		//shake well first
		shakeWell();
		//shakePlayer();

		float bRotationX;
		float bRotationY;
		float bRotationZ;
		Vector3 bRotation;
		float bPositionX;
		float bPositionY;
		float bPositionZ;
		Vector3 bPosition;
		
		bRotationX = other.gameObject.transform.rotation.x;
		bRotationY = other.gameObject.transform.rotation.y;
		bRotationZ = other.gameObject.transform.rotation.z;
		
		bPositionX = gemPosition.transform.position.x;
		bPositionY = gemPosition.transform.position.y;
		bPositionZ = gemPosition.transform.position.z ;
		
		bRotation = new Vector3(bRotationX, bRotationY, bRotationZ);
		bPosition = new Vector3(bPositionX, bPositionY, bPositionZ);
		
		Instantiate(particlePrefab, bPosition, Quaternion.Euler(180,0,0));
		audioSource.PlayOneShot(structureParticlesAudio);

	}

	void shakeWell(){
		audioSource.PlayOneShot (shakeAudio);
		minShakeOffset += 5f;
		//Quaternion newRot = Quaternion.Euler (new Vector3(well.transform.rotation.eulerAngles.x,well.transform.rotation.eulerAngles.y,well.transform.rotation.eulerAngles.z + minShakeOffset));
		//LeanTween.rotateLocal(well,newRot.eulerAngles,0.05f).setLoopPingPong(2);
		LeanTween.moveLocalZ(well,0.1f,0.08f).setLoopPingPong(2);;
		//LeanTween.moveLocalZ(well,0.5f,0.15f).setLoopPingPong(2);;
		audioSource.PlayOneShot (fearGrunt);


	}

	void shakePlayer(){
		Quaternion newPlayerRot = Quaternion.Euler (new Vector3(playerController.transform.rotation.eulerAngles.x,playerController.transform.rotation.eulerAngles.y + 10f,playerController.transform.rotation.eulerAngles.z));
		//LeanTween.moveLocalY(playerController,0.3f,0.25f).setLoopPingPong(1);;
		LeanTween.rotateX(handController,5f,0.25f).setLoopPingPong(1);;
		LeanTween.rotateX(plank,3f,0.25f).setLoopPingPong(1);;

	}

	void killPlayer(){
		LeanTween.rotateZ (handController, 60f, 0.35f).setEase(LeanTweenType.easeOutBack);
		LeanTween.rotateZ(plank,60f,0.35f).setEase(LeanTweenType.easeOutBack);;

	}
	
	
	void increaseWaterLayer(){

			audioSource.PlayOneShot(floatSounds[Random.Range(0,floatSounds.Length)]);
			LevelController.curLayer++;
			if (LevelController.curLayer != LevelController.maxLayers) {
				levelController.updateWellLayer ();

		    }
			
			LeanTween.moveY (well, well.transform.position.y -wellOffset, 1f).setEase(LeanTweenType.easeSpring).setOnComplete(
			()=>{

			Debug.Log("curLayer"+ LevelController.curLayer + " maxlayer :" + LevelController.maxLayers);
				if(LevelController.curLayer == LevelController.maxLayers)
				{
					onGameEnd(1);
				}
			}
		);

		audioSource.PlayOneShot(waterLevelUpAudio);


	}


	void onGameEnd(int endState){
		gameOverState = endState;

		LeanTween.alpha (fader, 1f, 0.3f).setOnComplete(startEndScene);

		//Invoke ("startEndScene", 0.05f);
	}

	void startEndScene(){
		Application.LoadLevel("End");

	}

}
