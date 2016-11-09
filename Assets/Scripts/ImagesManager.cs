using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ImagesManager : MonoBehaviour, IGameManager {

    public ManagerStatus status { get; private set; }

    private NetworkService _network;

    private Texture2D _webImage;

    public void Startup(NetworkService service)
    {
        Debug.Log("Images manager starting...");

        _network = service;

        status = ManagerStatus.Started;
    }

    public void GetWebImage(Action<Texture2D> callback)
    {
        if(_webImage == null) // Проеверяем, нет ли уже скачанного изображения
        {
            StartCoroutine(_network.DownLoadImage((Texture2D image) =>
            {
                _webImage = image; // Сохраняем скачанное изображение
                callback(_webImage); // Обратный вызов используется в лямбда-функции а не напрямую
            }));
        }
        else
        {
            callback(_webImage); // При наличии сохраненного изображения сразу активируется оратный вызов (без скачивания)
        }
    }
    
}
