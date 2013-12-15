// Orbit to point at a given target GameObject transform
// rotate when mouse button pressed
// zoom in/out with mouse wheel
// - Eric Grosser.
var target : Transform;
var distance = 1.0;
var xSpeed = 250.0;
var ySpeed = 120.0;
var yMinLimit = -20;
var yMaxLimit = 80;
private var x = 0.0;
private var y = 0.0;
public var mouseButtonRotate = 0;
public var shiftToAccelerate = true;
private var currentScrollSpeed = 1.0f;
public var  scrollSpeed = 10.0f;
public var  scrollSpeedFast = 100.0f;
public var  cameraDistanceMax = 1000f;
public var  cameraDistanceMin = 0.7f;
private var originalPosition;
@script AddComponentMenu("Camera-Control/Mouse Orbit")
function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
	currentScrollSpeed = scrollSpeed;
   	if (rigidbody)
		rigidbody.freezeRotation = true;
}
function LateUpdate () {
     if (Input.GetMouseButtonDown(mouseButtonRotate)) {
     			// on click, don't ignore the cameras position and just pop to the rotate position
     			// gotta lerp and make a good transition.
            	distance = Vector3.Distance(target.position, transform.position);
            	//originalPosition = transform.position;
            	//transform.LookAt(target.position);
            	
            }
	if (target && Input.GetMouseButton(mouseButtonRotate)) {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
        var rotation = Quaternion.Euler(y, x, 0);
        position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
        transform.rotation = rotation;
        transform.position = position;
    }
    if (shiftToAccelerate) {
    	if (Input.GetKey(KeyCode.LeftShift)) {
    		currentScrollSpeed = scrollSpeedFast;
    	}
    	else
    	{
    		currentScrollSpeed = scrollSpeed;
    	}
    }
	if (Input.GetAxis("Mouse ScrollWheel") != 0) {
  		distance -= Input.GetAxis("Mouse ScrollWheel") * currentScrollSpeed;
		distance = Mathf.Clamp(distance, cameraDistanceMin, cameraDistanceMax);
        rotation = Quaternion.Euler(y, x, 0);
        position = rotation * Vector3(0.0, 0.0, -distance) + target.position;
        transform.position = position;
		// mouse scrolling zoom conflicts with DemoOrbitCamera 
		//if (MainCamera.GetComponent<DemoOrbitCamera>()) {
		//	MainCamera.GetComponent<DemoOrbitCamera>.Pause;
		//}

		
	}
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}