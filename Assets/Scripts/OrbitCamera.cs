using UnityEngine;
using System.Collections;

public class OrbitCamera : MonoBehaviour {
    [SerializeField]
    private Transform target; // Объект, вокгруг которго камера производит облет

    public float rotSpeed = 1.5f;

    private float _rotY;
    private Vector3 _offset;

    void Start()
    {
        _rotY = transform.eulerAngles.y;
        _offset = target.position - transform.position; // Запоминаем начальне смещение
    }

    void LateUpdate()
    {
        float horInput = Input.GetAxis("Horizontal");

        if(horInput != 0) // Медленный поворот камеры при помощи клавишь со стрелками
        {
            _rotY += horInput * rotSpeed;
        }
        else
        {
            _rotY += Input.GetAxis("Mouse X") * rotSpeed * 3; // или быстрый с помощью мыши
        }

        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position - (rotation * _offset); // Поддерживаем начальное смещение, сдвигаемое вместе с поворотом
        transform.LookAt(target); // Камера всегда направлена на цель
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
