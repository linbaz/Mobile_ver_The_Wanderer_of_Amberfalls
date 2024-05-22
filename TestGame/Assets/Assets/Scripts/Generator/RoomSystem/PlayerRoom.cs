using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Клас, що відповідає за генерацію кімнати з гравцем
public class PlayerRoom : RoomGenerator
{
    // Гравець, який буде розміщений у кімнаті
    public GameObject player;

    // Дані про розташування предметів у кімнаті
    public List<ItemPlacementData> itemData;

    // Клас для розташування об'єктів у кімнаті
    [SerializeField]
    private PrefabPlacer prefabPlacer;

    // Перевизначений метод для обробки кімнати
    public override List<GameObject> ProcessRoom(
        Vector2Int roomCenter,
        HashSet<Vector2Int> roomFloor,
        HashSet<Vector2Int> roomFloorNoCorridors)
    {
        // Створення об'єкта для розташування предметів
        ItemPlacementHelper itemPlacementHelper =
            new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // Розташування всіх предметів у кімнаті
        List<GameObject> placedObjects =
            prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        // Визначення точки спавну гравця (по центру кімнати)
        Vector2Int playerSpawnPoint = roomCenter;

        // Створення об'єкта гравця та додавання його до списку розташованих об'єктів
        GameObject playerObject
            = prefabPlacer.CreateObject(player, playerSpawnPoint + new Vector2(0.5f, 0.5f));
        placedObjects.Add(playerObject);

        // Повертаємо список розташованих об'єктів у кімнаті
        return placedObjects;
    }
}

// Абстрактний клас для даних про розташування об'єктів
public abstract class PlacementData
{
    // Мінімальна кількість об'єктів
    [Min(0)]
    public int minQuantity = 0;

    // Максимальна кількість об'єктів (включно)
    [Min(0)]
    [Tooltip("Максимальна кількість включно")]
    public int maxQuantity = 0;

    // Випадкова кількість об'єктів в діапазоні від minQuantity до maxQuantity
    public int Quantity
        => UnityEngine.Random.Range(minQuantity, maxQuantity + 1);
}

// Клас для даних про розташування предметів
[Serializable]
public class ItemPlacementData : PlacementData
{
    // Дані про предмет
    public ItemData itemData;
}

// Клас для даних про розташування ворогів
[Serializable]
public class EnemyPlacementData : PlacementData
{
    // Префаб ворога
    public GameObject enemyPrefab;

    // Розмір ворога у вузлах сітки
    public Vector2Int enemySize = Vector2Int.one;
}
