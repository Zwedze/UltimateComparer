# UltimateComparer


How to use it ?

EquitabilityItem<Car> result = new ValidationEquitableComparer<Car>().PropertiesToCheck(x => x.Price).EqualsValidation(car1, car2);

result.AreEquals :
  True if both items have the same value for the properties defined
  False if at least one value of the provided properties is not the same
  
result.DifferentialExpressions :
  Contains all expressions that triggered a not equals actions
