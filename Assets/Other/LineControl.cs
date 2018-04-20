using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineControl : MonoBehaviour {

  // --- Public: --- //
  // Text object reference(To be set in editor)
  public Text speedText;
  // Single line prefab
  public GameObject linePrefab;
  // Spawn pos transform reference
  public Transform FirstLineSpawnTransform;

  // Speed control
  public float speedMin = 1;
  public float speedMax = 15;

  // --- Private: --- //
  // Default speed
  private float speed = 10;
  private Vector3 lastMousePos;
  private bool wasMousePressed;
  //Spawning new lines
  private Vector3 spawnPos;
  private float timer;

  private void Start() {
    spawnPos = FirstLineSpawnTransform.localPosition;
    spawnPos.z *= transform.localScale.z;
    timer = 0;
    wasMousePressed = false;
  }

  void Update () {

    timer += Time.deltaTime;
    if(timer > 0.4f) {
      timer = 0;
      GameObject.Instantiate(linePrefab, transform.position + spawnPos, Quaternion.identity, this.transform.transform);
    }

    // Input controlling speed
    float inputSpeed = 0;
    if(Input.GetButtonDown("SpeedUp")) {
      inputSpeed = 1;
    }
    else if (Input.GetButtonDown("SpeedDown")) {
      inputSpeed = -1;
    }

    // Add input to speed and apply limits
    if (inputSpeed != 0) {
      speed += inputSpeed;
      if (speed < speedMin)
        speed = speedMin;
      else if (speed > speedMax)
        speed = speedMax;

      if (speedText) {
        speedText.text = "Speed: " + speed;
      }
    }

    // Horizontal input moving the line
    float inputHor = Input.GetAxis("Horizontal");
    if (inputHor != 0) {
      transform.position += new Vector3(inputHor * Time.deltaTime * speed, 0, 0);
    }

    if (Input.GetMouseButton(0)) {
      if(Input.mousePosition != lastMousePos && wasMousePressed) {
        transform.position += new Vector3((Input.mousePosition - lastMousePos).x * Time.deltaTime, 0, 0);
      }
      lastMousePos = Input.mousePosition;
    }
    if (Input.GetMouseButtonDown(0)) {
      wasMousePressed = true;
    }
    if (Input.GetMouseButtonUp(0)) {
      wasMousePressed = false;
    }

  }

}
