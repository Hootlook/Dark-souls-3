using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	SphereCollider sc;
	Vector3 camPose = new Vector3(0, 0, -5);
	bool collision;

	private void Start()
	{
		sc = GetComponent<SphereCollider>();
	}
	private void Update()
	{
		if (collision) return;

		if (transform.localPosition.z >= -5 && !collision) 
		transform.localPosition -= new Vector3(0, 0, 0.1f) * Time.deltaTime * 100;
		else return;

	}

	private void OnTriggerEnter(Collider other)
	{
		transform.localPosition -= new Vector3(0, 0, 0.1f) * Time.deltaTime * 100;
	}

	private void OnTriggerStay(Collider other)
	{
		collision = true;
		transform.localPosition += new Vector3(0, 0, 0.1f) * Time.deltaTime * 100;
		Debug.Log("Stay");
	}

	private void OnTriggerExit(Collider other)
	{
		collision = false;
	}

}
