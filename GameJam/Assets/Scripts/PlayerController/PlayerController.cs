using System;
using System.Collections;
using Cinemachine;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace PlayerController {
	public class PlayerController : MonoBehaviour {
		public int playerID;
		
		[FormerlySerializedAs("PlayerSpeed")] public float playerSpeed = 2f;
		public float turnSmoothTime = 0.1f;
		
		
		private float internalSmoothingTime = 0.5f; // using this because the value from the input are too low
		public Rigidbody playerRigidBody;
		private Vector2 playerMovement;
		private bool isGettingAttacked = false;
		private Vector3 attackDirection = Vector3.zero;
		public int attackForce = 8000;
		public float playerHeight = 1.5f;

		private float turnSmoothVelocity;

		private GameManager _gameManager;
		public GameObject goldBitLootedFX;
		public CinemachineBrain cameraBrain;

		[SerializeField] Material[] playerMaterials;
		[SerializeField] private SkinnedMeshRenderer playerMeshRender;

		public bool isGameFinished = false;
		
		#region Animator variables
		public Animator animator;
		private static readonly int Move = Animator.StringToHash("move");
		private static readonly int Dig = Animator.StringToHash("dig");
		private static readonly int Attack = Animator.StringToHash("attack");
		#endregion

		#region Audio

		public AudioSource emptyStrike;
		public AudioSource hit1;
		public AudioSource gainRing;
		private AudioSource DigMiss => GameManager.Instance.digMiss;
		public AudioSource[] walkSFX;
		
		#endregion

		#region Unity public fonctions
		void Start() {
			_gameManager = GameManager.Instance;
			_gameManager.AddCameraTarget(this);
			cameraBrain = _gameManager.cameraBrain;
			if (playerMaterials[playerID] == null) {
				Debug.LogError("Missing material for playerID on playerPrefab");
			}

			if (playerMeshRender == null) {
				Debug.LogError("Missing meshRender reference on PlayerPrefab");
			} 
			playerMeshRender.material = playerMaterials[playerID];
		}

		private void Update() {
			if (!CanPlayerAct()) { return; }
			ApplyAttackForce();
		}

		private void ApplyAttackForce() {
			if (!isGettingAttacked || attackDirection == Vector3.zero) return;
			playerRigidBody.AddForce(attackDirection.normalized * attackForce);
			attackDirection = Vector3.zero;
		}

		void FixedUpdate () {
			if (!CanPlayerAct()) { return; }
			Vector3 movement = new Vector3 (playerMovement.x, 0.0f, playerMovement.y);
			movement = cameraBrain.transform.rotation * movement;
			playerRigidBody.AddForce (movement * playerSpeed);
			SetDirection(playerMovement);
			
			if(transform.position.y < 1.4f || transform.position.y > 1.6f) {
				transform.position = new Vector3(transform.position.x, playerHeight, transform.position.z);
			}
		}
		#endregion
		
		/// <summary>
		/// Sets a player direction according to where the left stick is pointing
		/// </summary>
		/// <param name="movement">A vector2 of current stick movement</param>
		void SetDirection(Vector2 movement)
		{
			// Inverts direction for the eyes to be in the same orientation as the moving direction
			// Vector3 direction = new Vector3(movement.x, 0f, movement.y).normalized;
			Vector3 direction = cameraBrain.transform.rotation * new Vector3(movement.x, 0f, movement.y);
			direction = direction.normalized;
			if (direction.magnitude >= 0.1f)
			{
				float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
				// smooth angle turning motion as to not jump between different direction
				float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref targetAngle,
					turnSmoothTime * internalSmoothingTime);
				transform.rotation = Quaternion.Euler(0f, angle, 0f);
			}
		}

		public void LootDataNode() {
			GameManager.Instance.AddPointToPlayer(playerID);
			goldBitLootedFX.SetActive(false);
			goldBitLootedFX.SetActive(true);

			StartCoroutine(Timer());
			IEnumerator Timer() {
				yield return new WaitForSeconds(0.5f);
				goldBitLootedFX.SetActive(false);
			}
			gainRing.Play();
		}

		public void GetsAttacked(Vector3 attackPosition) {
			isGettingAttacked = true;
			attackDirection = transform.position - attackPosition;
			hit1.Play();

			StartCoroutine(WaitBeforeSpawningLootableNodes());
			IEnumerator WaitBeforeSpawningLootableNodes() {
				yield return new WaitForSeconds(0.2f);
				GameManager.Instance.PlayerTakesDamages(playerID, transform.position);
			}
		}

		#region Audio functions

		public void PlayDigSound() {
			DigMiss.Play();
		}

		public void PlayWalkSound() {
			AudioSource selectedClip = walkSFX[Random.Range(0, 2)];
			selectedClip.pitch = Random.Range(0.88f, 1.10f);
			selectedClip.Play();
		}

		#endregion
		

		#region InputCallBack and Player Actions
		public void OnMove(InputAction.CallbackContext context) {
			if (!IsAnimatorValid() || !CanPlayerAct()) { return; }
			
			float playerMovementMagnitude = playerMovement.magnitude;
			playerMovementMagnitude = playerMovementMagnitude < 0 ? -playerMovementMagnitude
				: playerMovementMagnitude;
			animator.SetFloat(Move, playerMovementMagnitude);
			
			if(context.performed) {
				playerMovement = context.ReadValue<Vector2>();
			}
			if(context.canceled) {
				playerMovement = Vector3.zero;
				animator.SetFloat(Move, 0f);
			}
		}



		public void OnDig(InputAction.CallbackContext context) {
			// In case the animator reference gets lost during assignation
			if (!CanPlayerAct()) { return; }
			
			if(context.performed) {
				animator.SetTrigger(Dig);
			}
		}

		public void OnAttack(InputAction.CallbackContext context)
		{	
			if (!CanPlayerAct()) { return; }
			
			if (context.performed) {
				if (!isGettingAttacked) {
					emptyStrike.Play();
				}
				animator.SetTrigger(Attack);
			}
		}

		public void OnStart(InputAction.CallbackContext context) {
			if (context.performed) {
				if (GameManager.Instance.IsGameFinished()) {
					SceneManager.LoadScene("PlayerControllerTest");
				}
			}
		}

		#endregion

		#region Utility
		/// <summary>
		/// In case the animator reference gets lost during assignation
		/// </summary>
		/// <returns>Animator status</returns>
		private bool IsAnimatorValid() {
			return animator != null && animator.isActiveAndEnabled;
		}

		private bool CanPlayerAct() {
			return IsAnimatorValid() && !isGameFinished;
		}

		public Material GetPlayerMaterial() {
			return playerMeshRender.material;
		}
		

		#endregion
	}
}
