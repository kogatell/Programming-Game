module.exports = function (actions, constructor, fn) {
  const pipeline = { data: [] };
  const ds = fn(...constructor);
  actions.forEach(([name, fnParams]) => {
    let returnVal = null;
    if (fnParams) {
      returnVal = ds[name](ds, ...fnParams);
    } else {
      returnVal = ds[name];
    }
    pipeline.data.push([name, fnParams, returnVal]);
  });
  const jsonData = { data: null };
  const inputData = { data: constructor };
  console.log('jsonData:', JSON.stringify(jsonData));
  console.log('inputData:', JSON.stringify(inputData));
  console.log('pipeline:', JSON.stringify(pipeline));
};
