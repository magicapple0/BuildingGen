﻿namespace BuildingGen;

public class WaveFunction2
{
    private int Seed { get; set; }
    private Random Rand { get; set; }
    public State2 CurrState { get; private set; }
    private readonly Stack<State2> _previousStates = new ();

    public WaveFunction2(Vector2 size, TileManager tileManager, int seed, bool xSymmetry, bool ySymmetry)
    {
        CurrState = new State2(size, tileManager, xSymmetry, ySymmetry);
        Seed = seed;
        Rand = new Random(Seed);
    }

    public bool Run()
    {
        var n = 1;
        CurrState.Wave();
        if (CurrState.IsCollapse())
            return true;
        _previousStates.Push(CurrState);
        CurrState = CurrState.Copy();
        CurrState.Neighbors.Add((1, 1));
        
        while (true)
        {
            Console.Write($"\n{n++}:{_previousStates.Count}\t");
            if (CurrState.PossibleMoves == null)
                CurrState.CalculateMoves(Rand);
            while (CurrState.PossibleMoves == null || CurrState.PossibleMoves.Count == 0)
            {
                CurrState = _previousStates.Pop();
                //Console.Write("Шаг назад\t");
            }

            var move = CurrState.PossibleMoves.Dequeue();
            var currState = CurrState.Copy();
            currState.SetTile(move.Item1, move.Item2);
            currState.Wave();
            if (currState.IsBroken())
            {
                //Console.Write("Другой тайл\t");
                continue;
            }
            if (currState.IsCollapse())
            {
                CurrState = currState;
                return true;
            }

            _previousStates.Push(CurrState);
            CurrState = currState;
            //Console.Write("Дальше\t");
        }
    }
}