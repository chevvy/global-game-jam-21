using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerController {
	public class PlayerController : MonoBehaviour {
		[FormerlySerializedAs("PlayerSpeed")] public float playerSpeed = 2f;
		public float turnSmoothTime = 0.1f;
		
		
		private float internalSmoothingTime = 0.5f; // using this because the value from the input are too low
		public Rigidbody playerRigidBody;
		private Vector2 playerMovement;

		private float turnSmoothVelocity;

		#region Animator
		public Animator animator;
		private static readonly int Move = Animator.StringToHash("move");
		private static readonly int Dig = Animator.StringToHash("dig");
		#endregion

		#region Unity public fonctions
		void Start() {
			// playerRigidBody = GetComponent<Rigidbody>();
			// animator = GetComponent<Animator>();
		}

		void Update() {
			
		}

		void FixedUpdate () {
			Vector3 movement = new Vector3 (playerMovement.x, 0.0f, playerMovement.y);
 
			playerRigidBody.AddForce (movement * playerSpeed);
			SetDirection(playerMovement);
		}
		#endregion
		
		/// <summary>
		/// Sets a player direction according to where the left stick is pointing
		/// </summary>
		/// <param name="movement">A vector2 of current stick movement</param>
		void SetDirection(Vector2 movement)
		{
			// Inverts direction for the eyes to be in the same orientation as the moving direction
			Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized;
			if (direction.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				// smooth angle turning motion as to not jump between different direction
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref targetAngle,
					turnSmoothTime * internalSmoothingTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
			}
		}

		#region InputCallBack

		public void OnMove(InputAction.CallbackContext context) {
			
			if(context.performed) {
				playerMovement = context.ReadValue<Vector2>();
				float playerMovementMagnitude = playerMovement.magnitude;
				try
				{
					animator.SetFloat(Move, playerMovementMagnitude);
				}
				catch (Exception e)
				{
					Debug.Log("Nom object = " + name);
					throw;
				}
				

			}
			if(context.canceled) {
				playerMovement = Vector3.zero;
				animator.SetFloat(Move, 0f);
			}
		}

		public void OnDig(InputAction.CallbackContext context) {
			if(context.performed) {
				
			}
		}
		
		#endregion
	}
}
