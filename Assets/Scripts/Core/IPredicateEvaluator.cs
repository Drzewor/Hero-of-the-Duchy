using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPredicateEvaluator
{
    bool? Evaluate(predicateName predicate, string[] parameters);
}
