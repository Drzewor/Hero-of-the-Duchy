using System.Collections;
using System.Collections.Generic;

namespace RPG.Core
{
    public interface IPredicateEvaluator
    {
        bool? Evaluate(predicateName predicate, string[] parameters);
    }
}

