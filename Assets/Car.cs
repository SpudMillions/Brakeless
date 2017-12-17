using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour {

	[Tooltip("Controls speed at which car can turn left and right")]
	[SerializeField] private float turnSpeed = 100f;

	[Tooltip("Controls speed at which car accelerates")]
	[SerializeField]
	private float accelerationSpeed = 40f;

	[SerializeField] float levelLoadDelay = 2f;

	[SerializeField] AudioClip mainEngine;
	[SerializeField] AudioClip winLevel;
	[SerializeField] AudioClip carCrash;

	[SerializeField] ParticleSystem mainEngineParticle;
	[SerializeField] ParticleSystem winLevelParticle;
	[SerializeField] ParticleSystem carCrashParticle;



	Rigidbody rigidBody;

	AudioSource audioSource;

	enum State { Alive, Dying, Transcending };
	State state = State.Alive;

	// Use this for initialization
	private void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	private void Update () {
		if(state == State.Alive)
		{
			Accelerate();
			Turn();
		}
	}

	private void Accelerate()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			ApplyAcceleration();

		}
		else
		{
			audioSource.Stop();
			mainEngineParticle.Stop();
		}
	}

	private void ApplyAcceleration()
	{
		rigidBody.AddRelativeForce(Vector3.up * accelerationSpeed * Time.deltaTime);
		if (!audioSource.isPlaying)
		{
			audioSource.PlayOneShot(mainEngine);
		}
		mainEngineParticle.Play();
	}

	private void Turn()
	{
		rigidBody.freezeRotation = true; // take manual control of rotation

		// we want to be able to capture how fast to rotate our turn based on our rcsBoost
		float rotationThisFrame = turnSpeed * Time.deltaTime;

		if (Input.GetKey(KeyCode.A))
		{
			transform.Rotate(Vector3.forward * rotationThisFrame);
		}
		else if (Input.GetKey(KeyCode.D))
		{
			transform.Rotate(Vector3.back * rotationThisFrame);
		}

		rigidBody.freezeRotation = false; // release manual control
	}

	private void OnCollisionEnter(Collision collision)
	{
		//we only want to check while we are alive
		if (state != State.Alive) { return; }

		switch (collision.gameObject.tag)
		{
			case "Friendly":
				//do nothing
				break;
			case "Fuel":
				//add fuel
				break;
			case "Finish":
				StartWinSequence();
				break;
			default:
				//dead
				StartLoseSequence();
				break;
		}
	}

	private void StartLoseSequence()
	{
		state = State.Dying;
		audioSource.Stop();
		audioSource.PlayOneShot(carCrash);
		Invoke("LoadFirstLevel", levelLoadDelay);
	}

	private void StartWinSequence()
	{
		state = State.Transcending;
		audioSource.Stop();
		audioSource.PlayOneShot(winLevel);
		Invoke("LoadNextLevel", levelLoadDelay);
	}

	private void LoadFirstLevel()
	{
		SceneManager.LoadScene(0);
	}

	private void LoadNextLevel()
	{
		SceneManager.LoadScene(1);
	}
}
