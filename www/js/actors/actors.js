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
        props:props
    };
}

function Player(name) {
    //Just returns a new player object with some attributes and stuffs;

    var attrs = new Attributes(10, 10, 10);
    var actions = []; // a list of possible actions for this character
    actions.push(new Action('Stab','attack', {attack: 1}, {}));
    actions.push(new Action('Defend', 'defense', {defense: 2, attack: -1 }, {}));
    actions.push(new Action('Run away', 'defense', {defense: -1 }, {exclusive:true}));

    return new Actor(name, attrs, actions);
}