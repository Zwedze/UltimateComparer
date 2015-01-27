using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace UltimateComparer.Models
{
    public class EquitabilityItem<T> where T : class
    {
        public T Source { get; private set; }
        public T Target { get; private set; }
        public IList<Expression<Func<T, object>>> DifferentialExpressions { get; private set; }
        public State State { get; set; }
        public bool AreEquals { get; private set; }

        public EquitabilityItem(T source, T target, IList<Expression<Func<T, object>>> differentialExpressions, bool areEquals)
        {
            DifferentialExpressions = differentialExpressions;
            Target = target;
            Source = source;
            AreEquals = areEquals;
        }
    }

    public enum State
    {
        Modified = 0,
        Removed = 1,
        Added = 2
    }
}