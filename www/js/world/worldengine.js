function basicTileGenerator(x, y) {
    var randomSeed = Random(1, 10);
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
}

function Tile(x, y, type, subTileGenerator) {
    var self = this;
    self.key = KeyToString(x, y);
    self.x = x;
    self.y = y;
    var size = 10;
    var subTiles = {};
    self.subTileGenerator = subTileGenerator;
    var subTilesGenerated = false;
    var map = [];
    var mapRendered = false;

    var globalXFromLocal = function(x) {
        return self.x * 10 + x; 
    };

    var globalYFromLocal = function(y) {
        return self.y * 10 + y; 
    };

    //Every subtile needs an actual coordinate!
    var generateSubTiles = function () {
        for (i = 0; i < size; i++) {
            for (j = 0; j < size; j++) {
                var subX = globalXFromLocal(j);
                var subY = globalYFromLocal(i);
                var tile = self.subTileGenerator.generate(subX, subY);
                subTiles[tile.key] = tile;
            }
        }
        subTilesGenerated = true;
    };

    var getMap = function () {
        if (!subTilesGenerated) {
            generateSubTiles();
        }

        if (!mapRendered) {
            for (i = 0; i < size; i++) {
                var row = [];
                for (j = 0; j < size; j++) {
                var subX = globalXFromLocal(j);
                var subY = globalYFromLocal(i);
                    var key = KeyToString(subX, subY);
                    row.push(subTiles[key].type);
                };
                map.push(row);
            };
            mapRendered = true;
        }
        return map;
    };

    return {
        key: self.key,
        x: self.x,
        y: self.y,
        type: type,
        subTiles: subTiles,
        getMap: getMap,
        generateSubTiles: generateSubTiles
    };
}

function World(size, tileSize) {
    var width = size;
    var height = size;

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
}