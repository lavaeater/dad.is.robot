function RandomIntFromInterval(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
};

function basicTileGenerator(x, y) {
    var randomSeed = RandomIntFromInterval(1, 10);
    var type = '';
    if (randomSeed <= 4) {
        type = 'g';
    } else if (4 < randomSeed && randomSeed <= 7) {
        type = 'f';
    } else if (7 < randomSeed && randomSeed <= 8) {
        type = 'w';
    } else if (8 < randomSeed && randomSeed <= 9) {
        type = 'm';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'd';
    }

    return new Tile(x, y, type, Generators[type]);
};

function Tile(x, y, type, subTileGenerator) {
    var self = this;
    var key = KeyToString(x, y);
    var x = x;
    var y = y;
    var type = type;
    var size = 10;
    var subTiles = {};
    self.subTileGenerator = subTileGenerator;

    var generateSubTiles = function () {
        for (i = 0; i < size; i++) {
            for (j = 0; j < size; j++) {
                var tile = self.subTileGenerator(j, i);
                subTiles[tile.key] = tile;
            }
        }
    };

    var renderSubTiles = function () {
        for (i = 0; i < size; i++) {
            var row = '';
            for (j = 0; j < size; j++) {
                var key = KeyToString(j, i);
                row += subTiles[key].type;
            }
            console.log(row);
        }
    };

    return {
        key: key,
        x: x,
        y: y,
        type: type,
        subTiles: subTiles,
        renderSubTiles: renderSubTiles,
        generateSubTiles: generateSubTiles
    };
};

function World(size, tileSize) {
    var width = size;
    var height = size;
    var tileSize = tileSize;

    var tiles = {};
    var getTileAt = function (x, y) {
        var key = KeyToString(x, y);
        if (_.has(tiles, key)) {
            //tile exists, return it
            return tiles[key];
        } else {
            //tile does not exist, create it!
            tiles[key] = basicTileGenerator(x, y);
            return tiles[key];
        }
    };

    return {
        width: width,
        height: height,
        getTileAt: getTileAt
    };
};