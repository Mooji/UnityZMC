using UnityEngine;

public class CamearFollower : MonoBehaviour
{
    public Transform targetTransfrom;   
    public float smoothTime = 1f;  
    private Vector3 cameraVelocity = Vector3.zero ;     
    private Vector3 offSet;
    public void Start()
    {
        offSet = this.transform.position - targetTransfrom.position;
    }
    public void LateUpdate()
    {
       transform.position = Vector3.Lerp( this.transform.position , targetTransfrom.position + offSet , Time.deltaTime );
      // transform.position = Vector3.SmoothDamp( this.transform.position , targetTransfrom.transform.position + offSet , ref cameraVelocity , smoothTime , Mathf.Infinity , Time.deltaTime );
    }



}
