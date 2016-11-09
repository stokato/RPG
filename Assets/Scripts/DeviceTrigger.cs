using UnityEngine;
using System.Collections;

public class DeviceTrigger : MonoBehaviour {
    [SerializeField]
    private GameObject[] targets;

    public bool requireKey;

    // Попадание в зону триггера
    void OnTriggerEnter(Collider other)
    {
        if(requireKey && Managers.Inventory.equippedItem != "key")
        {
            return;
        }

        foreach (GameObject target in targets)
        {
            target.SendMessage("Activate");
        }
    }

    // Выход из зоны триггера
    void OnTriggerExit(Collider other)
    {
        foreach(GameObject target in targets)
        {
            target.SendMessage("Deactivate");
        }
    }
}
