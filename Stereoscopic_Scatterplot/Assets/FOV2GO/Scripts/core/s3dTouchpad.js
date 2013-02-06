/* This file is part of Stereoskopix FOV2GO for Unity V2.
 * URL: http://diy.mxrlab.com/ * Please direct any bugs/comments/suggestions to hoberman@usc.edu.
 * Stereoskopix FOV2GO for Unity Copyright (c) 2011-12 Perry Hoberman & MxR Lab. All rights reserved.
 */

/* S3D Touchpad Script - based on Joystick.js
 * rewritten to combine elements of touchpad behavior with optional joystick visual movement, or joystick behavior without visual movement.
 * and to assume one touch per touchpad, so no multiple finger latching
 * and to deal only with single taps, so no tapCount - thus we can use TouchPhase to deal with taps
 * also, we don't need to worry about keeping track of all the touchpads, because they don't interfere with each other
 * if a TouchPhase has never been TouchPhase.Moved when it becomes TouchPhase.Ended (or if it hasn't moved more than tapDistanceLimit), then it's a tap
 * otherwise its a drag/swipe
 * distinguishes between short and long taps with shortTapTimeMax and longTapTimeMax
 */

#pragma strict

@script RequireComponent( GUITexture )

// A simple class for bounding how far the GUITexture will move
class Boundary {
	var min : Vector2 = Vector2.zero;
	var max : Vector2 = Vector2.zero;
}

public var moveLikeJoystick : boolean = true;
public var actLikeJoystick : boolean = false;
public var shortTapTimeMax : float = 0.2;
public var longTapTimeMax : float = 0.5;
public var tapDistanceLimit : float = 10.0;

private var touchZone : Rect;
public var position : Vector2; 									// [-1, 1] in x,y
public var tap : int = 0; // 1 for short tap, 2 for long tap

private var gui : GUITexture;								// Joystick graphic
private var defaultRect : Rect;								// Default position / extents of the joystick graphic
private var guiBoundary : Boundary = Boundary();			// Boundary for joystick graphic
private var guiTouchOffset : Vector2;						// Offset to apply to touch input
private var guiCenter : Vector2;							// Center of joystick

private var thisTouchID : int = -1;
private var thisTouchDownTime : float;
private var thisTouchMoved : boolean = false;

private var fingerDownPos : Vector2;
private var fingerUpPos : Vector2;
private var fingerDownTime : float;

function Start() {
	setUp();
}

function setUp() {
	// Cache this component at startup instead of looking up every frame	
	gui = GetComponent( GUITexture );
	// Store the default rect for the gui, so we can snap back to it
	defaultRect = gui.pixelInset;
    defaultRect.x += transform.position.x * Screen.width;// + gui.pixelInset.x; // -  Screen.width * 0.5;
    defaultRect.y += transform.position.y * Screen.height;// - Screen.height * 0.5;
    transform.position.x = 0.0;
    transform.position.y = 0.0;
  	// If a texture has been assigned, then use the rect from the gui as our touchZone
	if (gui.texture) touchZone = defaultRect;
	// This is an offset for touch input to match with the top left corner of the GUI
	guiTouchOffset.x = defaultRect.width * 0.5;
	guiTouchOffset.y = defaultRect.height * 0.5;
	// Cache the center of the GUI, since it doesn't change
	guiCenter.x = defaultRect.x + guiTouchOffset.x;
	guiCenter.y = defaultRect.y + guiTouchOffset.y;
	// Let's build the GUI boundary, so we can clamp joystick movement
	guiBoundary.min.x = defaultRect.x - guiTouchOffset.x;
	guiBoundary.max.x = defaultRect.x + guiTouchOffset.x;
	guiBoundary.min.y = defaultRect.y - guiTouchOffset.y;
	guiBoundary.max.y = defaultRect.y + guiTouchOffset.y;
	gui.pixelInset = defaultRect;
}

