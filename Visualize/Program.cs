
//using var game = new Visualize.Game1(BuildingGen.Program.BuildFromInputTileSet((7, 7, 9), 0, "TileSetups/triangleRoof.json"));
//using var game = new Visualize.Game1(BuildingGen.Program.BuildFromInputField((8, 8, 10), 7, "InputFields/simpleHouse.json"));
using var game = new Visualize.Game1(BuildingGen.Program.BuildTestTile());

game.Run();
