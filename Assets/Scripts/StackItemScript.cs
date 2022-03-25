using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackItemScript : MonoBehaviour {
    public TextMesh myText;
    public float itemWidth = 1;

    [Range(0, 1)]
    public float moveSpeed = 0.1f;
    public float minMove = 0.005f;

    private Vector3 lPosFromLand;
    private Transform onLandParent;
    private int itemValue;
    public int value {
        get {
            return itemValue;
        }
        set {
            int n = (MaxValueStaticScript.maxValue + 1);
            itemValue = (n + value % n) % n;
            if (myText) {
                myText.text = ((int)itemValue).ToString();
            }
            if (fs) { fs.SmoothFill((float)itemValue / MaxValueStaticScript.maxValue); }
        }
    }
    private FillScript fs;

    public enum State { Landing, Landed, Dying };
    public State state { get; private set; }

    public bool isInPlace {
        get {
            return state == State.Landed;
        }
    }

    public void Init(Vector3 localExpectedPosFromLand, int val, float instantFillValue, Transform onLandParent) {
        lPosFromLand = localExpectedPosFromLand;
        this.onLandParent = onLandParent;
        transform.position = new Vector3(transform.position.x, transform.position.y, lPosFromLand.z);
        fs = gameObject.GetComponent<FillScript>();
        state = State.Landing;
        if (fs) { fs.InstantFill(instantFillValue); }
        value = val;
        transform.parent = onLandParent; //Not supposed to be here
        gameObject.SetActive(true);
    }

    public void OnPop(Vector3 force, float torque) {
        transform.SetParent(null, true);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        rb.constraints =
            RigidbodyConstraints.FreezeRotationX |
            RigidbodyConstraints.FreezeRotationY;
        rb.AddForce(force);
        rb.AddTorque(new Vector3(0, 0, torque));

        Destroy(gameObject, 10.0f);
        state = State.Dying;
    }

    public bool MoveDown(uint byN) {
        if (byN != 0 && state != State.Dying) {
            lPosFromLand.y -= itemWidth * transform.lossyScale.y;
            state = State.Landing;
            return true;
        }
        return false;
    }

    // Start is called before the first frame update
    void Awake() {
        // Deactivate the object until Init() is called
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (state == State.Landing) {
            Vector2 gTarget2D = onLandParent.TransformPoint(lPosFromLand);
            Vector2 currPos2D = transform.position;
            float z = transform.position.z;

            Vector2 a2D = Vector2.Lerp(transform.position, gTarget2D, moveSpeed * Time.deltaTime * 60);
            Vector2 b2D = Vector2.MoveTowards(transform.position, gTarget2D, minMove * Time.deltaTime * 60);
            Vector3 a3D = new Vector3(a2D.x, a2D.y, z);
            Vector3 b3D = new Vector3(b2D.x, b2D.y, z);
            transform.position = Vector2.Distance(a2D, transform.position) > Vector2.Distance(b2D, transform.position) ? a3D : b3D;

            if (currPos2D == gTarget2D) {
                state = State.Landed;
                transform.position = onLandParent.TransformPoint(lPosFromLand);
                transform.parent = onLandParent;
                return;
            }
        }
    }
}
