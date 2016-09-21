function Random(max, min) {
    return Math.floor(Math.random() * (max - min + 1) + min);
};

function StoryStep(title) {
    var self = this;
    return {
        title: title
    };
};

function QuestEngine() {
    var self = this;

    var generate = function () {
        var numberOfSteps = Random(5,1);
        var steps = [];
        for(i = 0; i < numberOfSteps; i++) {
            steps.push(new StoryStep())
        }
    };

    return {
        generate: generate
    };
};

function Encounter(key, type, title, storySteps) {
    return {
        key: key,
        type: type,
        title: title,
        storySteps: storySteps
    }
};

function EncounterGenerator() {
    var self = this;

    var questEngine = new QuestEngine();

    var generateEncounter = function (key) {
        var seed = self.random(100, 1);
        if (seed <= 10) {
            return new Encounter(key, 'quest', 'En förbipasserande ser din robot och berättar att hen har hört rykten om ett skrotupplag med delar');
        }
        if (seed <= 85) {
            return new Encounter(key, 'empty', 'Nothing happening here');
        }
        if (seed <= 100) {
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