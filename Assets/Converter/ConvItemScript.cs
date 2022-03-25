using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvItemScript : MonoBehaviour {
    public TextMesh myText;
    public float itemWidth = 1;

    [Range(0, 1)]
    public float moveSpeed = 0.1f;
    public float minMove = 0.005f;

    private Vector3 targetPos;
    private Quaternion targetRot;
    public string value {
        get {
            return myText.text;
        }
        set {
            myText.text = value;
        }
    }
    private FillScript fs;

    public enum State { Landing, Landed, Static };
    public State state { get; private set; }

    public bool isInPlace {
        get {
            return state == State.Landed;
        }
    }

    public void Init(Vector3 tarPos, Quaternion tarRot, string val) {
        targetPos = tarPos;
        targetRot = tarRot;
        transform.position = new Vector3(transform.position.x, transform.position.y, targetPos.z);
        state = State.Landing;
        value = val;
        gameObject.SetActive(true);
    }

    void Awake() {
        // Deactivate the object until Init() is called
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (state == State.Landing) {
            float z = transform.position.z;

            Vector2 a2D = Vector2.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime * 60);
            Vector2 b2D = Vector2.MoveTowards(transform.position, targetPos, minMove * Time.deltaTime * 60);
            Vector3 a3D = new Vector3(a2D.x, a2D.y, z);
            Vector3 b3D = new Vector3(b2D.x, b2D.y, z);
            transform.position = Vector2.Distance(a2D, transform.position) > Vector2.Distance(b2D, transform.position) ? a3D : b3D;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, moveSpeed * Time.deltaTime * 60);

            if ((Vector2)transform.position == (Vector2)targetPos) {
                state = State.Landed;
                return;
            }
        } else if (state == State.Landed) {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, moveSpeed * Time.deltaTime * 60);
            if (transform.rotation == targetRot) {
                state = State.Static;
            }
        }
    }
}
