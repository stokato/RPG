using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour, IGameManager
{
    private Dictionary<string, int> _items;
    private NetworkService _network;

    public ManagerStatus status { get; private set; }

    public string equippedItem { get; private set;  }

    public void Startup(NetworkService service)
    {
        Debug.Log("Inventory manager starting..."); // Сюда идут все задачи запуска с долгим временем выплнения

        _network = service;

        _items = new Dictionary<string, int>(); // Инициализируем пустой список элементов.

        status = ManagerStatus.Started;
    }

    private void DisplayItems() // Вывод на консоль сообщения о текущем инвентаре
    {
        string itemDisplay = "Items: ";

        foreach(KeyValuePair<string, int> item in _items)
        {
            itemDisplay += item.Key + "(" + item.Value + ")";
        }

        Debug.Log(itemDisplay);
    }

    public void AddItem(string name) // Другие сценарии не могут напрямую управлять списком элементов, но могут вызывать этот метод
    {
        if(_items.ContainsKey(name))
        {
            _items[name] += 1;
        } 
        else
        {
            _items[name] = 1;
        }

        DisplayItems();
    }

    public List<string> GetItemList()
    {
        List<string> list = new List<string>(_items.Keys);

        return list;
    }

    public int GetItemCount(string name)
    {
        if(_items.ContainsKey(name))
        {
            return _items[name];
        }
        return 0;
    }

    public bool EquipItem(string name)
    {
        if(_items.ContainsKey(name) && equippedItem != name)  // Проверяем наличие в инвентаре указанного элемента и тот факт
        {                                                     // что он еще не подготовлен к использованию
            equippedItem = name;
            Debug.Log("Equipped " + name);

            return true;
        }

        equippedItem = null;
        Debug.Log("Unequipped");

        return false;
    }

    public bool ConsumeItem(string name)
    {
        // Проверка наличия элемента среди интвентаря
        if(_items.ContainsKey(name))
        {
            _items[name]--;
            if(_items[name] == 0) // Удаление записи, если количество становится равным 0
            {
                _items.Remove(name);
            }
            else // Реакция в случае отсутствия в интвентаре нужного элемента
            {
                Debug.Log("cannot consume " + name);

                return false;
            }
        }
        DisplayItems();

        return true;
    }
}
