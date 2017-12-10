using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour {

    [Tooltip("Controls speed at which car can turn left and right")]
    [SerializeField] float rcsBoost = 100f;

    [Tooltip("Controls speed at which car accelerates")]
    [SerializeField]
    float mainBoost = 40f;

    Rigidbody rigidBody;
	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		rigidBody = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		Boost();
		Turn();
	}

	private void Boost()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			rigidBody.AddRelativeForce(Vector3.up * mainBoost);
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
        float rotationThisFrame = rcsBoost * Time.deltaTime;

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

	
}
