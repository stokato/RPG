using UnityEngine;
using System.Collections;

public class WeatherController : MonoBehaviour {

    [SerializeField]
    private Material sky;
    [SerializeField]
    private Light sun;

    private float _fullIntensity;


    void Awake() // Добавляем/удаляем подписчиков на событие
    {
        Messenger.AddListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.WEATHER_UPDATED, OnWeatherUpdated);
    }

	// Use this for initialization
	void Start () {
        _fullIntensity = sun.intensity; // Исходная интенсивность считается полной
	}
	
    private void OnWeatherUpdated()
    {
        SetOvercast(Managers.Weather.cloudValue); // Используем значени облачности из сценария WeatherManager
    }

    private void SetOvercast(float value) // Корректируем значение Blend и интенсивность света
    {
        sky.SetFloat("_Blend", value);
        sun.intensity = _fullIntensity - (_fullIntensity * value);
    }
}
