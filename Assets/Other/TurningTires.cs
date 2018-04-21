using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningTires : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    transform.Rotate(new Vector3(Time.deltaTime * 650, 0, 0));
	}
}
