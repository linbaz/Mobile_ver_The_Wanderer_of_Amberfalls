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
        // ������������� ����� ������ ������
        GameObject gameObject = new GameObject();
        dungeonGenerator = gameObject.AddComponent<CorridorFirstDungeonGenerator>();
    }

    [TearDown]
    public void Teardown()
    {
        // ������� ����� ������� �����
        Object.DestroyImmediate(dungeonGenerator.gameObject);
    }

    [Test]
    public void CorridorFirstGeneration_FloorPositionsNotEmpty()
    {
        // ��������, ��� ����� ��������� ��������� ��������� ���� �� ������
        dungeonGenerator.CorridorFirstGeneration();
        Assert.NotNull(dungeonGenerator.floorPositions);
        Assert.IsTrue(dungeonGenerator.floorPositions.Count > 0);
    }

    [Test]
    public void FindAllDeadEnds_ReturnsDeadEnds()
    {
        // �������� ��������� ��������� ������� ����
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

        // ��������, ��� ��������� ������ ������������� ���������
        Assert.AreEqual(2, deadEnds.Count);
        Assert.Contains(new Vector2Int(3, 0), deadEnds);
        Assert.Contains(new Vector2Int(2, 0), deadEnds);
    }

    // �������� ������ ����� ��� ��������� ������� ������ �� ��������
}
