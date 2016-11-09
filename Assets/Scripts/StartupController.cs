using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartupController : MonoBehaviour {
    [SerializeField]
    private Slider progressBar;

    void Awake()
    {
        Messenger<int, int>.AddListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.AddListener(StartupEvent.MANAGERS_STARTED, OnManagersStated);
    }

    void OnDestroy()
    {
        Messenger<int, int>.RemoveListener(StartupEvent.MANAGERS_PROGRESS, OnManagersProgress);
        Messenger.RemoveListener(StartupEvent.MANAGERS_STARTED, OnManagersStated);
    }

	private void OnManagersProgress(int numReady, int numModules)
    {
        float progress = (float)numReady / numModules;
        progressBar.value = progress; // Обновляем ползунок данными о процессе загрузки
    }

    private void OnManagersStated()
    {
        Managers.Mission.GoToNext(); // После загрузки диспетчера загружаем следующую сцену
    }
}
