function RandomIntFromInterval(min, max) {
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

function SubTileGenerator(godds, fodds, dodds, wodds, modds, sodds) {
    var self = this;
    self.min = 1;
    self.grassOdds = godds;
    self.forestOdds = godds+fodds;
    self.desertOdds = godds+fodds+dodds;
    self.waterOdds = godds+fodds+dodds+wodds;
    self.mountainOdds = godds+fodds+dodds+wodds+modds;
    self.siteOdds = godds+fodds+dodds+wodds+modds+sodds;
    self.max = self.siteOdds;
    
    self.random = function() {
        return Math.floor(Math.random() * (self.max - self.min + 1) + self.min);
    };

    var generate = function(x, y) {
        var seed = self.random();
        var type = 'g';
        //Order is important, below. less than will evaluate to the lowest odds (0-50% fer instance)
        if(seed <= self.siteOdds) {
            type = 's';
        }
        if(seed <= self.mountainOdds) {
            type = 'm';
        }
        if(seed <= self.waterOdds) {
            type = 'w';
        }
        if(seed <= self.desertOdds) {
            type = 'd';
        }
        if(seed <= self.forestOdds) {
            type = 'f';
        }
        if(seed <= self.grassOdds) {
            type = 'g';
        }
        return new SubTile(x, y, type);
    };

    return {
        generate: generate
    };
};

var grassGenerator = function (x, y) {
    var type = '';
    var randomSeed = RandomIntFromInterval(1, 10);

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
    var randomSeed = RandomIntFromInterval(1, 10);

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
    var randomSeed = RandomIntFromInterval(1, 10);

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
    var randomSeed = RandomIntFromInterval(1, 10);

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
    var randomSeed = RandomIntFromInterval(1, 10);

    if (randomSeed <= 7) {
        type = 'd';
    } else if (7 < randomSeed && randomSeed <= 9) {
        type = 'm';
    } else if (9 < randomSeed && randomSeed <= 10) {
        type = 'w';
    }
    return new SubTile(x, y, type);
};

var Generators = {
    g: new SubTileGenerator(70, 10, 5, 10, 10,3),
    w: new SubTileGenerator(10,3,1,80,10,3),
    d: new SubTileGenerator(5,5,80,1,15,5),
    f: new SubTileGenerator(15,80,1,5,10,5),
    m: new SubTileGenerator(10,10,10,5,80,5)
};