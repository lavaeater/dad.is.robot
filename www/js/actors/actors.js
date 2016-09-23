function Attributes(strength, agility, intelligence) {
    return {
        strength: strength,
        agility: agility,
        intelligence: intelligence
    };
}

function Actor(name, attributes, actions) {
    return _.assign({ name: name }, attributes, actions);
}

function Action(name, type, modifiers, props) {
    return {
        name: name,
        type: type,
        modifiers: modifiers,
        props: props
    };
}

function CommonActions() {
    var actions = {};
    return actions;
}

function Player(name) {
    //Just returns a new player object with some attributes and stuffs;

    var attrs = new Attributes(10, 10, 10);
    var actions = []; // a list of possible actions for this character
    actions.push(new Action('Stab', 'attack', { attack: 1 }, {}));
    actions.push(new Action('Slice', 'attack', { attack: 2, defense: -1, damage: 1 }, {}));
    actions.push(new Action('Defend', 'defense', { defense: 2, attack: -1 }, {}));
    actions.push(new Action('Run away', 'exit', { defense: -1 }, { exclusive: true }));

    return new Actor(name, attrs, actions);
}

function Radiyote() {
    var attrs = new Attributes(15, 12, 3);
    var actions = [];
    actions.push(new Action('Bite', 'attack', { attack: 2, defense: -1, damage: 1 }));
    actions.push(new Action('Claw', 'attack', { attack: 1, damage: -2 }));
    actions.push(new Action('Run away', 'exit', { defense: -1 }, { exclusive: true }));
}