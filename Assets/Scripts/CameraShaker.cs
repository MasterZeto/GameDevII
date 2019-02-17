using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraShaker : MonoBehaviour
{
	//tutorial by Alexander Zotov
	Vector3 cameraInitialPosition;
	public float shakePower = 0.05f, shakeTime = 0.3f;
	public Camera mainCamera;

	public void Shake()
	{
		cameraInitialPosition = mainCamera.transform.position;
		InvokeRepeating ("StartCameraShake", 0f, 0.005f);
		Invoke ("StopCameraShake", shakeTime);
	}

	void StartCameraShake()
	{
		float camkeraShakeOffsetX = Random.value * shakePower * 2 - shakePower;
		float camkeraShakeOffsetY = Random.value * shakePower * 2 - shakePower;
		Vector3 cameraIntermediatePosition = mainCamera.transform.position;
		cameraIntermediatePosition.x += camkeraShakeOffsetX;
		cameraIntermediatePosition.y += camkeraShakeOffsetY;
		mainCamera.transform.position = cameraIntermediatePosition;

	}

	void StopCameraShake()
	{
		CancelInvoke ("StartCameraShake");
		mainCamera.transform.position = cameraInitialPosition;
	}
}
