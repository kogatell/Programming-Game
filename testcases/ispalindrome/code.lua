import("string")

function is_palindrome(s) 
  s = string.lowercase(s)
  i = 0
  j = #s - 1
  while i < j do
    if s[i] ~= s[j] and is_good(s[i]) and is_good(s[j]) then 
      return false
    elseif not is_good(s[i]) then
      j = j+1
    elseif not is_good(s[j]) then
      i = i - 1
    end
    i = i + 1
    j = j - 1
  end
  return true
end

a = string.char_code("a")
z = string.char_code("z")
n0 = string.char_code("0")
n9 = string.char_code("9")

function is_good(c) 
  c = string.char_code(c)
  return (c >= a and c <= z) or (c >= n0 and c <= n9)
end



return is_palindrome