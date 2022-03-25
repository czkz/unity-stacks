using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConvBtnAnimScript : MonoBehaviour {
    public Text whatText;
    public Text fromText;
    public Transform arriveAt;
    public Button convButton;
    public ConvToDecScript convToDecScript;

    [Range(0, 1)]
    public float moveSpeed = 0.1f;

    private Vector3 btnInitPos;

    private bool isAllGood = false;

    void Start() {
        btnInitPos = convButton.transform.position;
    }

    public void OnButtonPress() {
        const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (isAllGood) {
            int baseN;
            if (int.TryParse(fromText.text, out baseN)) {
                Stack<char> s = new Stack<char>(whatText.text.ToCharArray());
                int val = 0;
                for (int pw = 1; s.Count != 0; pw *= baseN) {
                    char c = s.Pop();
                    val += digits.IndexOf(c) * pw;
                }
                convToDecScript.ToDecimal(whatText.text, baseN);
            }
        }
    }

    void Update() {
        isAllGood = whatText.text.Length > 0 && fromText.text.Length > 0;
        int v;
        if (int.TryParse(fromText.text, out v)) {
            isAllGood = isAllGood && v > 1;
        } else {
            isAllGood = false;
        }
        MoveBtn(isAllGood);
    }

    void MoveBtn(bool enabled) {
        Vector3 p = convButton.transform.position;
        Vector3 newPos;
        if (enabled) {
            newPos = Vector3.Lerp(p, arriveAt.position, moveSpeed * Time.deltaTime * 60);
        } else {
            newPos = Vector3.Lerp(p, btnInitPos, moveSpeed * Time.deltaTime * 60);
        }
        newPos.z = convButton.transform.position.z;
        convButton.transform.position = newPos;

        float t = (newPos - btnInitPos).magnitude / (arriveAt.position - btnInitPos).magnitude;

        Color c2 = convButton.GetComponent<Image>().color;
        c2.a = t;
        convButton.GetComponent<Image>().color = c2;
    }
}
