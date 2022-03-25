using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhatInputScript : InputFieldHandlerScript {
    public InputField baseInputField;

    int baseN = 0;
    const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public void OnBaseChanged() {
        int val;
        if (int.TryParse(baseInputField.text, out val)) {
            baseN = val;
            foreach (char c in inputField.text) {
                if (digits.IndexOf(c) >= baseN) {
                    inputField.text = string.Empty;
                    break;
                }
            }
        } else {
            baseN = 0;
            inputField.text = string.Empty;
        }
    }

    public override void OnValueChanged() {
        if (baseN <= 1 && inputField.text.Length != 0) {
            inputField.text = string.Empty;
            baseInputField.GetComponent<FlashScript>().Flash();
            return;
        }
        string newtext = RemovedZeros();
        if (newtext.Length != 0) {
            newtext = newtext.ToUpper();
            int lasti = newtext.Length - 1;
            int iof = digits.IndexOf(newtext[lasti]);
            if (iof >= baseN || iof == -1) {
                newtext = newtext.Remove(lasti);
            }
        }
        inputField.text = newtext;
        if (mirrorText) { mirrorText.text = inputField.text; }
    }
}
