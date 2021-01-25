import("array")

function valid_paren(s)
  paren_table = table()
  paren_table["("] = ")"
  paren_table["["] = "]"
  paren_table["{"] = "}"
  stack = {}
  for i=0, #s do
    if paren_table[s[i]] ~= null then
      array.append(stack, s[i])
    else 
      element = array.pop(stack)
      if element == null then
        return false
      end
      if paren_table[element] ~= s[i] then
        return false
      end
    end
  end
  return #stack == 0
end

return valid_paren
