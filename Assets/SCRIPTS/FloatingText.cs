using Cinemachine;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Transform objectToRotate;

    void LateUpdate()
    {
        if (mainCamera != null)
        
            objectToRotate.rotation = Quaternion.Slerp(objectToRotate.rotation, mainCamera.transform.rotation, 3f * Time.deltaTime);
        
    }
}
