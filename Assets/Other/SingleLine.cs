using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleLine : MonoBehaviour {


  float speed ;
  float lifeTime;
  float destroyAfter;

	void Start () {
    speed = 25;
    lifeTime = 0;
    destroyAfter = 60;
  }
	
	void Update () {
    transform.position += new Vector3(0, 0, -speed * Time.deltaTime);

    lifeTime += Time.deltaTime;

    if (lifeTime > destroyAfter)
      Destroy(this.gameObject);
  }
}
