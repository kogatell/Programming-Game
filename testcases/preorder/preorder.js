const testCase = require('../test_fn.js');
const deserialize = require('../deserialize.js');

function preorder(node, result = []) {
  if (!node) return result;
  result.push(node.value);
  preorder(node.left, result);
  preorder(node.right, result);
  return result;
}

testCase([deserialize([1, 2, 3, 4, 5, 6, 7])], preorder);
