using UnityEngine;

public class CameraController : MonoBehaviour
{
    public  Transform player; // Mario's Transform
    public  Transform endLimit; // GameObject that indicates end of map
    private  float _offset; // initial x-offset between camera and Mario
    private  float _startX; // smallest x-coordinate of the Camera
    private  float _endX; // largest x-coordinate of the camera
    private  float _viewportHalfWidth;

    // Start is called before the first frame update
    void Start()
    {
        // get coordinate of the bottomleft of the viewport
        // z doesn't matter since the camera is orthographic
        Vector3 bottomLeft =  Camera.main.ViewportToWorldPoint(new  Vector3(0, 0, 0));
        _viewportHalfWidth  =  Mathf.Abs(bottomLeft.x  -  this.transform.position.x);

        _offset  =  transform.position.x  -  player.position.x;
        _startX  =  transform.position.x;
        _endX  =  endLimit.transform.position.x  -  _viewportHalfWidth;
    }

    // Update is called once per frame
    void Update()
    {
        float desiredX =  player.position.x  +  _offset;
        // check if desiredX is within startX and endX
        if (desiredX  >  _startX  &&  desiredX  <  _endX)
            transform.position  =  new  Vector3(desiredX, transform.position.y, transform.position.z);
    }
}