function Update () {
	#if (UNITY_IPHONE || UNITY_ANDROID) && !UNITY_EDITOR
    for (var touch : Touch in Input.touches) {
		var guiTouchPos : Vector2 = touch.position - guiTouchOffset;
		if (touchZone.Contains(touch.position)) {
			if (touch.phase == TouchPhase.Began) {
				thisTouchID = touch.fingerId;
				fingerDownPos = touch.position;
				thisTouchDownTime = Time.time;
				thisTouchMoved = false;
			}
		}
		if (thisTouchID == touch.fingerId) {
			if (touch.phase == TouchPhase.Stationary && !actLikeJoystick) {
				// not moving - but if actLikeJoystick is true, we need to get a position reading on tapdown, even if touch hasn't moved
			} else if (touch.phase == TouchPhase.Moved || actLikeJoystick) {
				thisTouchMoved = true;
				if (!actLikeJoystick) {
					position.x = Mathf.Clamp( ( touch.position.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
					position.y = Mathf.Clamp( ( touch.position.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
				}
				if (moveLikeJoystick) {
					gui.pixelInset.x =  Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
					gui.pixelInset.y =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
				}
				if (actLikeJoystick) {
					var dummyInsetX : float = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
					var dummyInsetY : float =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
					position.x = ( dummyInsetX + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
					position.y = ( dummyInsetY + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
				}
			} else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				fingerUpPos = touch.position;
				var dist : float = Vector2.Distance(fingerDownPos,fingerUpPos);
				if (!thisTouchMoved || dist < tapDistanceLimit) {
					if (Time.time < thisTouchDownTime+shortTapTimeMax) {
						tap = 1;
					} else if (Time.time < thisTouchDownTime + longTapTimeMax) {
						tap = 2;
					}
				}
				thisTouchID = -1;
				position = Vector2.zero;
				if (moveLikeJoystick) {
					gui.pixelInset = defaultRect;
				}
			}
		}
    }
    #endif

	#if UNITY_EDITOR
	var guiTouchPos : Vector2 = Input.mousePosition - guiTouchOffset;
	if (touchZone.Contains(Input.mousePosition)) {
		if (Input.GetMouseButtonDown(0)) {
			thisTouchID = 1;
			fingerDownPos = Input.mousePosition;
			thisTouchDownTime = Time.time;
			thisTouchMoved = false;
		}
	}
	if (thisTouchID == 1) {
		if (!actLikeJoystick) {
			position.x = Mathf.Clamp( ( Input.mousePosition.x - fingerDownPos.x ) / ( touchZone.width / 2 ), -1, 1 );
			position.y = Mathf.Clamp( ( Input.mousePosition.y - fingerDownPos.y ) / ( touchZone.height / 2 ), -1, 1 );
		}
		if (moveLikeJoystick) {
			gui.pixelInset.x =  Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
			gui.pixelInset.y =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
		}
		if (actLikeJoystick) {
			var dummyInsetX : float = Mathf.Clamp( guiTouchPos.x, guiBoundary.min.x, guiBoundary.max.x );
			var dummyInsetY : float =  Mathf.Clamp( guiTouchPos.y, guiBoundary.min.y, guiBoundary.max.y );		
			position.x = ( dummyInsetX + guiTouchOffset.x - guiCenter.x ) / guiTouchOffset.x;
			position.y = ( dummyInsetY + guiTouchOffset.y - guiCenter.y ) / guiTouchOffset.y;
		}

	}
	if (Input.GetMouseButtonUp(0) && thisTouchID == 1) {
		fingerUpPos = Input.mousePosition;
		var dist : float = Vector2.Distance(fingerDownPos,fingerUpPos);
		if (dist < tapDistanceLimit) {
			if (Time.time < thisTouchDownTime+shortTapTimeMax) {
				tap = 1;
			} else if (Time.time < thisTouchDownTime + longTapTimeMax) {
				tap = 2;
			}
		}
		thisTouchID = -1;
		position = Vector2.zero;
		if (moveLikeJoystick) {
			gui.pixelInset = defaultRect;
		}
	}
	#endif
}

/* The client that directly registers the tap is responsible for resetting touchpad. Currently, the following scripts call this function: s3dDeviceManager.js, s3dFirstPersonController.js, s3dGuiTexture.js, triggerObjectButton.js, triggerSceneChange.js. Note that since s3dInteractor.js (called by s3dGuiTexture.js) & all interaction scripts (called by s3dInteractor.js) are not direct clients, they aren't responsible for resetting touchpad */
function reset() {
	tap = 0;
}
