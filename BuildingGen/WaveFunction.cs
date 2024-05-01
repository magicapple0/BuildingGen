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
        CurrModel.Wave();
        if (CurrModel.IsCollapse())
            return true;
        _previousModels.Push(CurrModel);
        CurrModel = CurrModel.Copy();
        while (true)
        {
            if (CurrModel.PossibleMoves == null)
                CurrModel.CalculateMoves(Rand);
            while (CurrModel.PossibleMoves == null || CurrModel.PossibleMoves.Count == 0)
            {
                CurrModel = _previousModels.Pop();
            }

            var move = CurrModel.PossibleMoves.Dequeue();
            var newModel = CurrModel.Copy();
            newModel.Field[move.Item1] = new[] { move.Item2 };
            newModel.Wave();
            if (newModel.IsBroken()) continue;
            if (newModel.IsCollapse())
            {
                CurrModel = newModel;
                return true;
            }

            _previousModels.Push(CurrModel);
            CurrModel = newModel;
        }
    }
}