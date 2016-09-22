function Random(max, min) {
    return Math.floor(Math.random() * (max - min + 1) + min);
};

function StoryStep(index, title, stepKey, x, y, stepFinished) {
    var self = this;
    self.index = index;
    self.title = title;
    self.stepKey = stepKey;
    self.stepFinished = stepFinished;
    self.x = x;
    self.y = y;
    return {
        index: self.index,
        title: self.title,
        stepKey: self.stepKey,
        stepFinished: self.stepFinished,
        x: self.x,
        y: self.y
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

function QuestEngine() {
    var self = this;

    var generateTitle = function() {
        var seed = Random(3,1);
        switch(seed) {
            case 1: 
            return 'En förbipasserande ser din robot och berättar att hen har hört rykten om ett skrotupplag med delar';
            case 2: 
            return 'En resande handelsman undrar om du kan leta efter en Gobrofank åt honom?';
            case 3: 
            return 'Min dotter blev bortrövad av rövare. Kan du leta efter henne?';
        }
    };

    var stepGenerator = function(index) {
        var randomX = Random(50,1);
        var randomY = Random(50,1);
        var key = KeyToString(randomX, randomY);
        return new Encounter(key, 'storystep', 'Go to ' + key);
    };

    var generate = function (key) {
        var numberOfSteps = Random(5,1);
        var steps = [];
        for(i = 0; i < numberOfSteps; i++) {
            
            steps.push(stepGenerator(i))
        }
        return new Encounter(key, 'quest', generateTitle(), steps);
    };

    return {
        generate: generate
    };
};


function EncounterGenerator() {
    var self = this;

    var questEngine = new QuestEngine();

    var generateEncounter = function (key) {
        var seed = Random(100, 1);
        if (seed <= 100) {
            return questEngine.generate(key);
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
            var encounter = encounterGenerator.generate(tile.key);
            encounters[tile.key] = encounter;
        }
        //Evaluate possibility of adding encounter if encounter was empty
        return encounters[tile.key];
    }

    return {
        getEncounter: self.getEncounter
    };
};