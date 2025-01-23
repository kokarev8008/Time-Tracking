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

    [SerializeField] private Button _pauseButton;
    [SerializeField] private Sprite _pauseSprite;
    [SerializeField] private Sprite _playSprite;

    private float timerSec;
    private float timerMin;
    private float timerHour;

    private bool _isStarting = false;
    private bool _isPlay = true;

    private void Awake()
    {
        if(Instance == null) 
            Instance = this;

        _startEndTimeTrackingButton.onClick.AddListener(() =>
        {
            _isStarting = !_isStarting;

            if(_isStarting && _isPlay)
            {
                _pauseButton.gameObject.SetActive(true);

                _startEndTimeTrackingText.text = "Stop";
            }
            else
            {
                _pauseButton.gameObject.SetActive(false);

                _isStarting = false;
                _isPlay = true;

                _pauseButton.GetComponent<Image>().sprite = _pauseSprite;

                _startEndTimeTrackingText.text = "Start";

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

        _pauseButton.onClick.AddListener(() =>
        {
            _isPlay = !_isPlay;

            if(!_isPlay)
            {
                _pauseButton.GetComponent<Image>().sprite = _playSprite;
                _isStarting = false;
            }
            else
            {
                _pauseButton.GetComponent<Image>().sprite = _pauseSprite;
                _isStarting = true;
            }
        });

        _pauseButton.gameObject.SetActive(false);

        _currentDateText.text = $"{DateTime.Today:d}\n{DateTime.Today.DayOfWeek}";
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

            _currentDateText.text = $"{DateTime.Today:d}\n{DateTime.Today.DayOfWeek}";
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
