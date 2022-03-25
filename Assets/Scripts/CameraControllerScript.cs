using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour {
    [Range(0, 1)]
    public float speed = 1;

    public Vector2 mapToBetween = new Vector2(0, 5);


    void Update() {
        Vector3 newPos = Camera.main.transform.position;
        newPos.x = Mathf.Lerp(
            newPos.x,
            Mathf.Lerp(mapToBetween.x, mapToBetween.y, Input.mousePosition.x / Screen.width),
            speed * Time.deltaTime * 60
        );
        Camera.main.transform.position = newPos;
    }



    // List<int> arr;

    // int c = 1337;
    // for (int i = 0; i < 32; i++) {
    //     int p2 = 1 << i;
    //     if ((c & 2) != 0) {
    //         arr.Add(p2);
    //     }
    // }
}
