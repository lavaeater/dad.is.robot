var World = require('./js/world/worldgenerator.js');

/*
Create and traverse a world!
*
*
*/
var size = 10;
var world = new World(10, 100);

for (y = 0; y < size; y++) {
    for (x = 0; x < size; x++) {
        var tile = world.getTileAt(x, y);
        console.log("Current tile" + tile.key);
        console.log("Type: " + tile.type);
        tile.generateSubTiles();
        tile.renderSubTiles();
    }
}