const testCase = require('../test_fn.js');
var isPalindrome = function (s) {
  s = s.toLowerCase();
  // s = s.split('').filter(e => isGood(e)).join('');
  for (let i = 0, j = s.length - 1; i < j; i++, j--) {
    if (s[i] !== s[j] && isGood(s[i]) && isGood(s[j])) return false;
    else if (!isGood(s[i])) {
      j++;
    } else if (!isGood(s[j])) {
      i--;
    }
  }
  return true;
};

function isGood(c) {
  return (c >= '0' && c <= '9') || (c >= 'a' && c <= 'z');
}

testCase(['A man, a plan, a canal: Panama'], isPalindrome);
testCase(['aaaaa'], isPalindrome);
testCase(['aaaaabbbb'], isPalindrome);
testCase(['race a car'], isPalindrome);
testCase(['race ecar'], isPalindrome);
testCase(['sssaaa'], isPalindrome);
testCase([''], isPalindrome);
testCase(['qwq'], isPalindrome);
