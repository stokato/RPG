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
        _rotY -= Input.GetAxis("Horizontal") * rotSpeed; // Меняем направление на обратное
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        transform.position = target.position - (rotation * _offset);
        transform.LookAt(target);
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
