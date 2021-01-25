const testCase = require('../test_ds.js');

function push(t, value) {
  t.array.push(value);
  return value;
}

function pop(t) {
  return t.array.pop();
}

function newStack(arr) {
  return {
    array: arr,
    pop,
    push,
  };
}
console.log(testCase);
testCase(
  [
    ['push', [1]],
    ['push', [2]],
    ['push', [3]],
    ['push', [4]],
    ['pop', []],
    ['pop', []],
    ['pop', []],
    ['pop', []],
  ],
  [[]],
  newStack
);

testCase(
  [
    ['push', [1]],
    ['push', [2]],
    ['push', [3]],
    ['push', [4]],
    ['pop', []],
    ['push', [4]],
    ['pop', []],
    ['pop', []],
    ['pop', []],
    ['push', [4]],
    ['pop', []],
    ['pop', []],
    ['array', null],
    ['push', [4]],
    ['array', null],
  ],
  [[]],
  newStack
);

// Lua solution

// import("array")

// function pop(self)
//     return array.pop(self.array);
//     end

// function push(self, element)
//     array.append(self.array, element)
// 	return element
//  end

// function new_stack(arr)
//     return { array=arr, pop=pop, push=push }
// end

// return new_stack;
