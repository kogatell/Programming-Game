function permute(nums)
  res = {}
  backtracking(res, nums, {}, table())
  return res
end

import("array")
function backtracking(res, nums, current, visited)
  if #current == #nums then
    array.append(res, current[0 .. #current])	
	return
  end
  for i=0, #nums do
    if visited[i] == null or visited[i] == false then
      visited[i] = true
      array.append(current, nums[i])
	  backtracking(res, nums, current, visited);
	  array.pop(current)
      visited[i] = false
    end
  end
end
return permute