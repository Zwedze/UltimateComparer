using System;
using System.Linq.Expressions;
using UltimateComparer.Models;

namespace UltimateComparer.Interface
{
    public interface IValidationEquitable<T> where T : class
    {
        EquitabilityItem<T> EqualsValidation(T source, T target);
        IValidationEquitable<T> PrimaryKeys(params Expression<Func<T, object>>[] primaryKeysDefinitions);
        IValidationEquitable<T> PropertiesToCheck(params Expression<Func<T, object>>[] propertiesToCheck);
    }
}