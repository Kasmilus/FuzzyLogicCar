﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AForge.Fuzzy;

public class CarInferenceSystem : MonoBehaviour {


  private InferenceSystem IS;
  private Database fuzzyDB;

  // Inputs
  private float distance;
  private float velocity;

  // Variables controlling FIS(for setting values in editor)
  public bool isRedCar;

  // Called at game start - set up FIS
  void Start () {

    // Create the inference system
    SetupFuzzySets(isRedCar);
    IS = new InferenceSystem(fuzzyDB, new CentroidDefuzzifier(1000));
    SetupRuleBase(isRedCar);

  }
	

	void Update () {

  }

  // Setup input variables
  // @isHastyDriver - will use triangles instead of trapezoids
  private void SetupFuzzySets(bool isHastyDriver) {

    // Linguistic labels (MFs) that compose the input velocity
    FuzzySet fsLargeLeftVelocityInput = new FuzzySet("LargeLeft",
        new TrapezoidalFunction(-4.0f, -2.75f, TrapezoidalFunction.EdgeType.Right));
    FuzzySet fsLeftVelocityInput = new FuzzySet("Left",
        new TrapezoidalFunction(-3.5f, -2.2f, -0.8f, 0.0f));
    FuzzySet fsZeroVelocityInput = new FuzzySet("Zero",
        new TrapezoidalFunction(-0.5f, 0.0f, 0.5f));
    FuzzySet fsRightVelocityInput = new FuzzySet("Right",
        new TrapezoidalFunction(0.0f, 0.8f, 2.2f, 3.5f));
    FuzzySet fsLargeRightVelocityInput = new FuzzySet("LargeRight",
        new TrapezoidalFunction(2.75f, 4.0f, TrapezoidalFunction.EdgeType.Left));
    
    // Fuzzy set - Input Velocity
    LinguisticVariable lvInputVelocity = new LinguisticVariable("InputVelocity", -5.0f, 5.0f);
    lvInputVelocity.AddLabel(fsLargeLeftVelocityInput);
    lvInputVelocity.AddLabel(fsLeftVelocityInput);
    lvInputVelocity.AddLabel(fsZeroVelocityInput);
    lvInputVelocity.AddLabel(fsRightVelocityInput);
    lvInputVelocity.AddLabel(fsLargeRightVelocityInput);

    // linguistic labels (MFs) that compose the input distance
    FuzzySet fsFarLeftDistance = new FuzzySet("FarLeft",
        new TrapezoidalFunction(-4.5f, -2.8f, TrapezoidalFunction.EdgeType.Right));
    FuzzySet fsLeftDistance = new FuzzySet("Left",
        new TrapezoidalFunction(-3.8f, -2.5f, -1.5f, -0.5f));
    FuzzySet fsZeroDistance = new FuzzySet("Zero",
        new TrapezoidalFunction(-1.0f, -0.25f, 0.25f, 1.0f));
    FuzzySet fsRightDistance = new FuzzySet("Right",
        new TrapezoidalFunction(0.5f, 1.5f, 2.5f, 3.8f));
    FuzzySet fsFarRightDistance = new FuzzySet("FarRight",
        new TrapezoidalFunction(2.8f, 4.5f, TrapezoidalFunction.EdgeType.Left));

    // Fuzzy set - Input Distance
    LinguisticVariable lvInputDistance = new LinguisticVariable("InputDistance", -6, 6);
    lvInputDistance.AddLabel(fsFarLeftDistance);
    lvInputDistance.AddLabel(fsLeftDistance);
    lvInputDistance.AddLabel(fsZeroDistance);
    lvInputDistance.AddLabel(fsRightDistance);
    lvInputDistance.AddLabel(fsFarRightDistance);

    // Linguistic labels (MFs) that compose the output velocity
    FuzzySet fsLargeLeftVelocityOutput = new FuzzySet("LargeLeftOutput",
        new TrapezoidalFunction(-4.5f, -2.1f, TrapezoidalFunction.EdgeType.Right));
    FuzzySet fsLeftVelocityOutput = new FuzzySet("LeftOutput",
        new TrapezoidalFunction(-3.0f, -1.5f, 0.0f));
    FuzzySet fsZeroVelocityOutput = new FuzzySet("ZeroOutput",
        new TrapezoidalFunction(-0.5f, 0.0f, 0.5f));
    FuzzySet fsRightVelocityOutput = new FuzzySet("RightOutput",
        new TrapezoidalFunction(0.0f, 1.5f, 3.0f));
    FuzzySet fsLargeRightVelocityOutput = new FuzzySet("LargeRightOutput",
        new TrapezoidalFunction(2.1f, 4.5f, TrapezoidalFunction.EdgeType.Left));

    // Fuzzy set - Output Velocity
    LinguisticVariable lvOutputVelocity = new LinguisticVariable("OutputVelocity", -5.0f, 5.0f);
    lvOutputVelocity.AddLabel(fsLargeLeftVelocityOutput);
    lvOutputVelocity.AddLabel(fsLeftVelocityOutput);
    lvOutputVelocity.AddLabel(fsZeroVelocityOutput);
    lvOutputVelocity.AddLabel(fsRightVelocityOutput);
    lvOutputVelocity.AddLabel(fsLargeRightVelocityOutput);

    // Database
    fuzzyDB = new Database();
    fuzzyDB.AddVariable(lvInputVelocity);
    fuzzyDB.AddVariable(lvInputDistance);
    fuzzyDB.AddVariable(lvOutputVelocity);
  }

