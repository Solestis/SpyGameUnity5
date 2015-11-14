#pragma strict

private var startTime: float;
private var startPos: Vector2;
private var couldBeSwipe: boolean;
private var touch : Touch;
public var comfortZone: float = 20f;
public var minSwipeDist: float = 20f;
public var maxSwipeTime: float = 3f;
public var hideObject : GameObject;

function PointerPress() {
	print("Pointerpress!");
	if (Input.touchCount > 0) {
		touch = Input.touches[0];
		couldBeSwipe = true;
		startPos = touch.position;
		startTime = Time.time;
	}
}

function PointerUp() {
	print(couldBeSwipe);
    var swipeTime = Time.time - startTime;
    var swipeDist = (touch.position - startPos).magnitude;
   	
    if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) {
        // It's a swiiiiiiiiiiiipe!
        var swipeDirection = Mathf.Sign(touch.position.x - startPos.x);
       
        // Do something here in reaction to the swipe.
        print("IT WORKS!!!");
        hideObject.SetActive(false);
    }
}

//function Update() {
//	if (Input.touchCount > 0) {
//	    touch = Input.touches[0];
//	    switch (touch.phase) {
//	        case TouchPhase.Began:
//				print("Something is touching me!");
//	            couldBeSwipe = true;
//	            startPos = touch.position;
//	            startTime = Time.time;
//	            break;
//	       
//	        case TouchPhase.Moved:
//	            if (Mathf.Abs(touch.position.y - startPos.y) > comfortZone) {
//	            	print("WRONG!");
//	                couldBeSwipe = false;
//	            }
//	            break;
//	       
//	        case TouchPhase.Ended:
//	        	print(couldBeSwipe);
//	            var swipeTime = Time.time - startTime;
//	            var swipeDist = (touch.position - startPos).magnitude;
//	           	
//	            if (couldBeSwipe && (swipeTime < maxSwipeTime) && (swipeDist > minSwipeDist)) {
//	                // It's a swiiiiiiiiiiiipe!
//	                var swipeDirection = Mathf.Sign(touch.position.x - startPos.x);
//	               
//	                // Do something here in reaction to the swipe.
//	                print("IT WORKS!!!");
//	                hideObject.SetActive(false);
//	            }
//	            break;
//	    }
//	}
//}