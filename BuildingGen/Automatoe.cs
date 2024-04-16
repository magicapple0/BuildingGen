using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingGen
{
    public class State
    {
        public Func<Tile, Tile> Modifier { get; set; }
        public List<State> NextStates { get; set; }
        public String Path;
    public List<Func<Tile, Tile>> NextModifiers { get; set; } 
        public State(State prev, Func<Tile, Tile> modifier)
        {
            NextStates = new List<State>();
            Modifier = modifier;
            NextModifiers = prev.NextModifiers.ToList();
            NextModifiers.Remove(modifier);
            Path += modifier.Method.Name + " ";
            for (int i = 0; i < NextModifiers.Count; i++)
            {
                Console.WriteLine(Path + NextModifiers[i].Method.Name + " ");
                NextStates.Add(new State(this, NextModifiers[i]));
            }
        }

        public State(Func<Tile, Tile> modifier, List<Func<Tile, Tile>> nextModifiers)
        {
            NextStates = new List<State>();
            Modifier = modifier;
            NextModifiers = nextModifiers;
            NextModifiers.Remove(modifier);
            for (int i = 0; i < NextModifiers.Count; i++)
            {
                Path = "";
                Console.WriteLine(Path + NextModifiers[i].Method.Name + " ");
                NextStates.Add(new State(this, NextModifiers[i]));
            }
        }
    }

    public class Automatoe
    {
        public Func<Tile, Tile>[] Modifiers;
        public State automaton;
        public Automatoe(Func<Tile, Tile>[] modifiers) {
            Modifiers = modifiers;
            var nextModifiers = modifiers.ToList();
            automaton = new State(modifiers[0], nextModifiers);
        }
    }
}
