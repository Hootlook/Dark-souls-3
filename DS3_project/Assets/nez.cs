using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{

	public class nez : MonoBehaviour
	{
		public float v;
		public float h;
		public float walkingSpeed = 1;
		public float runningSpeed = 2;
		public float dashSpeed = 2.5f;
		public float playerSpeed = 2;
		float targetRotation;

		CameraManager camManager;
		CharacterController cc;
		public Transform cameraT;
		private float turnSmoothVelocity;
	

		void Start()
		{
			cc = GetComponent<CharacterController>();
		}

		void FixedUpdate()
		{
			v = (Input.GetAxisRaw("Vertical"));
			h = (Input.GetAxisRaw("Horizontal"));
			Vector2 input = new Vector2 (Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
			Vector2 inputDir = input.normalized;

			Vector3 move = new Vector3(h, 0, v);

			Vector3 moveDir = move.normalized;
			Vector3 velocity = transform.forward;

			cc.Move (velocity  * inputDir.magnitude * Time.deltaTime);
			
			if (inputDir != Vector2.zero)
			{
				targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, Time.fixedDeltaTime);
			}
		
			


		}

	}

}