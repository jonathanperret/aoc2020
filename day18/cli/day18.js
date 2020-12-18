const fs = require('fs');
const exprs = fs.readFileSync("input.txt", "utf-8").trim().split("\n");

// part 1
console.log(exprs.map(expr => {
    let expr_;
    while (expr != (expr_ = expr.replace(/(\d+) [+*] (\d+)|\((\d+)\)/, eval))) {
        // console.log(expr);
        expr = expr_;
    }
    return eval(expr);
}).reduce((a, b) => a + b));

// part 2
console.log(exprs.map(expr => {
    let expr_;
    while (expr != (expr_ = expr.replace(/(\d+) \+ (\d+)|(^|\()[^()+]+($|\))/, eval))) {
        //console.log(expr);
        expr = expr_;
    }
    return eval(expr);
}).reduce((a, b) => a + b));
