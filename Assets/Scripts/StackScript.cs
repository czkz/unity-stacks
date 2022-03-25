using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackScript : MonoBehaviour {
    public float maxLength = -1;
    public StackItemScript stackItemPrefab;
    public InputField pushPopValue;
    public FillScript pushPopValueFill;
    public Transform onLandParent;
    [Range(0, 1)]
    public float stackMoveSpeed = 0.0f;
    private Vector3 landOriginalPos;
    private Vector3 landExpectedPos;
    private float itemSizeG;
    public int maxStackSizeTillMove = -1;

    [Header("Pop Top")]
    public Vector2 popTopForceFrom;
    public Vector2 popTopForceTo;
    public Vector2 popTopTorqueFromTo;
    [Header("Pop Bottom")]
    public Vector3 popBottomForce;
    public float popBottomTorque;

    [HideInInspector]
    public List<GameObject> stack = new List<GameObject>();

    public void Push() {
        int val;
        if (int.TryParse(pushPopValue.text, out val)) {
            Push(val, pushPopValueFill.currFill);
            pushPopValueFill.InstantFill(pushPopValueFill.tarFill);
        } else {
            pushPopValue.GetComponent<InputFieldHandlerScript>().FieldIsRequired();
        }
    }

    public void PushRnd() {
        Push(Random.Range(0, MaxValueStaticScript.maxValue), 0);
        pushPopValue.text = string.Empty;
    }

    public void Push(int value, float instantFillValue) {
        if (Length() < maxLength || maxLength < 0) {
            float lExpectedYFromLand = itemSizeG * (stack.Count + 0.5f);
            Vector3 lExpectedPosFromLand = new Vector3(0, lExpectedYFromLand, stack.Count);
            GameObject newInstance = Instantiate(stackItemPrefab.gameObject, transform.position, transform.rotation, transform);
            newInstance.GetComponent<StackItemScript>().Init(lExpectedPosFromLand, value, instantFillValue, onLandParent);
            stack.Add(newInstance);
        }
    }

    public void PopTop() {
        if (stack.Count > 0 && stack[stack.Count - 1].GetComponent<StackItemScript>().isInPlace) {
            Vector3 force;
            float torque;
            float x = Random.Range(popTopForceFrom.x, popTopForceTo.x);
            float y = Random.Range(popTopForceFrom.y, popTopForceTo.y);
            float z = popBottomForce.z;
            force = new Vector3(x, y, z);
            if (x < 0) {
                torque = Random.Range(popTopTorqueFromTo.x, popTopTorqueFromTo.y);
            } else {
                torque = Random.Range(-popTopTorqueFromTo.y, -popTopTorqueFromTo.x);
            }


            stack[stack.Count - 1].GetComponent<StackItemScript>().OnPop(force, torque);
            stack.RemoveAt(stack.Count - 1);
            return;
        } else {
            return;
        }
    }

    public void PopBottom() {
        if (stack.Count > 0 && stack[0].GetComponent<StackItemScript>().isInPlace) {
            //stack[0].GetComponent<Rigidbody>().useGravity = false;
            stack[0].GetComponent<StackItemScript>().OnPop(popBottomForce, popBottomTorque);
            stack.RemoveAt(0);
            foreach (var block in stack) {
                block.GetComponent<StackItemScript>().MoveDown(1);
            }
            return;
        } else {
            return;
        }
    }

    public int Length() {
        return stack.Count;
    }

    public StackItemScript AtTop() {
        if (stack.Count > 0) {
            return stack[stack.Count - 1].GetComponent<StackItemScript>();
        } else {
            return null;
        }
    }

    public StackItemScript At(int i) {
        if (i >= 0 && i < stack.Count) {
            return stack[i].GetComponent<StackItemScript>();
        } else {
            return null;
        }
    }

    void Start() {
        landOriginalPos = onLandParent.localPosition;
        itemSizeG = stackItemPrefab.itemWidth * stackItemPrefab.transform.localScale.y;
    }

    void Update() {
        if (maxStackSizeTillMove > 0) {
            landExpectedPos = landOriginalPos + onLandParent.up * (Mathf.Min(maxStackSizeTillMove - stack.Count, 0) * itemSizeG);
            onLandParent.localPosition = Vector3.Lerp(onLandParent.localPosition, landExpectedPos, stackMoveSpeed * Time.deltaTime * 60);
        }
    }
}
