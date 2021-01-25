const testCase = require('../test_fn.js');

/**
 * @param {number[]} nums
 * @return {number[][]}
 */
var permute = function (nums) {
  const res = [];
  backtracking(res, nums, [], {});
  return res;
};

function backtracking(res, nums, current, visited) {
  if (current.length === nums.length) {
    res.push([...current]);
    return;
  }
  for (let i = 0; i < nums.length; i++) {
    if (visited[i]) continue;
    visited[i] = true;
    current.push(nums[i]);
    backtracking(res, nums, current, visited);
    current.pop();
    visited[i] = false;
  }
}

testCase([[1, 2, 3]], permute);
testCase([[0, 1]], permute);
testCase([[0, 1, 9, 2]], permute);
testCase([[0, 1, 9, 2, 5]], permute);
testCase([[0, 8, 2]], permute);
