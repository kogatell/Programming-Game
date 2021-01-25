/**
 * @param {string} s
 * @return {number}
 */
var lengthOfLongestSubstring = function (str) {
  let memo = {};
  let maxLen = 0;
  let end = 0;
  let start = 0;
  while (start < str.length && end < str.length) {
    if (!memo[str[end]]) {
      memo[str[end]] = true;
      end++;
    } else {
      memo[str[start]] = false;
      start += 1;
    }
    maxLen = Math.max(maxLen, end - start);
  }
  return maxLen;
};

const testCase = require('../test_fn.js');

testCase(['abcabcbb'], lengthOfLongestSubstring);
testCase(['bbbbb'], lengthOfLongestSubstring);
testCase(['pwwkew'], lengthOfLongestSubstring);
testCase([''], lengthOfLongestSubstring);
testCase(['aaabbsbscjkscxcmasdkjqwpwjieireiruqu'], lengthOfLongestSubstring);
testCase(['pqpqpqpwoeoeiirkxmxmaa'], lengthOfLongestSubstring);
testCase(['pppppwsxqwe'], lengthOfLongestSubstring);
