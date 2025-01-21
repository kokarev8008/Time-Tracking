using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TimeTrackingUI : MonoBehaviour
{
    public static TimeTrackingUI Instance { get; private set; }

    [HideInInspector] public UnityEvent<OnCheckSectorAction> OnCheckSector;
    public class OnCheckSectorAction
    {
        public string CurrentTimeTracking;
    }

    [SerializeField] private Button _startEndTimeTrackingButton;
    [SerializeField] private TextMeshProUGUI _startEndTimeTrackingText;

    [SerializeField] private Button _quitAplicationButton;

    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _currentDateText;

    private float timerSec;
    private float timerMin;
    private float timerHour;

    private bool _isStarting = false;

    private void Awake()
    {
        if(Instance == null) 
            Instance = this;

        _startEndTimeTrackingButton.onClick.AddListener(() =>
        {
            _isStarting = !_isStarting;

            if(_isStarting)
            {
                _startEndTimeTrackingText.text = "Stop";
            }
            else
            {
                _startEndTimeTrackingText.text = "Start Tracking";

                OnCheckSector?.Invoke(new OnCheckSectorAction
                {
                    CurrentTimeTracking = _timeText.text
                });

                timerSec = 0;
                timerMin = 0;
                timerHour = 0;
            }
        });

        _quitAplicationButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        _currentDateText.text = $"{DateTime.Today.ToString("d")}\n{DateTime.Today.DayOfWeek}";
    }

    private void Start()
    {
        StartCoroutine(AutoUpdateDate());
    }

    private IEnumerator AutoUpdateDate()
    {
        while(true)
        {
            yield return new WaitForSeconds(30f);

            _currentDateText.text = $"{DateTime.Today.ToString("d")}\n{DateTime.Today.DayOfWeek}";
        }
    }

    private void Update()
    {
        if (_isStarting)
        {
            if(timerSec > 59)
            {
                timerMin++;
                timerSec = 0;
            }
            if(timerMin > 59)
            {
                timerHour++;
                timerMin = 0;
            }
            if (timerHour > 23)
                timerHour = 0;

            _timeText.text = $"{timerHour} h. {timerMin} min. {Mathf.Round(timerSec += 1 * Time.deltaTime)} sec.";
        }
    }
}
