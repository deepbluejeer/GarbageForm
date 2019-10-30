using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform player;
    public float distance;

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, player.transform.position.z - distance);
	}
}