  // Setup rules
  // @isHastyDriver - will react quicker when going out of the road(2 rules changed)
  private void SetupRuleBase(bool isHastyDriver) {

    // 25 rules coming from FAM
    IS.NewRule("Rule 1", "IF InputVelocity IS LargeRight AND InputDistance IS FarRight THEN OutputVelocity IS LargeLeftOutput");
    IS.NewRule("Rule 2", "IF InputVelocity IS Right AND InputDistance IS FarRight THEN OutputVelocity IS LargeLeftOutput");
    IS.NewRule("Rule 3", "IF InputVelocity IS Zero AND InputDistance IS FarRight THEN OutputVelocity IS LargeLeftOutput");
    IS.NewRule("Rule 4", "IF InputVelocity IS Left AND InputDistance IS FarRight THEN OutputVelocity IS LeftOutput");
    IS.NewRule("Rule 5", "IF InputVelocity IS LargeLeft AND InputDistance IS FarRight THEN OutputVelocity IS ZeroOutput");

    IS.NewRule("Rule 6", "IF InputVelocity IS LargeRight AND InputDistance IS Right THEN OutputVelocity IS LargeLeftOutput");
    if(isHastyDriver)
      IS.NewRule("Rule 7", "IF InputVelocity IS Right AND InputDistance IS Right THEN OutputVelocity IS LargeLeftOutput");
    else
      IS.NewRule("Rule 7", "IF InputVelocity IS Right AND InputDistance IS Right THEN OutputVelocity IS LeftOutput");
    IS.NewRule("Rule 8", "IF InputVelocity IS Zero AND InputDistance IS Right THEN OutputVelocity IS LeftOutput");
    IS.NewRule("Rule 9", "IF InputVelocity IS Left AND InputDistance IS Right THEN OutputVelocity IS ZeroOutput");
    IS.NewRule("Rule 10", "IF InputVelocity IS LargeLeft AND InputDistance IS Right THEN OutputVelocity IS RightOutput");

    IS.NewRule("Rule 11", "IF InputVelocity IS LargeRight AND InputDistance IS Zero THEN OutputVelocity IS LargeLeftOutput");
    IS.NewRule("Rule 12", "IF InputVelocity IS Right AND InputDistance IS Zero THEN OutputVelocity IS LeftOutput");
    IS.NewRule("Rule 13", "IF InputVelocity IS Zero AND InputDistance IS Zero THEN OutputVelocity IS ZeroOutput");
    IS.NewRule("Rule 14", "IF InputVelocity IS Left AND InputDistance IS Zero THEN OutputVelocity IS RightOutput");
    IS.NewRule("Rule 15", "IF InputVelocity IS LargeLeft AND InputDistance IS Zero THEN OutputVelocity IS LargeRightOutput");

    IS.NewRule("Rule 16", "IF InputVelocity IS LargeRight AND InputDistance IS Left THEN OutputVelocity IS LeftOutput");
    IS.NewRule("Rule 17", "IF InputVelocity IS Right AND InputDistance IS Left THEN OutputVelocity IS ZeroOutput");
    IS.NewRule("Rule 18", "IF InputVelocity IS Zero AND InputDistance IS Left THEN OutputVelocity IS RightOutput");
    if(isHastyDriver)
      IS.NewRule("Rule 19", "IF InputVelocity IS Left AND InputDistance IS Left THEN OutputVelocity IS LargeRightOutput");
    else
      IS.NewRule("Rule 19", "IF InputVelocity IS Left AND InputDistance IS Left THEN OutputVelocity IS RightOutput");
    IS.NewRule("Rule 20", "IF InputVelocity IS LargeLeft AND InputDistance IS Left THEN OutputVelocity IS LargeRightOutput");

    IS.NewRule("Rule 21", "IF InputVelocity IS LargeRight AND InputDistance IS FarLeft THEN OutputVelocity IS ZeroOutput");
    IS.NewRule("Rule 22", "IF InputVelocity IS Right AND InputDistance IS FarLeft THEN OutputVelocity IS RightOutput");
    IS.NewRule("Rule 23", "IF InputVelocity IS Zero AND InputDistance IS FarLeft THEN OutputVelocity IS LargeRightOutput");
    IS.NewRule("Rule 24", "IF InputVelocity IS Left AND InputDistance IS FarLeft THEN OutputVelocity IS LargeRightOutput");
    IS.NewRule("Rule 25", "IF InputVelocity IS LargeLeft AND InputDistance IS FarLeft THEN OutputVelocity IS LargeRightOutput");
  }


  // --- Public Functions --- //

  public void SetInput(float distance, float velocity) {
    this.distance = distance;
    this.velocity = velocity;

    IS.SetInput("InputVelocity", velocity);
    IS.SetInput("InputDistance", distance);
  }

  public float CalculateOutput() {
    float output = IS.Evaluate("OutputVelocity");

    // Just for now
   // float output = -distance / 5;

    return output;
  }

}
