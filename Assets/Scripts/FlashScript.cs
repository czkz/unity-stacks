using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashScript : MonoBehaviour {
    private Color initialColor;
    public Color flashColor = Color.red;
    [Range(0, 1)]
    public float lerpSpeed = 0.1f;

    private Image image;

    void Start() {
        image = GetComponent<Image>();
        initialColor = image.color;
    }

    public void Flash() {
        image.color = flashColor;
    }


    void Update() {
        image.color = Color.Lerp(image.color, initialColor, lerpSpeed * Time.deltaTime * 60);
    }
}
