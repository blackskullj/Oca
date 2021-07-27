using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderValues : MonoBehaviour
{
    TextMeshProUGUI _sliderValue;
    void Start()
    {
        _sliderValue = GetComponent<TextMeshProUGUI>();
    }

    public void TextUpdate(float value)
    {
        _sliderValue.text = value.ToString();
    }
}
