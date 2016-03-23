using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

	public bool triggerDown = false;
	public bool triggerUp = false;
	public bool triggerPressed = false;

	private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

	private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input ((int)trackedObj.index);  }  }
	private SteamVR_TrackedObject trackedObj;

	// Use this for initialization
	void Start () {
	
		trackedObj = GetComponent<SteamVR_TrackedObject> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (controller == null) {
			Debug.Log ("Controller not initialized.");
			return;
		}
	

		triggerDown = controller.GetPressDown (trigger);
		triggerUp = controller.GetPressUp (trigger);
		triggerPressed = controller.GetPress (trigger);

	}
}
