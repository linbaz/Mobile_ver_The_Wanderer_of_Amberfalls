using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Клас, що реалізує алгоритм Дейкстри для знаходження найкоротших шляхів у графі
public class DijkstraAlgorithm
{
    // Статичний метод для виконання алгоритму Дейкстри
    public static Dictionary<Vector2Int, int> Dijkstra(Graph graph, Vector2Int startposition)
    {
        // Черга для незавершених вершин
        Queue<Vector2Int> unfinishedVertices = new Queue<Vector2Int>();

        // Зберігання відстаней до вершин від стартової вершини
        Dictionary<Vector2Int, int> distanceDictionary = new Dictionary<Vector2Int, int>();

        // Зберігання батьківських вершин для кожної вершини
        Dictionary<Vector2Int, Vector2Int> parentDictionary = new Dictionary<Vector2Int, Vector2Int>();

        // Встановлення початкової відстані та батьківської вершини для стартової вершини
        distanceDictionary[startposition] = 0;
        parentDictionary[startposition] = startposition;

        // Додавання сусідів стартової вершини до черги та встановлення їхніх батьківських вершин
        foreach (Vector2Int vertex in graph.GetNeighbours4Directions(startposition))
        {
            unfinishedVertices.Enqueue(vertex);
            parentDictionary[vertex] = startposition;
        }

        // Цикл виконання алгоритму Дейкстри
        while (unfinishedVertices.Count > 0)
        {
            // Вибір вершини з черги
            Vector2Int vertex = unfinishedVertices.Dequeue();

            // Обчислення нової відстані до поточної вершини
            int newDistance = distanceDictionary[parentDictionary[vertex]] + 1;

            // Пропуск, якщо відстань до вершини вже визначена та менша або дорівнює новій відстані
            if (distanceDictionary.ContainsKey(vertex) && distanceDictionary[vertex] <= newDistance)
                continue;

            // Оновлення відстані до вершини
            distanceDictionary[vertex] = newDistance;

            // Додавання сусідів поточної вершини до черги та встановлення їхніх батьківських вершин
            foreach (Vector2Int neighbour in graph.GetNeighbours4Directions(vertex))
            {
                if (distanceDictionary.ContainsKey(neighbour))
                    continue;

                unfinishedVertices.Enqueue(neighbour);
                parentDictionary[neighbour] = vertex;
            }
        }

        // Повернення відстаней до вершин від стартової вершини
        return distanceDictionary;
    }
}
