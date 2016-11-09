using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour {
    [SerializeField]
    private string itemName; // Введите имя этого элемента на панели Inspector

    void OnTriggerEnter(Collider other)
    {
        Managers.Inventory.AddItem(itemName);

        Destroy(this.gameObject);
    }
}
