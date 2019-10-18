using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCamCollider : MonoBehaviour {



	public LayerMask collisionLayer; // all the objects that the camera can collide with 

	[HideInInspector]
	public bool colliding = false;
	[HideInInspector]
	public Vector3[] adjusterCameraClipPoints;
	[HideInInspector]
	public Vector3[] desiredCameraClipPoints;

	Camera cameras;

	private void Start()
	{
		Initialize(Camera.main);
		UpdateCameraClipPoints(transform.position, transform.rotation, ref adjusterCameraClipPoints);
		UpdateCameraClipPoints(Vector3.zero, transform.rotation, ref desiredCameraClipPoints);
	}

	private void FixedUpdate()
	{
		UpdateCameraClipPoints(transform.position, transform.rotation, ref adjusterCameraClipPoints);
		UpdateCameraClipPoints(Vector3.zero, transform.rotation, ref desiredCameraClipPoints);


	}

	public void Initialize(Camera cam)
	{
		cameras = cam;
		adjusterCameraClipPoints = new Vector3[5];
		desiredCameraClipPoints = new Vector3[5];
	}

	public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
	{
		if (!cameras) return;

		//clear the contents of intoArray
		intoArray = new Vector3[5];

		float z = cameras.nearClipPlane;
		float x = Mathf.Tan(cameras.fieldOfView / 3.41f) * z;
		float y = x / cameras.aspect;

		//top left
		intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition; //added and rotated the point relative to camera
		//top right
		intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition; //added and rotated the point relative to camera
		//bottom left
		intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition; //added and rotated the point relative to camera
		//bottom right
		intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition; //added and rotated the point relative to camera
		//camera's position
		intoArray[4] = cameraPosition - cameras.transform.forward;

		Debug.DrawRay(cameraPosition, intoArray[0], Color.blue);
		Debug.DrawRay(cameraPosition, intoArray[1], Color.blue);
		Debug.DrawRay(cameraPosition, intoArray[2], Color.blue);
		Debug.DrawRay(cameraPosition, intoArray[3], Color.blue);
	}

	bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
	{
		for (int i = 0; i < clipPoints.Length; i++)
		{
			Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
			float distance = Vector3.Distance(clipPoints[i], fromPosition);
			if (Physics.Raycast(ray, distance, collisionLayer))
			{
				return true;
			}
		}
		return false;
	}



	public float GetAdjustedDistanceWithRayFrom(Vector3 from)
	{
		float distance = -1;

		for (int i = 0; i < desiredCameraClipPoints.Length; i++)
		{
			Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (distance == -1)
					distance = hit.distance;
				else
				{
					if (hit.distance < distance)
						distance = hit.distance;
				}
			}
		}

		if (distance == -1) return 0;
		else return distance;
	}

	public void CheckColliding(Vector3 targetPosition)
	{
		if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
		{
			colliding = true;
		}
		else
		{
			colliding = false;
		}

	}
}
