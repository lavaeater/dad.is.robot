/*

Nu ska vi bygga en statemachine för strid

Vilka states har vi? Listan nedan kan förändras men man kan tänka sig ett start-state, t.ex.

start
pickmoves
combat
done

start kommer man till via någon funktion som tar emot en lista av protagonister och antagonister.
Från start kommer man till pickmoves i princip automatiskt
pickmoves börjar med protagonisterna och låter dem välja moves.

*/


function CombatRound() {

    return {

    };
}

function CombatSession(protagonist, antagonists) {
    var currentRound = 0;



    return {
        protagonist: protagonist,
        antagonists: antagonists,
        currentRound: currentRound
    };
}

function CombatViewModel() {
    var self = this;

    var player = new Player('Nausicae');

    var antagonists = [];
    antagonists.push(new Radiyote());

    var combatSession = new CombatSession(player, antagonists);

    var readyToPlay = ko.computed(function() {

    });

    var selectAction = function(actor, action) {

    };

    var playRound = function() {

    };

    return {
        protagonist: combatSession.protagonist,
        antagonists: combatSession.antagonists
    };
}