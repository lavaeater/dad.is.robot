var _ = require('lodash');

var tileTypes = ['g', 'w', 'd', 'm', 'f'];
var tileGenerators = [
    { g: grassGenerator },
    { w: waterGenerator },
    { d: desertGenerator },
    { m: mountainGenerator },
    { f: forestGenerator }
];

function KeyToString(x, y) {
    return x.toString() + ':' + y.toString();
};

function randomIntFromInterval(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
};

function basicTileGenerator(x, y) {
    var randomSeed = randomIntFromInterval(1, 10);
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
    return new Tile(x, y, type, tileGenerators[type]);
};

var grassGenerator = function (x, y) {
    var type = '';
    var randomSeed = randomIntFromInterval(1, 10);

    if (randomSeed <= 5) {
        type = 'g';
    } else if (5 < randomSeed && randomSeed <= 7) {
        type = 'f';
    } else if (7 < randomSeed && randomSeed <= 9) {
        type = 'w';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'm';
    }
    return new SubTile(x, y, type);
};

var forestGenerator = function (x, y) {
    var type = '';
    var randomSeed = randomIntFromInterval(1, 10);

    if (randomSeed <= 5) {
        type = 'f';
    } else if (5 < randomSeed && randomSeed <= 7) {
        type = 'm';
    } else if (7 < randomSeed && randomSeed <= 9) {
        type = 'w';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'g';
    }
    return new SubTile(x, y, type);
};

var mountainGenerator = function (x, y) {
    var type = '';
    var randomSeed = randomIntFromInterval(1, 10);

    if (randomSeed <= 6) {
        type = 'm';
    } else if (6 < randomSeed && randomSeed <= 8) {
        type = 'f';
    } else if (8 < randomSeed && randomSeed <= 9) {
        type = 'w';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'd';
    }
    return new SubTile(x, y, type);
};

var waterGenerator = function (x, y) {
    var type = '';
    var randomSeed = randomIntFromInterval(1, 10);

    if (randomSeed <= 8) {
        type = 'w';
    } else if (8 < randomSeed && randomSeed <= 9) {
        type = 'd';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'm';
    return new SubTile(x, y, type);
};

var desertGenerator = function (x, y) {
    var type = '';
    var randomSeed = randomIntFromInterval(1, 10);

    if (randomSeed <= 7) {
        type = 'd';
    } else if (7 < randomSeed && randomSeed <= 9) {
        type = 'm';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'w';
    }
    return new SubTile(x, y, type);
};

function SubTile(x, y, type) {
    var self = this;
    var key = KeyToString(x, y);
    var type = type;
    var x = x;
    var y = y;
    return {
        key: key,
        x: x,
        y: y,
        type: type
    };
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
    }

    return {
        width: width,
        height: height,
        getTileAt: getTileAt
    };
};

module.exports = World;