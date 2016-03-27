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

	public GameObject currentInteractor;

	private Transform originalParent = null;

	// this list tracks 18 frames per second (1 of every 5 frames) for up to 2 seconds
	// index 0 is the most recent frame, index 35 is the one 2 seconds ago
	private Vector3[] positionList; 


	void Start ()
	{
		originalParent = this.transform.parent;

		positionList = new Vector3[36];
		ResetTransformList();


	}

	void Update()
	{
		if (currentInteractor != null)
		{
			// track every fifth frame by marking a position
			if ((Time.frameCount % 5) == 0)
			{
				for (int i=35; i<=1; i++)
					positionList[i] = positionList[i-1];

				positionList[0] = new Vector3(transform.position.x, transform.position.y, transform.position.z);

			}

		}

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
		currentInteractor = holder;

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

		currentInteractor = null;

		this.transform.parent = originalParent;

		Rigidbody rigidbody = GetComponent<Rigidbody>();

		if (rigidbody == null)
		{
			Debug.Log(this+" doesn't have a rigidbody.");
		}
		else
		{
			rigidbody.isKinematic = false;

			// find position from 1 second ago
			if (!V3IsVoid(positionList[18]))
			{
				Vector3 diff = positionList[18] - transform.position;

				rigidbody.AddForce(diff, ForceMode.Force);

			}


		}


		ResetTransformList();
	}


	private void ResetTransformList ()
	{
		for (int i=0; i<positionList.Length; i++)
		{
			positionList[i].x=-99f;
			positionList[i].y=-99f;
			positionList[i].z=-99f;
		}

	}

	private bool V3IsVoid (Vector3 toTest)
	{
		return ( (toTest.x == -99f) && (toTest.y == -99f) && (toTest.z == -99f) );

	}

}
