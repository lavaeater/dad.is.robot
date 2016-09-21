function StoryStep(title) {
    var self = this;
    return {

    };
};

function QuestEngine() {
    var self = this;
    return {

    };
};

function Encounter(key, type, title) {
    return {
        key: key,
        type: type,
        title: title
    }
};

function EncounterGenerator() {
    var self = this;
    self.random = function (max, min) {
        return Math.floor(Math.random() * (max - min + 1) + min);
    };
    var generateEncounter = function (key) {
        var seed = self.random(100,1);
        if(seed <= 85) {
            return new Encounter(key, 'empty', 'Nothing happening here');
        }
        if(seed <= 100) {
            return new Encounter(key, 'ruin', 'There is a ruin down there. Investigate?');
        }
    };
    return {
        generate: generateEncounter
    };
};

function EncounterEngine() {
    var self = this;
    var encounters = {}; //Add generated encounters here, if of the permanent kind -- or always?

    var encounterGenerator = new EncounterGenerator();

    self.getEncounter = function (tile) {
        //Extend the object with encounter? Nah, save 'em here, for now
        if (!_.has(encounters, tile.key)) {
            encounters[tile.key] = encounterGenerator.generate(tile.key);
        }
        //Evaluate possibility of adding encounter if encounter was empty
        return encounters[tile.key];
    }

    return {
        getEncounter: self.getEncounter
    };
};