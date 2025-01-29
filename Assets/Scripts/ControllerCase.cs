using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Выполняет основную логику внутри сектора с кейсами 
/// </summary>
public class ControllerCase : MonoBehaviour
{
    /// <summary>
    /// Позволяет обратится к текущему последнему сектору и соответственно к экземпляру класса ControllerCase
    /// </summary>
    public static ControllerCase Singleton { get; private set; }

    [HideInInspector] public UnityEvent<OnViewUpdateTotalTimePerDayEvent> OnViewUpdateTotalTimePerDay;
    public class OnViewUpdateTotalTimePerDayEvent
    {
        public TimeSpan timeSpan;
    }

    [SerializeField] private Transform _containerCase;
    [SerializeField] private Transform _caseTemplate;

    public TimeSpan TotalTimePerDay { get; private set; }

    private void Awake()
    {        
        Singleton = this;

        ControllerSector.Singleton.OnAddCase.RemoveAllListeners();

        ControllerSector.Singleton.OnAddCase.AddListener((time) =>
        {
            TimeSpan timeSpan = TimeSpan.Parse(time.CurrentTimeTracking);
            TotalTimePerDay += timeSpan;            

            OnViewUpdateTotalTimePerDay?.Invoke(new OnViewUpdateTotalTimePerDayEvent { timeSpan = TotalTimePerDay });
            
            Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
            caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = $"{timeSpan.Hours} h. {timeSpan.Minutes} min. {timeSpan.Seconds} sec.";

            ControllerSector.Singleton.sectorsViewList[^1].caseViewsList.Add(caseTransform.AddComponent<CaseView>());

            ControllerSector.Singleton.shellDataSectorList.DataSectorList[^1].CaseList.Add(new DataSector.DataCase() 
            {
                TimeTrackingText = time.CurrentTimeTracking,
            });

            JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
        });
    }

    /// <summary>
    /// Спавнит несереализуемый CaseView в последний текущий сектор
    /// </summary>
    /// <param name="timeTrackingTextAsTimeSpan"></param>
    /// <param name="infoText"></param>
    public void SpawnNotSerializationModificationCase(string timeTrackingTextAsTimeSpan, string infoText = "")
    {
        if (TimeSpan.TryParse(timeTrackingTextAsTimeSpan, out TimeSpan timeSpan))
        {
            TotalTimePerDay += timeSpan;

            Transform caseTransform = Instantiate(_caseTemplate, _containerCase);
            caseTransform.GetComponentInChildren<TextMeshProUGUI>().text = $"{timeSpan.Hours} h. {timeSpan.Minutes} min. {timeSpan.Seconds} sec.";

            OnViewUpdateTotalTimePerDay?.Invoke(new OnViewUpdateTotalTimePerDayEvent { timeSpan = TotalTimePerDay });

            CaseView caseView = caseTransform.AddComponent<CaseView>();
            ControllerSector.Singleton.sectorsViewList[^1].caseViewsList.Add(caseView);
            caseView.GetComponentInChildren<TMP_InputField>().text = infoText;
        }
    }

    /// <summary>
    /// Сереализует в JSON переданный в параметры текст CaseView на данном SectorView в данном CaseView
    /// </summary>
    /// <param name="infoText"></param>
    /// <param name="locationSectorView"></param>
    /// <param name="caseForLocationText"></param>
    public void SaveInfoTextInCase(string infoText, SectorView locationSectorView, CaseView caseForLocationText)
    {
        ControllerSector.Singleton.shellDataSectorList
            .DataSectorList[ControllerSector.Singleton.sectorsViewList.IndexOf(locationSectorView)]
            .CaseList[locationSectorView.caseViewsList.IndexOf(caseForLocationText)].InfoText = infoText;

        JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
    }

    /// <summary>
    /// Удаляет из файла JSON в переданные параметры CaseView в SectorView
    /// </summary>
    /// <param name="locationSectorView"></param>
    /// <param name="caseForDelete"></param>
    public void DeleteCase(SectorView locationSectorView, CaseView caseForDelete)
    {
        TotalTimePerDay -= TimeSpan.Parse(ControllerSector.Singleton.shellDataSectorList
            .DataSectorList[ControllerSector.Singleton.sectorsViewList.IndexOf(locationSectorView)]
            .CaseList[locationSectorView.caseViewsList.IndexOf(caseForDelete)].TimeTrackingText);

        OnViewUpdateTotalTimePerDay?.Invoke(new OnViewUpdateTotalTimePerDayEvent() { timeSpan = TotalTimePerDay });

        ControllerSector.Singleton.shellDataSectorList
            .DataSectorList[ControllerSector.Singleton.sectorsViewList.IndexOf(locationSectorView)]
            .CaseList.RemoveAt(locationSectorView.caseViewsList.IndexOf(caseForDelete));

        locationSectorView.caseViewsList.Remove(caseForDelete);

        JsonServiceUtility.SaveData(ControllerSector.Singleton.shellDataSectorList);
    }
}
