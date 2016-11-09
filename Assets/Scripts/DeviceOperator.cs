using UnityEngine;
using System.Collections;

public class DeviceOperator : MonoBehaviour {

    public float radius = 1.5f; // С какого расстояния можно активировать устройство

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetButtonDown("Fire3")) // Реакция на кновку ввода
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius); // Получаем список ближайших объектов
            foreach (Collider hitCollider in hitColliders)
            {
                Vector3 direction = hitCollider.transform.position - transform.position;
                if(Vector3.Dot(transform.forward, direction) > 0.5f) // Определяем, лицом ли повернут персонаж к объекту
                {
                    hitCollider.SendMessage("Operate", SendMessageOptions.DontRequireReceiver); // Взывает именованную фунцию независимо от типа целевого объекта
                }               
            }
        }
	}
}
