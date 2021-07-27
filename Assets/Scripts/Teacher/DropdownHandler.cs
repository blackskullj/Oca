using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    private Dropdown __dropdown;
    // Start is called before the first frame update
    public void Start()
    {
        __dropdown = transform.GetComponent<Dropdown>();
    }
    // Updates options from List
    public void GetOptions()
    {
        __dropdown.ClearOptions();

        foreach (var element in TeacherUI.questionPools)
        {
            __dropdown.options.Add(new Dropdown.OptionData() { text = element });
        }

        DropdownItemSelected(__dropdown);
        __dropdown.onValueChanged.AddListener(delegate { DropdownItemSelected(__dropdown); });
    }

    // Stores item selected in dropdown
    void DropdownItemSelected(Dropdown dropdown)
    {
        int index = dropdown.value;
        TeacherUI.__selected = dropdown.options[index].text;
    }
}
