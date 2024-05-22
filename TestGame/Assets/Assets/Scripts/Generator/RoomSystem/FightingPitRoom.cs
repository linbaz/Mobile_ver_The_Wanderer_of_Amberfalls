using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightingPitRoom : RoomGenerator
{
    [SerializeField]
    private PrefabPlacer prefabPlacer;

    // Дані про розташування ворогів у кімнаті
    public List<EnemyPlacementData> enemyPlacementData;

    // Дані про розташування предметів у кімнаті
    public List<ItemPlacementData> itemData;

    // Перевизначений метод для обробки кімнати
    public override List<GameObject> ProcessRoom(Vector2Int roomCenter, HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridors)
    {
        // Створення об'єкта для розташування предметів
        ItemPlacementHelper itemPlacementHelper =
            new ItemPlacementHelper(roomFloor, roomFloorNoCorridors);

        // Розташування всіх предметів у кімнаті
        List<GameObject> placedObjects =
            prefabPlacer.PlaceAllItems(itemData, itemPlacementHelper);

        // Додавання ворогів до списку розташованих об'єктів
        placedObjects.AddRange(prefabPlacer.PlaceEnemies(enemyPlacementData, itemPlacementHelper));

        // Повертаємо список розташованих об'єктів у кімнаті
        return placedObjects;
    }
}
