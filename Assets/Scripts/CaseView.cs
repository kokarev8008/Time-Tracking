using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Данный класс не является компонентом игрового объекта по умолчанию
/// </summary>
public class CaseView : MonoBehaviour
{
    private void Start()
    {
        ControllerCase controllerCase = GetComponentInParent<ControllerCase>();
        SectorView sectorView = GetComponentInParent<SectorView>();

        this.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            controllerCase.DeleteCase(sectorView, this);

            Destroy(this.gameObject);
        });

        this.GetComponentInChildren<TMP_InputField>().onValueChanged.AddListener((text) =>
        {
            controllerCase.SaveInfoTextInCase(text, sectorView, this);
        });
    }
}
