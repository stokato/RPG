using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour , IGameManager {

    public ManagerStatus status { get; private set; }

    public int health { get; private set; }
    public int maxHealth { get; private set; }

    private NetworkService _network;

    public void Startup(NetworkService service)
    {
        Debug.Log("Player manager startup...");

        health = 50;
        maxHealth = 100;

        _network = service;

        UpdateData(50, 100);

        status = ManagerStatus.Started;
    }

    public void ChangeHealth(int value)
    {
        health += value;

        if(health > maxHealth)
        {
            health = maxHealth;
        } else if(health < 0)
        {
            health = 0;
        }

        if(health == 0)
        {
            Messenger.Broadcast(GameEvent.LEVEL_FAILED);
        }

        Messenger.Broadcast(GameEvent.HEALT_UPDATED);
    }

    public void UpdateData(int health, int maxHealth)
    {
        this.health = health;
        this.maxHealth = maxHealth;
    }

    public void Respawn()
    {
        UpdateData(50, 100); // Возвращаемся в исходное положение
    }
}
