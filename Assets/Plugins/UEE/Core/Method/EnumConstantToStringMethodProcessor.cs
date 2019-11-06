using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UniEnumExtension
{
    public sealed class EnumConstantToStringMethodProcessor
        : IMethodProcessor
    {
        public byte Stage => 66;
        public bool ShouldProcess(TypeDefinition typeDefinition) => !typeDefinition.IsEnum;

        public void Process(ModuleDefinition systemModuleDefinition, MethodDefinition methodDefinition)
        {
            var instructions = methodDefinition.Body.Instructions;
            var moduleDefinition = methodDefinition.Module;
            using (ScopedProcessor processor = methodDefinition.Body.GetILProcessor())
            {
                for (var index = instructions.Count - 1; index >= 3; index--)
                {
                    TryProcessEachInstruction(instructions, ref index, processor, moduleDefinition, systemModuleDefinition);
                }
            }
        }

        private static void TryProcessEachInstruction(IList<Instruction> collection, ref int index, ScopedProcessor processor, ModuleDefinition moduleDefinition, ModuleDefinition systemModuleDefinition)
        {
            var constrainedInstruction = collection[index];
            if (!(constrainedInstruction?.Operand is TypeReference enumTypeReference) || constrainedInstruction.OpCode.Code != Code.Constrained || !enumTypeReference.TryToDefinition(out var enumTypeDefinition) || enumTypeDefinition.HasMethods)
            {
                return;
            }
            var callVirtualInstruction = constrainedInstruction.Next;
            if (!(callVirtualInstruction?.Operand is MethodReference baseToStringMethodReference) || callVirtualInstruction.OpCode.Code != Code.Callvirt || baseToStringMethodReference.FullName != "System.String System.Object::ToString()")
            {
                return;
            }
            var ldlocasInstruction = constrainedInstruction.Previous;
            if (ldlocasInstruction is null || (ldlocasInstruction.OpCode.Code != Code.Ldloca_S && ldlocasInstruction.OpCode.Code != Code.Ldloca) || !(ldlocasInstruction.Operand is VariableDefinition loadLocalVariableDefinition))
            {
                return;
            }
            var stlocInstruction = ldlocasInstruction.Previous;
            if (stlocInstruction is null)
            {
                return;
            }
            switch (stlocInstruction.OpCode.Code)
            {
                case Code.Stloc_0:
                    if (loadLocalVariableDefinition.Index != 0) return;
                    break;
                case Code.Stloc_1:
                    if (loadLocalVariableDefinition.Index != 1) return;
                    break;
                case Code.Stloc_2:
                    if (loadLocalVariableDefinition.Index != 2) return;
                    break;
                case Code.Stloc_3:
                    if (loadLocalVariableDefinition.Index != 3) return;
                    break;
                case Code.Stloc_S:
                case Code.Stloc:
                    if (!(stlocInstruction.Operand is VariableReference storeVariableReference) || !storeVariableReference.TryToDefinition(out var storeVariableDefinition) || loadLocalVariableDefinition.Index != storeVariableDefinition.Index) return;
                    break;
            }
            switch (enumTypeDefinition.Fields[0].FieldType.Name)
            {
                case "UInt64":
                {
                    var instructionPrevious = stlocInstruction.Previous;
                    if (instructionPrevious == null) return;
                    if (instructionPrevious.OpCode.Code == Code.Conv_U8)
                    {
                        var instructionPreviousPrevious = instructionPrevious.Previous;
                        int value;
                        switch (instructionPreviousPrevious.OpCode.Code)
                        {
                            case Code.Ldc_I4_0:
                                value = 0;
                                break;
                            case Code.Ldc_I4_1:
                                value = 1;
                                break;
                            case Code.Ldc_I4_2:
                                value = 2;
                                break;
                            case Code.Ldc_I4_3:
                                value = 3;
                                break;
                            case Code.Ldc_I4_4:
                                value = 4;
                                break;
                            case Code.Ldc_I4_5:
                                value = 5;
                                break;
                            case Code.Ldc_I4_6:
                                value = 6;
                                break;
                            case Code.Ldc_I4_7:
                                value = 7;
                                break;
                            case Code.Ldc_I4_8:
                                value = 8;
                                break;
                            case Code.Ldc_I4_M1:
                                value = -1;
                                break;
                            case Code.Ldc_I4_S:
                                value = (sbyte) instructionPreviousPrevious.Operand;
                                break;
                            case Code.Ldc_I4:
                                value = (int) instructionPreviousPrevious.Operand;
                                break;
                        }
                    }
                    else if (instructionPrevious.OpCode.Code == Code.Ldc_I8)
                    {
                    }
                    else return;
                }
                    break;
                case "Int64":
                    break;
                default:
                    break;
            }
        }
    }
}