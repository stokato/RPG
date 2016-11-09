using UnityEngine;
using System.Collections;

public class CheckpointTrigger : MonoBehaviour {

    public string identifer;

    private bool _triggered; // Проверяем, сработала ли уже контрольная точка

    void OnTriggerEnter(Collider other)
    {
        if(_triggered) { return; }

        Managers.Weather.LogWeather(identifer); // Отправляем данные
        _triggered = true;
    }
}
