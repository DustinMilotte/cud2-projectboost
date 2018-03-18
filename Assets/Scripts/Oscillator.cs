﻿using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    [Range(0, 1)] [SerializeField] float movementFactor;

    Vector3 startingPos;

    void Start() {
        startingPos = transform.position;
        period /= ScoreKeeper.speedFactor;
    }

    void Update() {
        if (period != 0f) {
            float cycles = Time.time / period;

            const float tau = Mathf.PI * 2;
            float rawSine = Mathf.Sin(cycles * tau);

            movementFactor = (rawSine / 2) + 0.5f;
            Vector3 offset = movementFactor * movementVector;

            transform.position = startingPos + offset;
        }

    }
}
