using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldHandlerScript : MonoBehaviour {
    protected InputField inputField;
    public FlashScript redFiller;
    public TextMesh mirrorText;

    public int maxValue = MaxValueStaticScript.maxValue;

    public enum OverflowAction {
        revert,
        clamp,
        replace,
        clear
    }
    public OverflowAction overflowAction;

    protected Dictionary<OverflowAction, Func<int, string>> funcs;

    void Awake() {
        inputField = gameObject.GetComponent<InputField>();
        Debug.Assert(inputField != null, inputField);
    }

    void Start() {
        funcs = new Dictionary<OverflowAction, Func<int, string>>();

        funcs.Add(OverflowAction.revert, (int v) => {
            int newval = v / 10;
            if (newval != 0) { return newval.ToString(); } else { return funcs[OverflowAction.clear](v); }
        });
        funcs.Add(OverflowAction.clamp, (int v) => maxValue != 0 ? maxValue.ToString() : funcs[OverflowAction.revert](v));
        funcs.Add(OverflowAction.replace, (int v) => {
            int newval = v % 10;
            if (newval <= maxValue) { return newval.ToString(); } else { return funcs[OverflowAction.revert](v); }
        });
        funcs.Add(OverflowAction.clear, (int v) => string.Empty);
    }

    public void FieldIsRequired() {
        if (redFiller) {
            redFiller.Flash();
        }
    }

    public virtual void OnValueChanged() {
        int val;
        if (int.TryParse(inputField.text, out val)) {
            string newtext = RemovedZeros();
            if (val > maxValue) {
                newtext = funcs[overflowAction](val);
            }
            inputField.text = newtext;
        }
        if (mirrorText) { mirrorText.text = inputField.text; }
    }

    protected string RemovedZeros() {
        string s = inputField.text;
        while (s.Length > 1 && s[0] == '0') {
            s = s.Remove(0, 1);
        }
        return s;
    }
}
