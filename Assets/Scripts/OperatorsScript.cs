using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperatorsScript : MonoBehaviour {
    public StackScript stack;

    private StackItemScript trackedTop = null;
    private StackItemScript trackedBtm = null;


    enum Operator {
        add, sub, mlt, div, pow, log
    }
    Operator nextOperation;


    public void OnBtnAdd() { OnBtnGeneral(Operator.add); }
    public void OnBtnSub() { OnBtnGeneral(Operator.sub); }
    public void OnBtnMlt() { OnBtnGeneral(Operator.mlt); }
    public void OnBtnDiv() { OnBtnGeneral(Operator.div); }
    public void OnBtnPow() { OnBtnGeneral(Operator.pow); }
    public void OnBtnLog() { OnBtnGeneral(Operator.log); }


    private void OnBtnGeneral(Operator op) {
        if (!trackedTop && stack.Length() >= 2) {
            nextOperation = op;
            trackedTop = stack.AtTop();
            trackedBtm = stack.At(stack.Length() - 2);
            trackedTop.MoveDown(1);
            if (trackedTop.value > trackedBtm.value) {
                Vector3 pos = trackedTop.transform.position;
                pos.z = trackedBtm.transform.position.z - 1;
                trackedTop.transform.position = pos;
            }
        }
    }


    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void LateUpdate() {
        if (trackedTop && trackedTop.isInPlace) {
            stack.PopTop();
            int v1 = trackedTop.value;
            int v2 = trackedBtm.value;
            int res = 0;
            switch (nextOperation) {
                case Operator.add:
                    res = v1 + v2;
                    break;
                case Operator.sub:
                    res = v1 - v2;
                    break;
                case Operator.mlt:
                    res = v1 * v2;
                    break;
                case Operator.div:
                    if (v2 != 0) {
                        res = v1 / v2;
                    } else {
                        while (stack.Length() != 0) { stack.PopTop(); }
                    }
                    break;
                case Operator.pow:
                    res = (int)Mathf.Round(Mathf.Pow((float)v1, (float)v2));
                    break;
                case Operator.log:
                    res = (int)Mathf.Round(Mathf.Log((float)v2, (float)v1));
                    break;
            }
            FillScript fs = trackedBtm.GetComponent<FillScript>();
            if (fs) { fs.InstantFill((float)Mathf.Max(v1, v2) / MaxValueStaticScript.maxValue); }
            trackedBtm.value = res;
            Destroy(trackedTop.gameObject);
            trackedTop = null;
            trackedBtm = null;
        }
    }
}
