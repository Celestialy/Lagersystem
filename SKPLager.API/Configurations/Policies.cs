using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SKPLager.API.Configurations
{
    public static class Policies
    {
        public const string IsAtleastInventoryManager = nameof(IsAtleastInventoryManager);
        public const string IsAtleastInstructor = nameof(IsAtleastInstructor);
        public const string AllAccess = nameof(AllAccess);
    }
}
