using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConveyorsController : MonoBehaviour
{
    [SerializeField] private GameObject conveyorConfigPane;
    [SerializeField] private Conveyor[] conveyors;
    private Dropdown dropdown;
    private InputField inputField;
    private int lastSelected = -1;

    void Start()
    {
        dropdown = conveyorConfigPane.GetComponentInChildren<Dropdown>();
        inputField = conveyorConfigPane.GetComponentInChildren<InputField>();
    }

    public void OnActivateSelf()
    {
        HighlightSelected();
    }

    public void OnDeactivateSelf()
    {
        if (lastSelected >= 0)
            conveyors[lastSelected].GetComponentInChildren<Outline>().enabled = false;
    }

    public void HighlightSelected()
    {
        if (lastSelected >= 0)
            conveyors[lastSelected].GetComponentInChildren<Outline>().enabled = false;
        lastSelected = dropdown.value;
        conveyors[lastSelected].GetComponentInChildren<Outline>().enabled = true;
        inputField.text = "" + conveyors[lastSelected].speed;
    }

    public void SetSpeed()
    {
        conveyors[lastSelected].speed = float.Parse(inputField.text);
    }
}
