namespace BuildingGen;

public class WaveFunction
{
    private int Seed { get; set; }
    private Random Rand { get; set; }
    public Model CurrModel { get; set; }
    private readonly Stack<Model> _previousModels = new ();

    public WaveFunction(int width, int depth, int height, Tile[] tileSet, int seed)
    {
        CurrModel = new Model(width + 2, depth + 2, height + 2, tileSet);
        Seed = seed;
        Rand = new Random(Seed);
    }

    public bool Run()
    {
        var n = 1;
        CurrModel.Wave();
        if (CurrModel.IsCollapse())
            return true;
        _previousModels.Push(CurrModel);
        CurrModel = CurrModel.Copy();
        CurrModel.Neighbors.Add((1, 1, 1));
        
        while (true)
        {
            Console.Write($"\n{n++}:{_previousModels.Count}\t");
            if (CurrModel.PossibleMoves == null)
                CurrModel.CalculateMoves(Rand);
            while (CurrModel.PossibleMoves == null || CurrModel.PossibleMoves.Count == 0)
            {
                CurrModel = _previousModels.Pop();
                Console.Write("Шаг назад\t");
            }

            var move = CurrModel.PossibleMoves.Dequeue();
            var newModel = CurrModel.Copy();
            newModel.SetTile(move.Item1, move.Item2);
            newModel.Wave();
            if (newModel.IsBroken())
            {
                Console.Write("Другой тайл\t");
                continue;
            }
            if (newModel.IsCollapse())
            {
                CurrModel = newModel;
                return true;
            }

            _previousModels.Push(CurrModel);
            CurrModel = newModel;
            Console.Write("Дальше\t");
        }
    }
}