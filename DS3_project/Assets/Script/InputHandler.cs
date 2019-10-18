using UnityEngine;

namespace SA
{
	public class InputHandler : MonoBehaviour {

		float horizontal;
		float vertical;

		StateManager states;
		CameraManager camManager;

		float delta;

		// Use this for initialization
		void Start() {

			states = GetComponent<StateManager>();
			states.Init();

			camManager = CameraManager.singleton;
			camManager.Init(transform);
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			delta = Time.fixedDeltaTime;
			GetInput();
			UpdateStates();
			states.FixedTick(delta);
		}

		void Update()
		{
			delta = Time.deltaTime;
			camManager.Tick(delta);	
		}

		void GetInput()
		{
			vertical = Input.GetAxis("Vertical");
			horizontal = Input.GetAxis("Horizontal");
		}

		void UpdateStates()
		{
			states.horizontal = horizontal;
			states.vertical = vertical;

			Vector3 v = vertical * camManager.transform.forward;
			Vector3 h = horizontal * camManager.transform.right;
			states.moveDir = (v + h).normalized;
			float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			states.moveAmount = Mathf.Clamp01(m);

		}

	}
}