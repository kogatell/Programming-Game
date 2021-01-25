module.exports = function (inputFunction, func) {
  const result = func(...inputFunction);
  const jsonData = { data: result };
  const input = { data: inputFunction };
  console.log(JSON.stringify(jsonData));
  console.log(JSON.stringify(input));
};
