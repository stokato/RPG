﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventoryPopup : MonoBehaviour {

    [SerializeField]
    private Image[] itemIcons;
    [SerializeField]
    private Text[] itemLabels;

    [SerializeField]
    private Text curItemLabel;
    [SerializeField]
    private Button equipButton;
    [SerializeField]
    private Button useButton;

    private string _curItem;

    public void Refresh()
    {
        List<string> itemList = Managers.Inventory.GetItemList();

        int len = itemIcons.Length;
        for (int i = 0; i < len; i++) // Проверка списка инвентаря в процессе циклического просмотра всех изображений элементов UI
        {
            if(i < itemList.Count)
            {
                itemIcons[i].gameObject.SetActive(true);
                itemIcons[i].gameObject.SetActive(true);

                string item = itemList[i];

                Sprite sprite = Resources.Load<Sprite>("Icons/" + item); // Загрузка спрайта из папки Resources
                itemIcons[i].sprite = sprite;
                itemIcons[i].SetNativeSize(); // Изменение размеров изображения под исходный размер спрайта

                int count = Managers.Inventory.GetItemCount(item);
                string message = "x" + count;

                if(item == Managers.Inventory.equippedItem)
                {
                    message = "Equipped\n" + message; // На метке может появиться не только количество элементов но и "Equipped"
                }
                itemLabels[i].text = message;

                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick; // Превращаем значки в интерактивные объекты
                entry.callback.AddListener((BaseEventData data) =>
                {
                    OnItem(item);
                });

                EventTrigger trigger = itemIcons[i].GetComponent<EventTrigger>();
                trigger.triggers.Clear(); // Сброс подписчика чтобы начать с чистого листа
                trigger.triggers.Add(entry); // Добавление функции-подписчика к классу EventTrigger
            }
            else
            {
                itemIcons[i].gameObject.SetActive(false);
                itemLabels[i].gameObject.SetActive(false);
            }
        }

        if(!itemList.Contains(_curItem))
        {
            _curItem = null;
        }
        if(_curItem == null) // Скрываем кнопки при отсутствии выделенных элементов
        {
            curItemLabel.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(false);
            useButton.gameObject.SetActive(false);
        }
        else
        {
            curItemLabel.gameObject.SetActive(true);
            equipButton.gameObject.SetActive(true);
            if(_curItem == "health")
            {
                useButton.gameObject.SetActive(true);
            }
            else
            {
                useButton.gameObject.SetActive(false);
            }

            curItemLabel.text = _curItem + ":";
        }
    }

    public void OnItem(string item)
    {
        _curItem = item;
        Refresh(); 
    }

    public void OnEquip()
    {
        Managers.Inventory.EquipItem(_curItem);
        Refresh();
    }

    public void OnUse()
    {
        Managers.Inventory.ConsumeItem(_curItem);
        if(_curItem == "health")
        {
            Managers.Player.ChangeHealth(25);
        }
        Refresh();
    }
}
