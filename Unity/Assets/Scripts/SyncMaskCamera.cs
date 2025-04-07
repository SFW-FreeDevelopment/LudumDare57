using UnityEngine;

public class SyncMaskCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera maskCamera;

    void LateUpdate()
    {
        if (mainCamera == null || maskCamera == null) return;

        maskCamera.orthographicSize = mainCamera.orthographicSize;
        maskCamera.transform.position = mainCamera.transform.position;
        maskCamera.aspect = mainCamera.aspect;

        maskCamera.fieldOfView = mainCamera.fieldOfView;
        maskCamera.orthographic = mainCamera.orthographic;
        maskCamera.nearClipPlane = mainCamera.nearClipPlane;
        maskCamera.farClipPlane = mainCamera.farClipPlane;
    }
}
