using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Utilities;
namespace PlayerController {
	public class PlayerControllerManager : MonoBehaviour {
		private ReadOnlyArray<Gamepad> allGamepads;
		private List<Gamepad> assignedGamepads;
		private int numberOfConnectedController => assignedGamepads.Count;

		public int CurrentDeviceID => Gamepad.current.deviceId;

		void Start() {
			AssignConnectedGamepads();
			
		}
		
		public void OnPlayerJoined(PlayerInput playerInput) {
			Debug.Log("Joined: " + playerInput.GetInstanceID());
		}

		public void OnPlayerLeft(PlayerInput playerInput) {
			Debug.Log("removed : " + playerInput.GetInstanceID());
		}
		
		private void AssignConnectedGamepads() {
			allGamepads = Gamepad.all;
			assignedGamepads = new List<Gamepad>();
			foreach(var gamepad in allGamepads) {
				if(assignedGamepads.Count <= 4) {
					assignedGamepads.Add(gamepad);
				}
				if(assignedGamepads.Count == 4) { Debug.Log("Max controller reached! "); }
			}
			Debug.Log("Number of connected controller" + numberOfConnectedController);
		}

		private void AssignGamepad(int deviceID) {
			foreach (var currentGamepad in allGamepads) {
				if (currentGamepad.deviceId == deviceID) {
					assignedGamepads.Add(currentGamepad);
					Debug.Log("assigned gamepad => " + deviceID);
				}
			}
		}

		private void RemoveGamepad(int deviceID) {
			foreach (var assignedGamepad in assignedGamepads) {
				if (assignedGamepad.deviceId == deviceID) {
					assignedGamepads.Remove(assignedGamepad);
					Debug.Log("Removed device -> " + deviceID);
				}
			}
		}

		private void AddPlayerToGame(int deviceID) {
			
		}

		void Update() {
			InputSystem.onDeviceChange +=
				(device, change) => {
					switch(change) {
						case InputDeviceChange.Added:
							AssignGamepad(device.deviceId);
							break;
						case InputDeviceChange.Disconnected:
							RemoveGamepad(device.deviceId);
							break;
						case InputDeviceChange.Reconnected:
							// Plugged back in.
							break;
						case InputDeviceChange.Removed:
							// Remove from Input System entirely; by default, Devices stay in the system once discovered.
							break;
						default:
							// See InputDeviceChange reference for other event types.
							break;
					}
				};
		}
	}
}
