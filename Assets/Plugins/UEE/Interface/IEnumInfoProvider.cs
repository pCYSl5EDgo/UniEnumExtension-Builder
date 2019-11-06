using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

namespace UniEnumExtension
{
    public interface IEnumInfoProvider
    {
        bool TryGetInfo(TypeReference typeReference, out IEnumInfo info);
    }
}
