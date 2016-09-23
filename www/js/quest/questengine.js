function Encounter(key, x, y, type, title) {
    return {
        key: key,
        x: x,
        y: y,
        type: type,
        title: title,
        nextStep: null
    };
}

function QuestEngine() {
    var self = this;

    var generateTitle = function () {
        var seed = Random(1, 3);
        switch (seed) {
            case 1:
                return 'En förbipasserande ser din robot och berättar att hen har hört rykten om ett skrotupplag med delar';
            case 2:
                return 'En resande handelsman undrar om du kan leta efter en Gobrofank åt honom?';
            case 3:
                return 'Min dotter blev bortrövad av rövare. Kan du leta efter henne?';
        }
    };

    var stepGenerator = function (x, y) {
        var randomX = x + Random(10, -10);
        var randomY = y + Random(10, -10);
        var key = KeyToString(randomX, randomY);
        return new Encounter(key, randomX, randomY, 'quest', 'Go to ' + key);
    };

    var generate = function (tile) {
        var numberOfSteps = Random(1, 5);
        var returnStep = new Encounter(tile.key, tile.x, tile.y, 'quest', generateTitle());
        var previousStep = returnStep;
        for (i = 0; i < numberOfSteps; i++) {
            //Set next step, then set previousStep to nextSTep so we can set NextSTep on it
            previousStep.nextStep = stepGenerator(previousStep.x, previousStep.y);
            previousStep = previousStep.nextStep;
        }
        return returnStep;
    };

    return {
        generate: generate
    };
};


function EncounterGenerator() {
    var self = this;

    var questEngine = new QuestEngine();

    var generateEncounter = function (tile) {
        var seed = Random(1, 100);
        if (seed <= 85) {
            return new Encounter(tile.key, tile.x, tile.y, 'empty', 'Nothing happening here');
        }
        if (seed <= 100) {
            return questEngine.generate(tile);
            //return new Encounter(tile.key, tile.x, tile.y, 'ruin', 'There is a ruin down there. Investigate?');
        }
    };
    return {
        generate: generateEncounter
    };
}

function FixStepsRecursive(encounter, collection) {
    if (encounter !== null && !_.has(collection, encounter.key)) {
        var encs = [];
        encs.push(encounter);
        collection[encounter.key] = encs;
        FixStepsRecursive(encounter.nextStep, collection);
    } else if(encounter !== null && _.has(collection, encounter.key)) {
        collection[encounter.key].push(encounter);
        FixStepsRecursive(encounter.nextStep, collection);
    }
}

function EncounterEngine() {
    var self = this;
    var encounters = {}; //Add generated encounters here, if of the permanent kind -- or always?

    var encounterGenerator = new EncounterGenerator();

    self.getEncounter = function (tile) {
        //Extend the object with encounter? Nah, save 'em here, for now
        //We have coords from tile, use 'em
        if (!_.has(encounters, tile.key)) {
            var encs = [];
            var encounter = encounterGenerator.generate(tile);
            if (encounter.type === 'quest') {
                FixStepsRecursive(encounter.nextStep, encounters);
                //Add child steps to the encounters!
            }
            encs.push(encounter);
            encounters[tile.key] = encs;
        }
        //Evaluate possibility of adding new encounter if encounter was empty
        return encounters[tile.key];
    };

    return {
        getEncounter: self.getEncounter
    };
}