using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public Transform mario;
    private float leftLimit = -4.3f;
    private float rightLimit = 4.3f;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(mario != null) {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(mario.position.x, leftLimit, rightLimit);
            transform.position = position;
        }
    }
}
