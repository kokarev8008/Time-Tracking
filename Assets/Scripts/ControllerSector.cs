using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ControllerSector : MonoBehaviour
{
    public static ControllerSector Instance { get; private set; }

    [HideInInspector] public UnityEvent<OnAddCaseAction> OnAddCase;
    public class OnAddCaseAction
    {
        public string CurrentTimeTracking;
    }

    [SerializeField] private Transform _containerSectors;
    [SerializeField] private Transform _sectorTemplate;

    [HideInInspector] public ShellListDataSector shellDataSectorList;
    [HideInInspector] public List<SectorView> sectorsViewList = new();

    private List<DateTime> dateTimesList = new();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        shellDataSectorList = JsonServiceUtility.LoadData<ShellListDataSector>() ?? new ShellListDataSector();   

        TimeTrackingUI.Instance.OnCheckSector.AddListener(TimeTrackingUI_OnAddSector);
    }

    private void Start()
    {
        LoadData();
    }

    private void TimeTrackingUI_OnAddSector(TimeTrackingUI.OnCheckSectorAction obj)
    {
        if (dateTimesList.Contains(DateTime.Today.Date))
        {
            OnAddCase?.Invoke(new OnAddCaseAction
            {
                CurrentTimeTracking = obj.CurrentTimeTracking,
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
                CurrentTimeTracking = obj.CurrentTimeTracking,
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
                ControllerCase.Instance.SpawnModificationCase(cases.TimeTrackingText);
            
            index++;
        }
    }
}

[Serializable]
public class ShellListDataSector
{
    public List<DataSector> DataSectorList = new();
}

[Serializable]
public class DataSector
{
    public string DateTimeText;

    public List<DataCase> CaseList = new();

    [Serializable]
    public class DataCase
    {
        public string TimeTrackingText;
    }
}