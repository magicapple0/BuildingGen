﻿using System.Diagnostics;

namespace BuildingGen;

public class WaveFunction
{
    private int Seed { get; set; }
    private Random Rand { get; set; }
    public State CurrState { get; private set; }
    private readonly Stack<State> _previousStates = new ();

    public WaveFunction(Vector3 size, TileManager tileManager, int seed, bool xSymmetry, bool ySymmetry)
    {
        CurrState = new State(size, tileManager, xSymmetry, ySymmetry);
        Seed = seed;
        Rand = new Random(Seed);
    }

    public bool Run()
    {
        var n = 1;
        var sw = new Stopwatch();
        sw.Start();
        CurrState.Wave();
        sw.Stop();
        Console.Write(sw.Elapsed);
        if (CurrState.IsCollapse())
            return true;
        CurrState.Neighbors.Add((1, 1, 1));
        _previousStates.Push(CurrState);
        CurrState = CurrState.Copy();
        
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