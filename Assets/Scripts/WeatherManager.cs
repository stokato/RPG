using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using MiniJSON;

public class WeatherManager : MonoBehaviour, IGameManager {
    public ManagerStatus status { get; private set; }

    public float cloudValue { get; private set; } 

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Weather manager starting...");

        _network = service; // Сохранение вставленного объекта NetworkService

        //StartCoroutine(_network.GetWeatherXML(OnXMLDataLoaded)); // Начинаем загрузку данных из интернета
        StartCoroutine(_network.GetWeatherJSON(OnXMLDataLoaded));

        status = ManagerStatus.Initializing; 
    }

    public void OnXMLDataLoaded(string data)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data); // Разбиваем код на структуру с возможностью поиска
        XmlNode root = doc.DocumentElement;

        XmlNode node = root.SelectSingleNode("clouds"); // Извлекаем из данных один узел
        string value = node.Attributes["value"].Value;
        cloudValue = XmlConvert.ToInt32(value) / 100f; // Преобразуем значение в число типа float в диапазоне от 0 до 1

        Debug.Log("Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED); // Рассылка сообщения для информирования остальных сценаривев

        status = ManagerStatus.Started;
    }

    public void OnJSONDataLoaded(string data)
    {
        Dictionary<string, object> dict; // Разбираем не созданный нами XML-контейнер а содержимое словаря
        dict = Json.Deserialize(data) as Dictionary<string, object>;

        Dictionary<string, object> clouds = (Dictionary<string, object>)dict["clouds"];
        cloudValue = (long)clouds["all"] / 100f;

        Debug.Log("Value: " + cloudValue);

        Messenger.Broadcast(GameEvent.WEATHER_UPDATED);
        status = ManagerStatus.Started;
    }

    public void LogWeather(string name)
    {
        StartCoroutine(_network.LogWeather(name, cloudValue, OnLogged));
    }

    private void OnLogged(string response)
    {
        Debug.Log(response);
    }
}
