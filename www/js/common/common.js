function KeyToString(x, y) {
    return x.toString() + ':' + y.toString();
}

function Random(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}