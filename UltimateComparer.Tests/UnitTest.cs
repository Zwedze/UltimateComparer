using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UltimateComparer.Models;
using UltimateComparer.Tests.Models;

namespace UltimateComparer.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void TestNotNullReferences()
        {
            Car car1 = new Car
            {
                Id = 1,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            Car car2 = new Car
            {
                Id = 2,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            EquitabilityItem<Car> result =
                new ValidationEquitableComparer<Car>().PrimaryKeys(x => x.ModelName)
                    .PropertiesToCheck(x => x.Price)
                    .EqualsValidation(car1, car2);

            Assert.AreEqual(true, result.AreEquals);

            car2.Price = 10;

            result = new ValidationEquitableComparer<Car>().PrimaryKeys(x => x.ModelName)
                .PropertiesToCheck(x => x.Price)
                .EqualsValidation(car1, car2);
            Assert.AreEqual(false, result.AreEquals);
            Assert.AreEqual("Price", GetExpressionName(result.DifferentialExpressions.First()));
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TestNullReferences()
        {
            Car car1 = new Car
            {
                Id = 1,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            Car car2 = null;

            EquitabilityItem<Car> result =
                new ValidationEquitableComparer<Car>().PrimaryKeys(x => x.ModelName)
                    .PropertiesToCheck(x => x.Price)
                    .EqualsValidation(car1, car2);

            Assert.AreEqual(true, result.AreEquals);
        }

        [TestMethod]
        public void TestOneFieldValue()
        {
            Car car1 = new Car
            {
                Id = 1,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            Car car2 = new Car
            {
                Id = 2,
                Color = Color.Blue,
                ModelName = "Fiat 500",
                Price = 30000
            };

            EquitabilityItem<Car> result =
                new ValidationEquitableComparer<Car>().PrimaryKeys(x => x.ModelName)
                    .PropertiesToCheck(x => x.Color)
                    .EqualsValidation(car1, car2);

            Assert.AreEqual(false, result.AreEquals);
        }

        [TestMethod]
        public void TestAllExceptOneFieldValue()
        {
            Car car1 = new Car
            {
                Id = 1,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            Car car2 = new Car
            {
                Id = 2,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            EquitabilityItem<Car> result =
                new ValidationEquitableComparer<Car>()
                    .PropertiesToCheck(x => x.Price, x => x.Color, x => x.ModelName)
                    .EqualsValidation(car1, car2);

            Assert.AreEqual(false, result.AreEquals);
        }

        [TestMethod]
        public void TestBidon()
        {
            Car car1 = new Car
            {
                Id = 1,
                Color = Color.Blue,
                ModelName = "Twingo",
                Price = 25000
            };

            Car car2 = new Car
            {
                Id = 2,
                Color = Color.Blue,
                ModelName = "Pigeon",
                Price = 25000
            };

            EquitabilityItem<Car> result =
                new ValidationEquitableComparer<Car>().PrimaryKeys(x => x.ModelName)
                    .PropertiesToCheck(x => x.ModelName)
                    .EqualsValidation(car1, car2);

            Assert.AreEqual(true, result.AreEquals);
        }

        private static string GetExpressionName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression)
            {
                return (expression.Body as MemberExpression).Member.Name;
            }
            if (expression.Body is UnaryExpression)
            {
                UnaryExpression body = expression.Body as UnaryExpression;
                if (body.Operand is MemberExpression)
                {
                    return (body.Operand as MemberExpression).Member.Name;
                }
            }

            return "unknown_name";
        }
    }
}