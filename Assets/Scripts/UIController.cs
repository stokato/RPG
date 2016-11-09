using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {
    [SerializeField]
    private Text healthLabel; // Ссылка на UI-объект в сцене
    [SerializeField]
    private InventoryPopup popup;

    void Awake()
    {
        Messenger.AddListener(GameEvent.HEALT_UPDATED, OnHealthUpdated);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.HEALT_UPDATED, OnHealthUpdated);
    }

	// Use this for initialization
	void Start () {
        OnHealthUpdated();
        popup.gameObject.SetActive(false); // Всплывающее окно инициализируется как скрытое
	}
	
	// Update is called once per frame
	void Update () {
	if(Input.GetKeyDown(KeyCode.M))
        {
            bool isShowing = popup.gameObject.activeSelf;
            popup.gameObject.SetActive(!isShowing);
            popup.Refresh();
        }
	}

    private void OnHealthUpdated()
    {
        string message = "Health: " + Managers.Player.health + "/" + Managers.Player.maxHealth;
        healthLabel.text = message;
    }
}
