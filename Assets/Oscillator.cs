using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

	[SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);

	[Range(0,1)]
	[SerializeField] float movementFactor; //0 for not move, 1 for move
    [SerializeField] float period = 2f;

	private Vector3 startingPos;

	// Use this for initialization
	void Start () {
		startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

        float cycles = Time.time / period;  //grows continually from 0

        const float TAU = Mathf.PI * 2;
        float rawSinWave = Mathf.Sin(cycles * TAU); // goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;

		Vector3 offset = movementFactor * movementVector;
		transform.position = startingPos + offset;
	}
}
