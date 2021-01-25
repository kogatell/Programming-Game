const testCase = require('../test_fn.js');

var twoSum = function (nums, target) {
  const sums = {};
  for (let i = 0; i < nums.length; i++) {
    const t = target - nums[i];
    if (t in sums) {
      return [i, sums[t]].sort((a, b) => a - b);
    }
    sums[nums[i]] = i;
  }
  return [0, 0];
};

// Testcase 1
testCase([[2, 7, 11, 15], 9], twoSum);
testCase([[2, 7, 11, 15, 17, 19, 21], 15 + 17], twoSum);
testCase([[2, 4], 6], twoSum);
testCase([[2, 4, 2, 1, 0, 1], 1], twoSum);
testCase([[2, 4, 2, 1, 0, 1], 2], twoSum);
testCase([[2, 4, 2, 1, 0, 1], 3], twoSum);

// return function(arr, target)
// 	sums = table()
// 	for i=0, #arr do
// 		t = target - arr[i]
// 		if (sums[t] ~= null) then
// 			value = {i, sums[t]}
// 			import("array")
// 			array.sort(value, array.ascendant)
// 			return value
// 		end
// 		sums[arr[i]] = i
// 	end
// 	return {0, 0}
// end
