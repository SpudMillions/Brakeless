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
			rigidBody.AddRelativeForce(Vector3.up * accelerationSpeed);
			if (!audioSource.isPlaying)
			{
				audioSource.Play();
			}

		}
		else
		{
			audioSource.Stop();
		}
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
				state = State.Transcending;
				Invoke("LoadNextLevel", 1f);
				break;
			default:
				//dead
				state = State.Dying;
				Invoke("LoadFirstLevel", 1f);
				break;
		}
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
