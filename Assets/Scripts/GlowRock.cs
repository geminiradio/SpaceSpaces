using UnityEngine;
using System.Collections;



public class GlowRock : MonoBehaviour {

	public bool triggered = false;
	public Renderer myRend;
	public Light myLight;

	public float emisLow = 0.08f;
	public float emisHigh = 1f;
	public float lightIntensityLow = 0.01f;
	public float lightIntensityHigh = 1.62f;

	public float brightenDuration = 0.5f;  // stage 1
	public float glowDuration = 5f;   // stage 2
	public float darkenDuration = 2.5f;   // stage 3


	private enum GlowStage : int {
		Inactive,
		Brighten,
		Glowing,
		Darken
	}

	private GlowStage glowStage = GlowStage.Inactive;
	private Interpolator2D interp_emis, interp_light;
	private float glowingStartTime;



	void Start () {
	
		if (myRend == null)
			Debug.LogError(this+" material not assigned!");

		if (myLight == null)
			Debug.LogError(this+" light not assigned!");

		if (glowDuration < 0)
			Debug.LogError(this+" glowDuration not assigned correctly!");

		interp_emis = new Interpolator2D();
		interp_light = new Interpolator2D();

		myRend.material.EnableKeyword("_EmissionColor");
	}


	void Update () {

		if ( (triggered) && (glowStage != GlowStage.Inactive))
			triggered = false;


		Vector2 newLight, newEmis;
		float newColor;

		if ((glowStage == GlowStage.Brighten) || (glowStage == GlowStage.Darken))
		{

			newLight = interp_light.Update();
			newEmis = interp_emis.Update();
			newColor = newEmis.x;

			myRend.material.shader = Shader.Find("Standard");
			myRend.material.SetColor("_EmissionColor",new Color(newColor, newColor, newColor));
			myLight.intensity = newLight.x;

			if (interp_emis.complete && interp_light.complete)
			{
				if (glowStage == GlowStage.Brighten)
				{
					glowStage = GlowStage.Glowing;
					glowingStartTime = Time.time;

					// slam to max values
					myRend.material.shader = Shader.Find("Standard");
					myRend.material.SetColor("_EmissionColor",new Color(emisHigh, emisHigh, emisHigh));
					myLight.intensity = lightIntensityHigh;

				}
				else
				{
					glowStage = GlowStage.Inactive;

					// slam to min values
					myRend.material.shader = Shader.Find("Standard");
					myRend.material.SetColor("_EmissionColor",new Color(emisLow, emisLow, emisLow));
					myLight.intensity = lightIntensityLow;
				}
					
			}

		} // end Brighten or Darken


		else if (glowStage == GlowStage.Glowing)
		{
			if ((Time.time - glowingStartTime) > glowDuration)
			{
				glowStage = GlowStage.Darken;

				interp_emis.Initialize(new Vector2(emisHigh,0f), new Vector2(emisLow,0f), darkenDuration);
				interp_light.Initialize(new Vector2(lightIntensityHigh,0f), new Vector2(lightIntensityLow,0f), darkenDuration);
			}
		}  // end Glowing
			

		else if (glowStage == GlowStage.Inactive)
		{
			if (triggered)
			{
				triggered = false;

				glowStage = GlowStage.Brighten;

				// TODO - implement and use 1D interpolators instead of ignoring the second dimension
				interp_emis.Initialize(new Vector2(emisLow,0f), new Vector2(emisHigh,0f), brightenDuration);
				interp_light.Initialize(new Vector2(lightIntensityLow,0f), new Vector2(lightIntensityHigh,0f), brightenDuration);
			}

		} // end Inactive
		
	}
}
