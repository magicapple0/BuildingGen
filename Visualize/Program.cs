using System;

//using var game = new Visualize.Game1(BuildingGen.Program.BuildFromInputTileSet((7, 7, 9), 0, "TileSetups/triangleRoof.json"));
//using var game = new Visualize.Core(BuildingGen.Program.BuildFromInputField((6, 6, 10), 0, "InputFields/inputField3D.json"));
//(9, 9, 9), 3 
//using var game = new Visualize.Core(BuildingGen.Program.CreateExampleField());
//using var game = new Visualize.Core(BuildingGen.Program.BuildTest2DField());
//using var game = new Visualize.Core(BuildingGen.Program.BuildTest3DField());
//using var game = new Visualize.Core(BuildingGen.Program.BuildFromInput2DField((15, 13), 4, "InputFields/inputField2D.json"));
//using var game = new Visualize.Core(BuildingGen.Program.GenerateFromTest2DField((15, 13), 0));


//using var game = new Visualize.Core(BuildingGen.Program.BuildJsonFile("InputFields/simpleHouse.json"));
//using var game = new Visualize.Core(BuildingGen.Program.DivideGeneratedField("InputFields/generatedField2D2.json", (3, 3), (8, 8), 0));
//using var game = new Visualize.Core(BuildingGen.Program.GenerateStreet("InputFields/generatedField2D.json", "TileSetups/triangleRoof.json", (4, 4), (8, 8), 0));
//using var game = new Visualize.Core(BuildingGen.Program.GenerateStreetFromInputFieldFile("InputFields/generatedField2D2.json", "InputFields/simpleHouse.json", (4, 4), (8, 8), 2));

//using var game = new Visualize.Core(BuildingGen.Program.BuildTest3DField());
//GenerateStreet(String inputFieldFileName, String inputHouseFileName, int seed, (x1, y1, z1), (x2, y2, z2))

/*using var game = new Visualize.Core(BuildingGen.Program.GenerateStreet(
        "InputFields/inputField2D.json", 
        "InputFields/simpleHouse.json",
        (4, 4), (8, 8), 2));*/
//using var game = new Visualize.Core(BuildingGen.Program.GenerateFromTestField((6,6,10), 1));
using var game = new Visualize.Core(BuildingGen.Program.CreateExampleField3D2());
game.Run();
  