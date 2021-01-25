const testCase = require('../test_fn.js');
var isValid = function (s) {
  const table = {
    '{': '}',
    '[': ']',
    '(': ')',
  };

  const stack = [];
  for (let i = 0; i < s.length; i++) {
    if (s[i] in table) {
      stack.push(s[i]);
      continue;
    }
    const el = stack.pop();
    if (!el) {
      return false;
    }
    if (table[el] !== s[i]) {
      return false;
    }
  }
  return stack.length <= 0;
};

testCase(['()()(())'], isValid);
testCase(['()()(())((('], isValid);
testCase(['()()(())((([{[]}])))[][][][][]()()((()))'], isValid);
testCase(['()()(())((([{[]}])))[][][][][]()()((())'], isValid);
testCase(['()()(())((([{[]}])))][][][][]()()((())'], isValid);
testCase(['(())[()(())((([{[]}])))[][][][][]()()((()))]'], isValid);
