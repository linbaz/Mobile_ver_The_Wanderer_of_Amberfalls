using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Клас для допомоги при розташуванні предметів в кімнатах
public class ItemPlacementHelper
{
    // Словник для збереження позицій плиток за типами розташування
    Dictionary<PlacementType, HashSet<Vector2Int>> tileByType = new Dictionary<PlacementType, HashSet<Vector2Int>>();

    // Множина позицій на підлозі кімнати без коридорів
    HashSet<Vector2Int> roomFloorNoCorridor;

    // Конструктор класу
    public ItemPlacementHelper(HashSet<Vector2Int> roomFloor, HashSet<Vector2Int> roomFloorNoCorridor)
    {
        // Створення графу на основі позицій плиток у кімнаті
        Graph graph = new Graph(roomFloor);
        this.roomFloorNoCorridor = roomFloorNoCorridor;

        // Проходження крізь позиції на підлозі кімнати без коридорів
        foreach (var position in roomFloorNoCorridor)
        {
            int neighboursCount8Dir = graph.GetNeighbours8Directions(position).Count;
            PlacementType type = neighboursCount8Dir < 8 ? PlacementType.NearWall : PlacementType.OpenSpace;

            // Ініціалізація множини для даного типу розташування, якщо вона не існує
            if (tileByType.ContainsKey(type) == false)
                tileByType[type] = new HashSet<Vector2Int>();

            // Визначення позиції на підлозі відносно типу розташування
            if (type == PlacementType.NearWall && graph.GetNeighbours4Directions(position).Count == 4)
                continue;
            tileByType[type].Add(position);
        }
    }

    // Метод для отримання позиції розташування предмета з вказаним типом розташування
    public Vector2? GetItemPlacementPosition(PlacementType placementType, int iterationsMax, Vector2Int size, bool addOffset)
    {
        int itemArea = size.x * size.y;
        if (tileByType[placementType].Count < itemArea)
            return null;

        int iteration = 0;
        while (iteration < iterationsMax)
        {
            iteration++;
            int index = UnityEngine.Random.Range(0, tileByType[placementType].Count);
            Vector2Int position = tileByType[placementType].ElementAt(index);

            if (itemArea > 1)
            {
                var (result, placementPositions) = PlaceBigItem(position, size, addOffset);

                if (result == false)
                    continue;

                tileByType[placementType].ExceptWith(placementPositions);
                tileByType[PlacementType.NearWall].ExceptWith(placementPositions);
            }
            else
            {
                tileByType[placementType].Remove(position);
            }

            return position;
        }
        return null;
    }

    // Метод для розташування великих предметів та визначення їх позицій
    private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPosition, Vector2Int size, bool addOffset)
    {
        List<Vector2Int> positions = new List<Vector2Int>() { originPosition };
        int maxX = addOffset ? size.x + 1 : size.x;
        int maxY = addOffset ? size.y + 1 : size.y;
        int minX = addOffset ? -1 : 0;
        int minY = addOffset ? -1 : 0;

        for (int row = minX; row <= maxX; row++)
        {
            for (int col = minY; col <= maxY; col++)
            {
                if (col == 0 && row == 0)
                    continue;
                Vector2Int newPosToCheck = new Vector2Int(originPosition.x + row, originPosition.y + col);
                if (roomFloorNoCorridor.Contains(newPosToCheck) == false)
                    return (false, positions);
                positions.Add(newPosToCheck);
            }
        }
        return (true, positions);
    }
}

// Перерахування для типів розташування
public enum PlacementType
{
    OpenSpace,  // Вільний простір
    NearWall    // Біля стіни
}
