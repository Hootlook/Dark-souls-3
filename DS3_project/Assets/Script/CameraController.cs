using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UP AND DOWN
public class CameraController : MonoBehaviour {

	public float distance = 0.1f;
	public float horizontal = 10f;
	public float vertical = 10f;
	[Range (1,20)]
	public float rotationSpeedY = 1;
	[Range(1, 20)]
	public float rotationSpeedX = 1;
	public Transform Target;
	public Transform camTransform;

	void Start () {

		camTransform = transform;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

	}

	void Update () {
	
		vertical -= Input.GetAxis("RightAxis Y") * rotationSpeedY;
		horizontal -=  Input.GetAxis("RightAxis X") * rotationSpeedX;
		vertical = Mathf.Clamp(vertical, -80, 80);
	
	}

	private void LateUpdate()
	{
		Vector3 dir = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(vertical, horizontal, 0);
		camTransform.position = Target.position + rotation * dir;
		camTransform.LookAt(Target.position);
	}


}
