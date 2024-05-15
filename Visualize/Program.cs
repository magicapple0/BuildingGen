
using var game = new Visualize.Game1(BuildingGen.Program.Build((8, 8, 8), 3, "TileSetups/well.json"));
//в нечетном, соседи тайлов обрабатывать

game.Run();
