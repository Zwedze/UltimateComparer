using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UltimateComparer.Interface;
using UltimateComparer.Models;

namespace UltimateComparer
{
    public class ValidationEquitableComparer<T> : IValidationEquitable<T> where T : class
    {
        private readonly List<Expression<Func<T, object>>> _primaryKeysExpressions;
        private readonly List<Expression<Func<T, object>>> _propertiesToCheckExpressions;

        public ValidationEquitableComparer()
        {
            _primaryKeysExpressions = new List<Expression<Func<T, object>>>();
            _propertiesToCheckExpressions = new List<Expression<Func<T, object>>>();
        }

        public EquitabilityItem<T> EqualsValidation(T source, T target)
        {
            List<Expression<Func<T, object>>> validationExpressions = GetNotEqualsExpressions(source, target).ToList();
            EquitabilityItem<T> equalityItem = new EquitabilityItem<T>(source, target, validationExpressions,
                !validationExpressions.Any());

            if (source != null && target == null)
            {
                equalityItem.State = State.Removed;
                return equalityItem;
            }
            if (source == null && target != null)
            {
                equalityItem.State = State.Added;
            }

            equalityItem.State = State.Modified;
            return equalityItem;
        }

        private IEnumerable<Expression<Func<T, object>>> GetNotEqualsExpressions(T source, T target)
        {
            foreach (Expression<Func<T, object>> property in _primaryKeysExpressions)
            {
                Func<T, object> compiledProperty = property.Compile();
                object sourceValue = compiledProperty.Invoke(source);
                object targetValue = compiledProperty.Invoke(target);

                if (!sourceValue.Equals(targetValue))
                {
                    yield break;
                }
            }

            foreach (Expression<Func<T, object>> property in _propertiesToCheckExpressions)
            {
                Func<T, object> compiledProperty = property.Compile();
                object sourceValue = compiledProperty.Invoke(source);
                object targetValue = compiledProperty.Invoke(target);

                if ((sourceValue == null && targetValue == null))
                {
                    continue;
                }
                if (sourceValue == null || targetValue == null)
                {
                    yield return property;
                }
                else
                {
                    if (!sourceValue.Equals(targetValue))
                    {
                        yield return property;
                    }
                }
            }
        }

        /// <summary>
        ///     Defines which properties are primary keys to perform the comparaison. Two instances with different primary key
        ///     values will not be considered as not equals.
        /// </summary>
        public IValidationEquitable<T> PrimaryKeys(params Expression<Func<T, object>>[] primaryKeysDefinitions)
        {
            _primaryKeysExpressions.Clear();
            _primaryKeysExpressions.AddRange(primaryKeysDefinitions);

            return this;
        }

        public IValidationEquitable<T> PropertiesToCheck(params Expression<Func<T, object>>[] propertiesToCheck)
        {
            _propertiesToCheckExpressions.Clear();
            _propertiesToCheckExpressions.AddRange(propertiesToCheck);

            return this;
        }
    }
}