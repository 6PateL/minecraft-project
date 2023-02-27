using UnityEngine;

public class Move : MonoBehaviour
{
   private readonly float _speed = 7f;
   private void Update()
   {
      float horizontal = Input.GetAxis("Mouse X") * _speed * Time.deltaTime;
      float vertical = Input.GetAxis("Mouse Y") * _speed * Time.deltaTime;
      transform.Translate(horizontal,0f,vertical);
   }
}
