using UnityEngine;
using System.Collections;

// what inputs does this object respond to
// TODO this should totes be a bitfield
public enum VRInteractedBy : int {
	WandTrigger
}

// what response does this object have to being interacted with
public enum VRInteractResponse : int {
	PickUp,
	Trigger
}



public class VRInteractable : MonoBehaviour {

	public VRInteractedBy interactInput;
	public VRInteractResponse interactResponse;

	private Transform originalParent = null;

	void Start ()
	{
		originalParent = this.transform.parent;

	}


	public void InteractedWith (bool triggerDown, GameObject interactor)
	{
		Debug.Log("I was totally interacted with by "+interactor+".");

		if (interactResponse == VRInteractResponse.PickUp)
		{
			if (triggerDown)
				GetPickedUp(interactor);
			else
				GetDropped(interactor);
		}
	}


	private void GetPickedUp (GameObject holder)
	{
		this.transform.parent = holder.transform;

		Rigidbody rigidbody = GetComponent<Rigidbody>();

		if (rigidbody == null)
		{
			Debug.Log(this+" doesn't have a rigidbody.");
		}
		else
		{
			rigidbody.isKinematic = true;
		}

	}

	private void GetDropped (GameObject holder)
	{
		this.transform.parent = originalParent;

		Rigidbody rigidbody = GetComponent<Rigidbody>();

		if (rigidbody == null)
		{
			Debug.Log(this+" doesn't have a rigidbody.");
		}
		else
		{
			rigidbody.isKinematic = false;
		}


	}

}
