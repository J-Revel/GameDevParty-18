using Cinemachine;
using UnityEngine;

/**
 * Force the Cinemachine camera to make viewport exactly as wide as the bounding object.
 * This allows us to always be 100% wide regardless of device. Handy for vertical scrollers.
 */
public class CameraSizeSetter : MonoBehaviour {
	public int fullWidthUnits = 14;

	void Update () {
		// Force fixed width
		float ratio = (float)Screen.height / (float)Screen.width;
		GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = (float)fullWidthUnits * ratio / 2.0f;
	}
}