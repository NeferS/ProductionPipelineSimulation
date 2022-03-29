using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SourceProviderOutline : PipelineOutline
{
    [SerializeField] private RectTransform dropdownInput;
    [SerializeField] private RectTransform integerInput;
    [SerializeField] private GameObject[] blockTypes;

    public override void TriggerConfiguration() 
    {
        base.TriggerConfiguration();

        RectTransform _dropdown = Instantiate(dropdownInput, MainSceneManager.Instance.elementPanes.pipelineConfigContent.transform);
        _dropdown.GetComponentInChildren<Text>().text = "Block type:";
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        foreach (GameObject type in blockTypes)
            options.Add(new Dropdown.OptionData(type.name));
        _dropdown.GetComponentInChildren<Dropdown>().AddOptions(options);
        for (int i = 0; i < blockTypes.Length; i++)
            if (blockTypes[i].name.Equals(selfElement.outputTypes[0].name))
            {
                _dropdown.GetComponentInChildren<Dropdown>().value = i;
                break;
            }
        _dropdown.GetComponentInChildren<Dropdown>().onValueChanged.AddListener(delegate
        {
            selfElement.outputTypes[0] = blockTypes[_dropdown.GetComponentInChildren<Dropdown>().value];
        });

        RectTransform _integer = Instantiate(integerInput, MainSceneManager.Instance.elementPanes.pipelineConfigContent.transform);
        _integer.GetComponentInChildren<Text>().text = "Time span (s):";
        _integer.transform.position = _integer.transform.position - new Vector3(0, integerInput.rect.height, 0);
        _integer.GetComponentInChildren<InputField>().text = "" + selfElement.productionTimes[0];
        _integer.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate
        {
            selfElement.productionTimes[0] = int.Parse(_integer.GetComponentInChildren<InputField>().text);
        });
    }

    public override void TriggerInfo()
    {
        base.TriggerInfo();
        RectTransform specific = MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>()
                                    [MainSceneManager.Instance.elementPanes.content.GetComponentsInChildren<RectTransform>().Length - 1];
        RectTransform row = Instantiate(MainSceneManager.Instance.elementPanes.pipelineInfoRow, specific.transform);
        Text[] textBoxes = row.GetComponentsInChildren<Text>();
        textBoxes[0].text = "Block type:";
        textBoxes[1].text = selfElement.outputTypes[0].name;
        row = Instantiate(MainSceneManager.Instance.elementPanes.pipelineInfoRow, specific.transform);
        row.position = row.position - new Vector3(0, MainSceneManager.Instance.elementPanes.pipelineInfoRow.rect.height, 0);
        textBoxes = row.GetComponentsInChildren<Text>();
        textBoxes[0].text = "Time span (s):";
        textBoxes[1].text = "" + selfElement.productionTimes[0];
    }
}
