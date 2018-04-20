using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AForge.Fuzzy;

public class CarInferenceSystem : MonoBehaviour {


  InferenceSystem IS;

  int Dist = 15;

  // Inputs
  float distance;
  float velocity;

  // Variables controlling FIS(for setting values in editor)
  //floats floats floats

  // Called at game start - set up FIS
  void Start () {
    // linguistic labels (fuzzy sets) that compose the distances
    FuzzySet fsNear = new FuzzySet("Near",
        new TrapezoidalFunction(15, 50, TrapezoidalFunction.EdgeType.Right));
    FuzzySet fsMedium = new FuzzySet("Medium",
        new TrapezoidalFunction(15, 50, 60, 100));
    FuzzySet fsFar = new FuzzySet("Far",
        new TrapezoidalFunction(60, 100, TrapezoidalFunction.EdgeType.Left));

    // front distance (input)
    LinguisticVariable lvFront = new LinguisticVariable("FrontalDistance", 0, 120);
    lvFront.AddLabel(fsNear);
    lvFront.AddLabel(fsMedium);
    lvFront.AddLabel(fsFar);

    // linguistic labels (fuzzy sets) that compose the angle
    FuzzySet fsZero = new FuzzySet("Zero",
        new TrapezoidalFunction(-10, 5, 5, 10));
    FuzzySet fsLP = new FuzzySet("LittlePositive",
        new TrapezoidalFunction(5, 10, 20, 25));
    FuzzySet fsP = new FuzzySet("Positive",
        new TrapezoidalFunction(20, 25, 35, 40));
    FuzzySet fsVP = new FuzzySet("VeryPositive",
        new TrapezoidalFunction(35, 40, TrapezoidalFunction.EdgeType.Left));

    // angle
    LinguisticVariable lvAngle = new LinguisticVariable("Angle", -10, 50);
    lvAngle.AddLabel(fsZero);
    lvAngle.AddLabel(fsLP);
    lvAngle.AddLabel(fsP);
    lvAngle.AddLabel(fsVP);

    // the database
    Database fuzzyDB = new Database();
    fuzzyDB.AddVariable(lvFront);
    fuzzyDB.AddVariable(lvAngle);

    // creating the inference system
    IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));

    // going Straight
    IS.NewRule("Rule 1", "IF FrontalDistance IS Far THEN Angle IS Zero");
    // Turning Left
    IS.NewRule("Rule 2", "IF FrontalDistance IS Near THEN Angle IS Positive");
  }
	

	// Update is called once per frame
	void Update () {
    // inference section

    // setting inputs
    if (Input.GetKey(KeyCode.A))
      Dist++;
    if (Input.GetKey(KeyCode.D))
      Dist--;
    IS.SetInput("FrontalDistance", Dist);

    // getting outputs
    if (Input.GetKey(KeyCode.G)) {
      float newAngle = IS.Evaluate("Angle");
      print("Dist: " + Dist + " Angle: " + newAngle);
    }
  }

  // --- Public Functions --- //

  public void SetInput(float distance, float velocity) {
    this.distance = distance;
    this.velocity = velocity;
  }

  public float CalculateOutput() {
    //float output = IS.Evaluate("Response");

    // Just for now
    float output = -distance / 5;

    return output;
  }

}
