const parseNumberWithDecimal = (value: string): number => {
  const integer = value.split('.')[0];
  let decimals = value.split('.')[1];
  if (decimals.length > 2) {
    decimals = decimals.substring(0, 2);
  }
  return parseFloat(parseFloat(`${integer}.${decimals}`).toFixed(2));
}

export const parseNumber = (value: string): number => {
  return value.indexOf('.') === -1
    ? parseInt(value)
    : parseNumberWithDecimal(value)
}