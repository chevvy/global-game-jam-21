using UnityEngine;
using UnityEngine.InputSystem;
namespace PlayerController {
	public class PlayerController : MonoBehaviour
	{
		#region InputCallBack

		public void OnMove(InputAction.CallbackContext context) {
			if(context.performed) {
				Debug.Log("hello there");
			}
		}

		public void OnDig(InputAction.CallbackContext context) {
			if(context.performed) {
				Debug.Log("Digging baby");
			}
		}
		
		#endregion
	
	}
}
