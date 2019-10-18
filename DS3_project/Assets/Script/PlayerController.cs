using UnityEngine;
using System.Collections;
namespace SA
{
	public class PlayerController : MonoBehaviour
	{

		public float walkSpeed = 1.5f;
		public float runSpeed = 5;
		public float dashSpeed = 8;
		public float gravity = -30;
		public float jumpHeight = 1;
		[Range(0, 1)]
		public float airControlPercent;

		public float turnSmoothTime = 0.3f;
		float turnSmoothVelocity;

		public float speedSmoothTime = 0.1f;
		float speedSmoothVelocity;
		public float currentSpeed;
		float velocityY;

		Transform cameraT;
		CharacterController controller;
		private Vector3 previous;
		public float v;
		public float h;
		private float movState;
		public float playerSpeed;
		public bool running;
		public bool rolling;

		public float timer;
		public bool stepBack;

		void Start()
		{
			cameraT = Camera.main.transform;
			controller = GetComponent<CharacterController>();
		}

		void Update()
		{
			//Animator blend values
			v = Input.GetAxis("Vertical");
			h = Input.GetAxis("Horizontal");

			// input
			Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			Vector2 inputDir = input.normalized;
			Debug.Log(input);

			if (Input.GetButton("Dash"))
			{
				timer += Time.deltaTime;
			}

			running = (timer > 0.5) ? running = true : running = false;

			if (rolling || stepBack) return;
			if (Input.GetButtonUp("Dash")) 
			{
				if (inputDir.x + inputDir.y != 0 && timer < 0.3)
				{
					StartCoroutine(roll());
				}
				if (inputDir.x == 0 && inputDir.y == 0 && timer < 0.3)
				{
					StartCoroutine(StepBack());
				}
				timer = 0;
			}
			else
			Move(inputDir, running);

		}

		IEnumerator StepBack()
		{
			stepBack = true;
			velocityY += Time.deltaTime * gravity;
			Vector3 velocity = Vector3.up * velocityY;
			controller.Move(velocity * Time.smoothDeltaTime);
			yield return new WaitForSeconds(0.8f);
			stepBack = false;
		}
		IEnumerator roll()
		{
			rolling = true;
			velocityY += Time.deltaTime * gravity;
			Vector3 velocity = Vector3.up * velocityY;
			controller.Move(velocity * Time.smoothDeltaTime);
			yield return new WaitForSeconds(0.8f);
			rolling = false;
		}

		void Move(Vector2 inputDir, bool isRunning)
		{
			if (inputDir != Vector2.zero)
			{
				float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
				transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
			}

			float walking = ((v < 0.7f && v > -0.7f && h < 0.7f && h > -0.7f) ? walkSpeed : runSpeed);

			playerSpeed = ((isRunning) ? dashSpeed : walking) * inputDir.magnitude;

			currentSpeed = Mathf.SmoothDamp(currentSpeed, playerSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

			velocityY += Time.deltaTime * gravity;
			Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

			controller.Move(velocity * Time.deltaTime);

			currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

			if (controller.isGrounded)
			{
				velocityY = 0;
			}

		}

		void Jump()
		{
			if (controller.isGrounded)
			{
				float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
				velocityY = jumpVelocity;
			}
		}

		float GetModifiedSmoothTime(float smoothTime)
		{
			if (controller.isGrounded)
			{
				return smoothTime;
			}

			if (airControlPercent == 0)
			{
				return float.MaxValue;
			}
			return smoothTime / airControlPercent;
		}
	}

}