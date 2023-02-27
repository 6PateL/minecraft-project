using System;
using UnityEngine;

public class RotateByMouse : MonoBehaviour
{
   private float _xRotation = 0f;
   private float _sentivity = 300f;
   [SerializeField] private Transform Player;

   private void Update()
   {
      float MouseX = Input.GetAxis("Mouse X") * _sentivity * Time.deltaTime;
      float MouseY = Input.GetAxis("Mouse Y") * _sentivity * Time.deltaTime;

      _xRotation -= MouseY;
      _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
      transform.localRotation = Quaternion.Euler(_xRotation,0f,0f);
      Player.Rotate(Vector3.up * MouseX);
   }
}
