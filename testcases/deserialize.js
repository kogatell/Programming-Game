function TreeNode(value) {
  return {
    left: null,
    right: null,
    value,
  };
}

var deserialize = function (data) {
  if (data === '') {
    return null;
  }
  var values = data;
  var root = TreeNode(parseInt(values[0]));
  var queue = [root];
  for (var i = 1; i < values.length; i++) {
    var parent = queue.shift();

    if (values[i] !== 'null') {
      var left = TreeNode(parseInt(values[i]));
      parent.left = left;
      queue.push(left);
    }
    if (values[++i] !== 'null' && i !== values.length) {
      var right = TreeNode(parseInt(values[i]));
      parent.right = right;
      queue.push(right);
    }
  }

  return root;
};

module.exports = deserialize;
