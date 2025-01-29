using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// ¬ыполн€ет основную логику с секторами
/// </summary>
public class ControllerSector : MonoBehaviour
{
    private const string VALUE_SCROLLBAR_PLAYER_PREFS = "ValueScrollbar";

    public static ControllerSector Singleton { get; private set; }

    [HideInInspector] public UnityEvent<OnAddCaseAction> OnAddCase;
    public class OnAddCaseAction
    {
        public string CurrentTimeTracking;
    }

    [SerializeField] private Transform _containerSectors;
    [SerializeField] private Transform _sectorTemplate;

    [SerializeField] private Scrollbar _scrollbarControllerContainerSector;

    [HideInInspector] public ShellListDataSector shellDataSectorList;
    [HideInInspector] public List<SectorView> sectorsViewList = new();

    private List<DateTime> dateTimesList = new();

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;

        shellDataSectorList = JsonServiceUtility.LoadData<ShellListDataSector>() ?? new ShellListDataSector();   

        TimeTrackingUI.Singleton.OnCheckSector.AddListener(TimeTrackingUI_OnAddSector);

        _scrollbarControllerContainerSector.onValueChanged.AddListener((value) =>
        {
            PlayerPrefs.SetFloat(VALUE_SCROLLBAR_PLAYER_PREFS, value);
            PlayerPrefs.Save();
        });
    }

    private void Start()
    {
        LoadData();
    }

    private void TimeTrackingUI_OnAddSector(TimeTrackingUI.OnCheckSectorAction time)
    {
        if (dateTimesList.Contains(DateTime.Today.Date))
        {
            OnAddCase?.Invoke(new OnAddCaseAction
            {
                CurrentTimeTracking = time.CurrentTimeStr,
            });
        }
        else
        {
            dateTimesList.Add(DateTime.Today.Date);

            Transform sectorTransform = Instantiate(_sectorTemplate, _containerSectors);
            string textDateTime = sectorTransform.GetComponentInChildren<TextMeshProUGUI>().text = DateTime.Now.ToString("d");

            sectorsViewList.Add(sectorTransform.AddComponent<SectorView>());

            shellDataSectorList.DataSectorList.Add(new DataSector()
            {
                DateTimeText = textDateTime,
            });
            
            OnAddCase?.Invoke(new OnAddCaseAction
            {
                CurrentTimeTracking = time.CurrentTimeStr,
            });
        }
    }

    private void LoadData()
    {
        int index = 0;
        foreach (DataSector sector in shellDataSectorList.DataSectorList)
        {
            Transform sectorTransform = Instantiate(_sectorTemplate, _containerSectors);
            sectorTransform.GetComponentInChildren<TextMeshProUGUI>().text = sector.DateTimeText;

            dateTimesList.Add(DateTime.Parse(sector.DateTimeText));
            sectorsViewList.Add(sectorTransform.AddComponent<SectorView>());

            foreach (DataSector.DataCase cases in sector.CaseList)            
                ControllerCase.Singleton.SpawnNotSerializationModificationCase(cases.TimeTrackingText, cases.InfoText);
            
            index++;
        }

        //ѕо непон€тной причине нормализованное значение превышает границу 
        //ƒл€ решени€ € ничего лучше не придумал чем просто присвоить value значение близкое к границе 
        if (PlayerPrefs.GetFloat(VALUE_SCROLLBAR_PLAYER_PREFS) == 1)
            _scrollbarControllerContainerSector.value = 0.95f;
        else
            _scrollbarControllerContainerSector.value = PlayerPrefs.GetFloat(VALUE_SCROLLBAR_PLAYER_PREFS, 0);

    }
}

/// <summary>
/// —ериализуемый класс-оболочка 
/// </summary>
[Serializable]
public class ShellListDataSector
{
    public List<DataSector> DataSectorList = new();
}

/// <summary>
/// —ериализуемый класс дл€ хранени€ данных о секторе и о его кейсах
/// </summary>
[Serializable]
public class DataSector
{
    public string DateTimeText;

    public List<DataCase> CaseList = new();

    [Serializable]
    public class DataCase
    {
        public string TimeTrackingText;

        public string InfoText;
    }
}