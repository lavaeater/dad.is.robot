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

function CombatViewModel() {
    var self = this;
    self.onState = function(event, from, to) {
        console.log(event);
        console.log(from);
        console.log(to);
    };
    
    self.onEvent = function(event, from, to) {
        console.log(event);
        console.log(from);
        console.log(to);
    };
    
    self.fsm = StateMachine.create({
        initial: 'before',
        events: [
            { name: 'start', from: 'before', to: 'pickfriendly' },
            { name: 'friendlypicked', from: 'pickfriendly', to: 'pickenemy' },
            { name: 'enemypicked', from: 'pickenemy', to: 'resolvecombat' },
            { name: 'combatresolved', from: 'resolvecombat', to: 'pickfriendly' },
            { name: 'stop' }
        ],
        callbacks: [

        ]
    });

    var currentState = ko.computed(function() {
        return self.fsm.current;
    });

    return {
        currentState: currentState
    };
}