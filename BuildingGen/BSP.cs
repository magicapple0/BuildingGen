namespace BuildingGen;

public static class BSP
{
    public static Dictionary<int, List<Vector2>> GetFoundations(Dictionary<Vector2, Tile> map, Vector2 minSize, Vector2 maxSize, Random random)
    {
        var foundations = GetRectangles(map);
        foundations = DeleteSmallRectangles(foundations, minSize);
        while (IsBiggerRectanglesExist(foundations, maxSize))
            foundations = DivideBigRectangles(foundations, maxSize, minSize, random);
        //Paint(foundations, map);
        return foundations;
    }

    private static bool IsBiggerRectanglesExist(Dictionary<int, List<Vector2>> rectangles, Vector2 maxSize)
    {
        foreach (var rectangle in rectangles)
        {
            var maxPoint = rectangle.Value.Aggregate((0, 0),
                (max, pair) => (Math.Max(max.Item1, pair.X), Math.Max(max.Item2, pair.Y)));
            var minPoint = rectangle.Value.Aggregate((9999999999, 99999999999),
                (min, pair) => (Math.Min(min.Item1, pair.X), Math.Min(min.Item2, pair.Y)));
            var size = (maxPoint.Item1 - minPoint.Item1 + 1, maxPoint.Item2 - minPoint.Item2 + 1);
            if ((size.Item1 >= maxSize.X || size.Item2 >= maxSize.Y) &&
                (size.Item2 >= maxSize.X || size.Item1 >= maxSize.Y))
            {
                return true;
            }
        }

        return false;
    }

    private static Dictionary<int, List<Vector2>> DivideBigRectangles(Dictionary<int, List<Vector2>> rectangles, Vector2 maxSize, Vector2 minSize, Random random)
    {
        var newRectangles = new Dictionary<int, List<Vector2>>();
        var maxId = rectangles.Keys.Max() + 1;
        foreach (var rectangle in rectangles)
        {
            var maxPoint = rectangle.Value.Aggregate((0, 0),
                (max, pair) => (Math.Max(max.Item1, pair.X), Math.Max(max.Item2, pair.Y)));
            var minPoint = rectangle.Value.Aggregate((9999999999, 99999999999),
                (min, pair) => (Math.Min(min.Item1, pair.X), Math.Min(min.Item2, pair.Y)));
            var size = (maxPoint.Item1 - minPoint.Item1 + 1, maxPoint.Item2 - minPoint.Item2 + 1);
            if ((size.Item1 >= maxSize.X || size.Item2 >= maxSize.Y) &&
                (size.Item2 >= maxSize.X || size.Item1 >= maxSize.Y))
            {
                var smallRectangles = DivideRectangle(rectangle.Value, minSize, random);
                foreach (var r in smallRectangles)
                {
                    newRectangles.Add(maxId++, r);
                }
            }
            else
            {
                newRectangles.Add(rectangle.Key, rectangle.Value);
            }
        }
        return newRectangles;
    }

    private static List<List<Vector2>> DivideRectangle(List<Vector2> rectangle, Vector2 minSize, Random random)
    {
        var maxPoint = rectangle.Aggregate((0, 0),
            (max, pair) => (Math.Max(max.Item1, pair.X), Math.Max(max.Item2, pair.Y)));
        var minPoint = rectangle.Aggregate((9999999999, 99999999999),
            (min, pair) => (Math.Min(min.Item1, pair.X), Math.Min(min.Item2, pair.Y)));
        var size = new Vector2((int)(maxPoint.Item1 - minPoint.Item1 + 1), (int)(maxPoint.Item2 - minPoint.Item2 + 1));
        (List<Vector2>, List<Vector2>) dividedRectangles;
        dividedRectangles = Divide(size.X >= size.Y ? new Vector2(Math.Max(minSize.X, size.X / 2 + random.Next(0, 2) - 1), size.Y) : 
            new Vector2(size.X, Math.Max(minSize.Y, size.Y / 2 + random.Next(0, 2) - 1)), minPoint, rectangle);
        return new List<List<Vector2>>() { dividedRectangles.Item1, dividedRectangles.Item2 };

    }

