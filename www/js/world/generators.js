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

function SubTileGenerator(godds, fodds, dodds, wodds, modds) {
    var self = this;
    self.min = 1;
    self.grassOdds = godds;
    self.forestOdds = godds+fodds;
    self.desertOdds = godds+fodds+dodds;
    self.waterOdds = godds+fodds+dodds+wodds;
    self.mountainOdds = godds+fodds+dodds+wodds+modds;
    self.max = self.mountainOdds;
    
    self.random = function() {
        return Math.floor(Math.random() * (self.max - self.min + 1) + self.min);
    };

    var generate = function(x, y) {
        var seed = self.random();
        var type = 'g';
        //Order is important, below. less than will evaluate to the lowest odds (0-50% fer instance)
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

var Generators = {
    g: new SubTileGenerator(70, 10, 5, 10, 1),
    w: new SubTileGenerator(10,3,1,80,10),
    d: new SubTileGenerator(5,5,80,1,15),
    f: new SubTileGenerator(15,80,1,5,10),
    m: new SubTileGenerator(10,10,10,5,80)
};