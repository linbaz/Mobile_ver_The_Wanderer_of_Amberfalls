using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Клас для розташування об'єктів у сцені
public class PrefabPlacer : MonoBehaviour
{
    // Префаб предмету, який буде розміщений
    [SerializeField]
    private GameObject itemPrefab;

    // Розташування ворогів у кімнаті за вказаними даними
    public List<GameObject> PlaceEnemies(List<EnemyPlacementData> enemyPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        foreach (var placementData in enemyPlacementData)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                // Отримання можливої позиції для розташування ворога
                Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                    PlacementType.OpenSpace,
                    100,
                    placementData.enemySize,
                    false
                );

                if (possiblePlacementSpot.HasValue)
                {
                    // Створення об'єкта ворога та додавання його до списку розташованих об'єктів
                    placedObjects.Add(CreateObject(placementData.enemyPrefab, possiblePlacementSpot.Value + new Vector2(0.5f, 0.5f)));
                }
            }
        }
        return placedObjects;
    }

    // Розташування всіх предметів у кімнаті за вказаними даними
    public List<GameObject> PlaceAllItems(List<ItemPlacementData> itemPlacementData, ItemPlacementHelper itemPlacementHelper)
    {
        List<GameObject> placedObjects = new List<GameObject>();

        // Сортування списку за спаданням розмірів предметів
        IEnumerable<ItemPlacementData> sortedList = new List<ItemPlacementData>(itemPlacementData).OrderByDescending(placementData => placementData.itemData.size.x * placementData.itemData.size.y);

        foreach (var placementData in sortedList)
        {
            for (int i = 0; i < placementData.Quantity; i++)
            {
                // Отримання можливої позиції для розташування предмету
                Vector2? possiblePlacementSpot = itemPlacementHelper.GetItemPlacementPosition(
                    placementData.itemData.placementType,
                    100,
                    placementData.itemData.size,
                    placementData.itemData.addOffset
                );

                if (possiblePlacementSpot.HasValue)
                {
                    // Розміщення предмету та додавання його до списку розташованих об'єктів
                    placedObjects.Add(PlaceItem(placementData.itemData, possiblePlacementSpot.Value));
                }
            }
        }
        return placedObjects;
    }

    // Розміщення предмету у вказаній позиції
    private GameObject PlaceItem(ItemData item, Vector2 placementPosition)
    {
        // Створення об'єкта предмету та ініціалізація його даними
        GameObject newItem = CreateObject(itemPrefab, placementPosition);
        newItem.GetComponent<ItemInDungeon>().Initialize(item);
        return newItem;
    }

    // Створення об'єкта за допомогою префабу
    public GameObject CreateObject(GameObject prefab, Vector3 placementPosition)
    {
        GameObject newItem = null;

        // Перевірка, чи префаб існує
        if (prefab == null)
            return null;

        // Створення об'єкта у режимі відтворення гри
        if (Application.isPlaying)
        {
            newItem = Instantiate(prefab, placementPosition, Quaternion.identity);
        }
        // Створення об'єкта у редакторі
        else
        {
#if UNITY_EDITOR
            newItem = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            newItem.transform.position = placementPosition;
            newItem.transform.rotation = Quaternion.identity;
#endif
        }

        return newItem;
    }
}
