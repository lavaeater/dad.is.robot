var _ = require('lodash');

var tileTypes = ['grass', 'water','desert', 'mountain', 'forest'];
var tileRelations = [
    { grass: [] },
    { water: [] },
    { desert: [] },
    { mountain: [] },
    { forest: [] }
];

function TileRelation(tileType)


var tileGenerator = function(above, below, left, right, x, y) {
    //Takes into account the tiles around this tile to return a new type of tile at some position
};


function World(size, tileSize) {
    var width = size;
    var height = size;
    var tileSize = tileSize;

    var tiles = {};
    var getTileAt = function(x, y) {
        var key = x.toString() + ":" + y.toString();
        if(_.has(tiles, key)) {
            return tiles[key];
        }
    };

    return {
        width: width,
        height: height,
        getTileAt: getTileAt
    };
};

function Tile(x, y, type) {
    var key = x.toString() + ":" + y.toString();
    var x = x;
    var y = y;
    var type = type;

    return {
        key: key,
        x: x,
        y: y,
        type: type
    };
};