using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный класс не является компонентом игрового объекта по умолчанию
/// </summary>
public class SectorView : MonoBehaviour
{
    [HideInInspector] public List<CaseView> caseViewsList = new();

    private void Awake()
    {
        this.GetComponentInChildren<ControllerCase>().OnViewUpdateTotalTimePerDay.AddListener((time) =>
        {
            this.GetComponentInChildren<TotalTimeView>().TotalTimePerDayText.text = time.timeSpan.ToString();
        });
    }
}