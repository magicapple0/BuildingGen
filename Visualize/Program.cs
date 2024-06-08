
//using var game = new Visualize.Game1(BuildingGen.Program.BuildFromInputTileSet((7, 7, 9), 0, "TileSetups/triangleRoof.json"));
//using var game = new Visualize.Core(BuildingGen.Program.BuildFromInputField((8, 8, 7), 5, "InputFields/simpleHouse.json"));
//(9, 9, 9), 3 
using var game = new Visualize.Core(BuildingGen.Program.CreateExampleField());
//using var game = new Visualize.Core(BuildingGen.Program.BuildTest2DField());
//using var game = new Visualize.Core(BuildingGen.Program.BuildFromInput2DField((10, 10), 4, "InputFields/inputField2D.json"));

game.Run();
  