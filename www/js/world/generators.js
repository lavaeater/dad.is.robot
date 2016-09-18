var KeyToString = require('./KeyToString');

function randomIntFromInterval(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
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
    }
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

function Generators() {
    this.g = grassGenerator;
    this.w = waterGenerator;
    this.d = desertGenerator;
    this.f = forestGenerator;
    this.m = mountainGenerator;
}

module.exports = new Generators();