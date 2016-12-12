using UnityEngine;
using System.Collections;

public class RamdomLightFlickering : MonoBehaviour {
	public Light lamp;

	[Range(0f,0.5f)]
	public float flickerInterval;

	float time;
	void Update () {

		//Intensity Range

		float randomNum = Random.Range(0.4f,1f);

		// Interval value
		float randomInterval = Random.Range (0f, .2f);
		time += Time.deltaTime;

		if (time > flickerInterval + randomInterval) 
		{
			lamp.intensity = randomNum;
			time = 0;
		}
	}
}
