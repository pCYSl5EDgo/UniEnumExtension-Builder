using System;
using Mono.Cecil;
using Mono.Cecil.Cil;
using UnityEngine;

namespace UniEnumExtension
{
    public static class DefinitionUtility
    {
        public static bool TryToDefinition(this TypeReference reference, out TypeDefinition definition)
        {
            if (reference.IsGenericParameter || reference.IsGenericInstance)
            {
                definition = null;
                return false;
            }
            definition = reference as TypeDefinition;
            if (!(definition is null))
            {
                return true;
            }
            try
            {
                definition = reference.Resolve();
                return !(definition is null);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }
        }

        public static bool TryToDefinition(this VariableReference reference, out VariableDefinition definition)
        {
            definition = reference as VariableDefinition;
            if (!(definition is null))
            {
                return true;
            }
            try
            {
                definition = reference.Resolve();
                return !(definition is null);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                return false;
            }
        }
    }
}
