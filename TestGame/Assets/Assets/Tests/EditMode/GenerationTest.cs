using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class CorridorFirstDungeonGeneratorTests
{
    private CorridorFirstDungeonGenerator dungeonGenerator;

    [SetUp]
    public void Setup()
    {
        // »нициализаци€ перед каждым тестом
        GameObject gameObject = new GameObject();
        dungeonGenerator = gameObject.AddComponent<CorridorFirstDungeonGenerator>();
    }

    [TearDown]
    public void Teardown()
    {
        // ќчистка после каждого теста
        Object.DestroyImmediate(dungeonGenerator.gameObject);
    }

    [Test]
    public void CorridorFirstGeneration_FloorPositionsNotEmpty()
    {
        // ѕроверка, что после генерации коридоров положение пола не пустое
        dungeonGenerator.CorridorFirstGeneration();
        Assert.NotNull(dungeonGenerator.floorPositions);
        Assert.IsTrue(dungeonGenerator.floorPositions.Count > 0);
    }

    [Test]
    public void FindAllDeadEnds_ReturnsDeadEnds()
    {
        // —оздание тестового множества позиций пола
        HashSet<Vector2Int> testFloorPositions = new HashSet<Vector2Int>
        {
            new Vector2Int(0, 0),
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(2, 0),
            new Vector2Int(3, 0)
        };

        List<Vector2Int> deadEnds = dungeonGenerator.FindAllDeadEnds(testFloorPositions);

        // ѕроверка, что найденные тупики соответствуют ожидани€м
        Assert.AreEqual(2, deadEnds.Count);
        Assert.Contains(new Vector2Int(3, 0), deadEnds);
        Assert.Contains(new Vector2Int(2, 0), deadEnds);
    }

    // ƒобавьте другие тесты дл€ остальных методов класса по аналогии
}
