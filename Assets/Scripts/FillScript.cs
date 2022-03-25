using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillScript : MonoBehaviour {
    public Transform myFill;
    public bool isColorChangable = false;
    public Material myMaterial;
    public float myFillLength = 1.0f;

    [Range(0.0f, 1.0f)]
    public float fillSpeed = 0.1f;
    [Range(0.0f, 1.0f)]
    public float colorSpeed = 0.1f;
    public float maxFillDelta = 0.01f;

    public float tarFill { get; private set; }
    public float currFill { get; private set; }

    public Color tarColor { get; private set; }
    public Color currColor { get; private set; }


    void Start() {
        if (isColorChangable) {
            currColor = myMaterial.GetColor("_Color");
            tarColor = currColor;
        }
    }


    public void SmoothFill(float fillLen) { tarFill = Mathf.Clamp(fillLen, 0, 1); }
    public void InstantFill(float fillLen) { tarFill = currFill = Mathf.Clamp(fillLen, 0, 1); FillToCurr(); }

    public void SmoothColor(Color color) { tarColor = color; }
    public void InstantColor(Color color) { currColor = tarColor = color; }
    public Color GetColor() { return tarColor; }


    private void FillToCurr() {
        Vector3 ls = myFill.localScale;
        Vector3 lp = myFill.localPosition;
        ls.x = currFill;
        lp.x = -0.5f * myFillLength + 0.5f * myFillLength * currFill;
        myFill.localScale = ls;
        myFill.localPosition = lp;
    }

    private void ColorToCurr() { myMaterial.SetColor("_Color", currColor); }

    private void LerpFill() {
        float delta = Mathf.Abs(currFill - tarFill);
        if (delta != 0) {
            if (delta > maxFillDelta) {
                currFill = Mathf.Lerp(currFill, tarFill, fillSpeed * Time.deltaTime * 60);
            } else {
                currFill = tarFill;
            }
            FillToCurr();
        }
    }

    private void LerpColor() {
        if (currColor != tarColor) {
            Color newColor = Color.Lerp(currColor, tarColor, colorSpeed * Time.deltaTime * 60);
            if (colorSpeed != 0 && newColor == currColor) {
                currColor = tarColor;
            } else {
                currColor = newColor;
            }
            ColorToCurr();
        }
    }

    void Update() {
        LerpFill();
        if (isColorChangable) { LerpColor(); }
    }
}
