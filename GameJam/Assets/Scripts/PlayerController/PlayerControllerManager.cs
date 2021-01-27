using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
namespace PlayerController {
	public class PlayerControllerManager : MonoBehaviour {
		private ReadOnlyArray<Gamepad> allGamepads;
		private List<Gamepad> assignedGamepads;
		private int numberOfConnectedController => assignedGamepads.Count;
	
		void Start() {
			Debug.Log("Current gamepad = " + Gamepad.current.deviceId);
			Debug.Log("Keyboard" + Keyboard.current.deviceId);
			
			AssignGamepads();
		}
		private void AssignGamepads() {
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

		void Update() {
			InputSystem.onDeviceChange +=
				(device, change) => {
					switch(change) {
						case InputDeviceChange.Added:
							// New Device.
							break;
						case InputDeviceChange.Disconnected:
							// Device got unplugged.
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
