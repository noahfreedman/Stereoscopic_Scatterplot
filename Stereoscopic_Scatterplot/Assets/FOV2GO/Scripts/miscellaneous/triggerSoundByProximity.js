/* Simple script to trigger sound by proximity */

#pragma strict
var player : Transform;
var distance = 5;
private var playerCloseEnough = false;
@script RequireComponent(AudioSource);

function Update(){
    if (Vector3.Distance(transform.position, player.position) < distance) {
        playerCloseEnough = true;
        if (!audio.isPlaying) {
			audio.Play();
        }
   } else {
       playerCloseEnough = false;
       if (audio.isPlaying) {
			audio.Stop();
        }
    }
}