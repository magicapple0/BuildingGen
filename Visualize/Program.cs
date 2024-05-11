
using var game = new Visualize.Game1(BuildingGen.Program.Build((7, 8, 10), 3, "TileSetups/pileHouse.json"));
//в нечетном, соседи тайлов обрабатывать

game.Run();
