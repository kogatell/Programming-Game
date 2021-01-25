import("array")

function preorder(node)
  function inner(node, arr) 
    if (node == null) then 
      return arr 
	  end
    array.append(arr, node.value)
    inner(node.left, arr)
    inner(node.right, arr)
    return arr
  end

  return inner(node, {})
end

return preorder