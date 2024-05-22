using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

// Клас для генерації контенту у кімнатах
public class RoomContentGenerator : MonoBehaviour
{
    // Посилання на генератор кімнати для гравця та кімнату з ворогами
    [SerializeField]
    private RoomGenerator playerRoom, defaultRoom;

    // Список розташованих об'єктів в кімнаті
    List<GameObject> spawnedObjects = new List<GameObject>();

    // Посилання на граф
    [SerializeField]
    private GraphTest graphTest;

    // Батьківський об'єкт для згенерованих предметів
    public Transform itemParent;

    // Подія для регенерації данжу
    public UnityEvent RegenerateDungeon;



    // Генерація контенту для кімнати на основі даних про лабіринт
    public void GenerateRoomContent(DungeonData dungeonData)
    {
        // Знищення попередньо розташованих об'єктів
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        // Вибір точки спавну гравця та розташування предметів
        SelectPlayerSpawnPoint(dungeonData);
        SelectEnemySpawnPoints(dungeonData);

        // Розташування створених об'єктів у батьківському об'єкті для предметів
        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    // Вибір точки спавну гравця та генерація кімнати для гравця
    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
        Vector2Int playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        // Запуск алгоритму Дейкстри для знаходження шляху до гравця
        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector2Int roomIndex = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex);

        // Обробка кімнати для гравця та отримання списку розташованих об'єктів
        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
        );

        // Додавання розташованих об'єктів до списку
        spawnedObjects.AddRange(placedPrefabs);

        // Видалення кімнати для гравця зі словника лабіринту
        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    // Вибір точок спавну ворогів та генерація кімнат
    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in dungeonData.roomsDictionary)
        {
            // Обробка кожної кімнати та отримання списку розташованих об'єктів
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value,
                    dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                )
            );
        }
    }
}