    private static (List<Vector2>, List<Vector2>) Divide(Vector2 point, (long, long) minPoint, List<Vector2> rectangle)
    {
        var firstRectangle = new List<Vector2>();
        for (int i = 0; i < point.X; i++)
        {
            for (int j = 0; j < point.Y; j++)
            {
                firstRectangle.Add((minPoint.Item1 + i, minPoint.Item2 + j));
            }
        }
        var secondRectangle = new List<Vector2>();
        foreach (var cell in rectangle)
        {
            if (!firstRectangle.Contains(cell))
                secondRectangle.Add(cell);
        }

        return (firstRectangle, secondRectangle);
    }

    private static Dictionary<int, List<Vector2>> DeleteSmallRectangles(Dictionary<int, List<Vector2>> rectangles, Vector2 minSize)
    {
        var newRectangles = new Dictionary<int, List<Vector2>>();
        foreach (var rectangle in rectangles)
        {
            var maxPoint = rectangle.Value.Aggregate((0, 0),
                (max, pair) => (Math.Max(max.Item1, pair.X), Math.Max(max.Item2, pair.Y)));
            var minPoint = rectangle.Value.Aggregate((9999999999, 99999999999),
                (min, pair) => (Math.Min(min.Item1, pair.X), Math.Min(min.Item2, pair.Y)));
            var size = (maxPoint.Item1 - minPoint.Item1 + 1, maxPoint.Item2 - minPoint.Item2 + 1);
            if ((size.Item1 >= minSize.X && size.Item2 >= minSize.Y) ||
                (size.Item2 >= minSize.X && size.Item1 >= minSize.Y))
                newRectangles.Add(rectangle.Key, rectangle.Value);
        }

        return newRectangles;
    }

    private static Dictionary<int, List<Vector2>> GetRectangles(Dictionary<Vector2, Tile> map)
    {
        var RectangleList = new Dictionary<int, List<Vector2>>();
        var VisitedCells = new List<Vector2>();
        var rectangleId = 0;
        while (true)
        {
            var currTile = new Vector2(0, 0);
            var rectanglePoints = new List<Vector2>();
            var UpLeftPoint = new Vector2(0, 0);
            var UpRightPoint = new Vector2(0, 0);
            while (map[currTile].TileInfo.Name != "grass" || VisitedCells.Contains(currTile))
            {
                if (map.ContainsKey((currTile.X + 1, currTile.Y)))
                    currTile.X++;
                else if (map.ContainsKey((currTile.X, currTile.Y + 1)))
                {
                    currTile.Y++;
                    currTile.X = 0;
                }
                else
                    return RectangleList;
            }
            UpLeftPoint = currTile;
            while (map.ContainsKey(currTile) && map[currTile].TileInfo.Name == "grass" && !VisitedCells.Contains(currTile))
            {
                rectanglePoints.Add(currTile);
                VisitedCells.Add(currTile);
                currTile.X++;
            }
            currTile.X--;
            UpRightPoint = currTile;
            while (true)
            {
                var f = false;
                if (map.ContainsKey((currTile.X, currTile.Y + 1)) && !VisitedCells.Contains((currTile.X, currTile.Y + 1)))
                {
                    currTile.Y++;
                }
                else
                {
                    RectangleList.Add(rectangleId, rectanglePoints);
                    rectangleId++;
                    break;
                }

                for (int k = 0; k < UpRightPoint.X - UpLeftPoint.X + 1; k++)
                {
                    currTile.X = UpLeftPoint.X + k;
                    if (!map.ContainsKey(currTile) || map[currTile].TileInfo.Name != "grass" || VisitedCells.Contains(currTile))
                    {
                        RectangleList.Add(rectangleId, rectanglePoints);
                        rectangleId++;
                        f = true;
                        break;
                    }
                }
                if (f)
                    break;
                for (int k = 0; k < UpRightPoint.X - UpLeftPoint.X + 1; k++)
                {
                    currTile.X = UpLeftPoint.X + k;
                    VisitedCells.Add(currTile);
                    rectanglePoints.Add(currTile);
                }
            }
        }
    }

    private static void Paint(Dictionary<int, List<Vector2>> rectangles, Dictionary<Vector2, Tile> map)
    {
        var colors = new List<String>()
        {
            "CD5C5C",
            "FFC0CB",
            "FFD700",
            "FFEFD5",
            "DDA0DD",
            "6A5ACD",
            "F4A460",
            "808080",
            "808000",
            "8FBC8F",
            "ADD8E6"
        };
        foreach (var rectangle in rectangles)
        {
            foreach (var cell in rectangle.Value)
            {
                map[cell].TileInfo.Color = colors[rectangle.Key % colors.Count];
            }
        }
    }
}