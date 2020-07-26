using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject shootScene;
    public GameObject birdViewCamera;
    private void Start()
    {
        EventManager.AddCameraSwitchtListener(ShootCameraEnable);
        ShootCameraEnable(false);
    }
   public void ShootCameraEnable(bool isShootCameraActive) {

        if (isShootCameraActive) {
            shootScene.SetActive(true);
            birdViewCamera.SetActive(false);
        }
       else
        {
            shootScene.SetActive(false);
            birdViewCamera.SetActive(true);
        }

    }
    
}
