import("math")
function longestNonRepeating(str) 
  memo = table();
  maxLen = 0;
  endV = 0;
  start = 0;
  while (start < #str and endV < #str) do
    if (not memo[str[endV]]) then
      memo[str[endV]] = true
      endV = endV + 1
    else
      memo[str[start]] = false;
      start = start + 1;
    end
    maxLen = math.max(maxLen, endV - start);
  end
  return maxLen;
end

return longestNonRepeating