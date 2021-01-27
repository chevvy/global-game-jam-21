using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace PlayerController {
	public class PlayerController : MonoBehaviour {
		public float PlayerSpeed = 2f;
		
		private Rigidbody playerRigidBody;
		private Vector2 playerMovement;

		#region Unity public fonctions
		void Start() {
			playerRigidBody = GetComponent<Rigidbody>();
		}

		void Update() {
			SetDirection(playerMovement);
		}

		void FixedUpdate () {
			Vector3 movement = new Vector3 (playerMovement.x, 0.0f, playerMovement.y);
 
			playerRigidBody.AddForce (movement * PlayerSpeed);
		}
		#endregion
		
		/// <summary>
		/// Sets a player direction according to where the left stick is pointing
		/// </summary>
		/// <param name="movement">A vector2 of current stick movement</param>
		void SetDirection(Vector2 movement)
		{
			// Inverts direction for the eyes to be in the same orientation as the moving direction
			Vector3 direction = new Vector3(-movement.x, 0f, -movement.y).normalized;
			if (direction.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
			}
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
