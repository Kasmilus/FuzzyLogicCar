using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour {

  // UI references
  public Text distanceText;
  public Text velocityText;
  public Text outputText;

  // Car Control references
  private CarInferenceSystem carIS;
  private Transform LineTransform;

  // Control variables
  private float distance;
  private float velocity;
  private float lastFramePosX;

  private float timer;

	void Start () {
    carIS = GetComponent<CarInferenceSystem>();
    LineTransform = FindObjectOfType<LineControl>().transform;

    velocity = 0;
    timer = 0;
  }
	
	void Update () {
    timer += Time.deltaTime;

    // Distance
    distance = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(LineTransform.position.x, 0));
    if (transform.position.x < LineTransform.position.x)
      distance = -distance;

    // Velocity
    velocity = (transform.position.x - lastFramePosX);
    lastFramePosX = transform.position.x;

    // Pass variables to FIS
    carIS.SetInput(distance, velocity);
    float response = carIS.CalculateOutput();

    // Move car
    transform.position += new Vector3(response * Time.deltaTime, 0, 0);

    // UI
    if (timer > 0.15) {
      distanceText.text = "Distance: " + distance.ToString("F2");
      velocityText.text = "Velocity: " + velocity.ToString("F2");
      outputText.text = "Output: " + response.ToString("F2");

      timer = 0;
    }
  }
}
