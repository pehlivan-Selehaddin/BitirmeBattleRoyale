using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour, IDragHandler
{
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera aimCamera;

    private float sensivity = 20;
    [HideInInspector] public float degisimY = 0;
    [HideInInspector] public float degisimX = 0;

    public void OnDrag(PointerEventData eventData)
    {
        degisimX += eventData.delta.x * Time.deltaTime * sensivity;
        degisimY -= eventData.delta.y * Time.deltaTime * sensivity;

        degisimY = Mathf.Clamp(degisimY, -7.5f, 50f);

        followCamera.transform.eulerAngles = new Vector2(degisimY, degisimX);
        aimCamera.transform.eulerAngles = new Vector2(degisimY, degisimX);
    }

}
