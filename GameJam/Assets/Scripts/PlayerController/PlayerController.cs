using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace PlayerController {
	public class PlayerController : MonoBehaviour {
		public float PlayerSpeed = 2f;
		
		private Rigidbody playerRigidBody;
		private Vector2 playerMovement;
		void Start() {
			playerRigidBody = GetComponent<Rigidbody>();
		}
		
		void FixedUpdate ()
		{
			Vector3 movement = new Vector3 (playerMovement.x, 0.0f, playerMovement.y);
 
			playerRigidBody.AddForce (movement * PlayerSpeed);
		}

		#region InputCallBack

		public void OnMove(InputAction.CallbackContext context) {
			
			if(context.performed) {
				playerMovement = context.ReadValue<Vector2>();
				// left x -1
				// right x +1
				// up +1
				// down -1
			}
			if(context.canceled) {
				playerMovement = Vector3.zero;
			}
		}

		public void OnDig(InputAction.CallbackContext context) {
			if(context.performed) {
				
			}
		}
		
		#endregion
	}
}
