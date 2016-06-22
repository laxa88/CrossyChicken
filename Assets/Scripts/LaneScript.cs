using UnityEngine;
using System.Collections;

public class LaneScript : MonoBehaviour {

	// You can add as many lane types as you want here,
	// then link it from Unity's inspector
	public GameObject grassLane;
	public GameObject roadLane;
	public GameObject riverLane;

	public void setLaneType (string roadType)
	{
		// Hide all the lanes before showing the new lane type
		hideAllLanes();

		switch (roadType)
		{
		case "grass": grassLane.SetActive(true); break;
		case "road": roadLane.SetActive(true); break;
		case "river": riverLane.SetActive(true); break;
		}
	}

	public void hideAllLanes ()
	{
		grassLane.SetActive(false);
		roadLane.SetActive(false);
		riverLane.SetActive(false);
	}
}
