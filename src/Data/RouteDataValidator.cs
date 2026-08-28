using System;
using System.Collections.Generic;
using System.Reflection;

namespace HallownestWayfinder
{
    public static class RouteDataValidator
    {
        private const BindingFlags PlayerDataMembers =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public static IReadOnlyList<string> Validate(IReadOnlyList<RoutePlan> routes)
        {
            List<string> errors = new List<string>();
            HashSet<string> checkedFields = new HashSet<string>(StringComparer.Ordinal);
            if (routes == null) return errors;

            foreach (RoutePlan route in routes)
            {
                if (route?.Steps == null) continue;
                foreach (RouteStep step in route.Steps)
                {
                    if (step == null) continue;
                    RouteCompletion completion = step.Completion;
                    Check(errors, checkedFields, route, step,
                        completion.PlayerBool, typeof(bool));
                    CheckAll(errors, checkedFields, route, step,
                        completion.AllPlayerBools, typeof(bool));
                    CheckAll(errors, checkedFields, route, step,
                        completion.AnyPlayerBools, typeof(bool));
                    Check(errors, checkedFields, route, step,
                        completion.PlayerInt, typeof(int));
                    CheckAll(errors, checkedFields, route, step,
                        completion.PlayerIntSum, typeof(int));

                    if (step.Prerequisites == null) continue;
                    foreach (PlayerDataPrerequisite[] alternative in step.Prerequisites)
                    {
                        if (alternative == null) continue;
                        foreach (PlayerDataPrerequisite condition in alternative)
                        {
                            if (condition == null) continue;
                            Check(errors, checkedFields, route, step,
                                condition.PlayerBool, typeof(bool));
                            Check(errors, checkedFields, route, step,
                                condition.PlayerInt, typeof(int));
                        }
                    }
                }
            }
            return errors;
        }

        private static void CheckAll(List<string> errors, HashSet<string> checkedFields,
            RoutePlan route, RouteStep step, string[]? fields, Type expectedType)
        {
            if (fields == null) return;
            foreach (string field in fields)
                Check(errors, checkedFields, route, step, field, expectedType);
        }

        private static void Check(List<string> errors, HashSet<string> checkedFields,
            RoutePlan route, RouteStep step, string? field, Type expectedType)
        {
            if (string.IsNullOrEmpty(field)) return;
            string key = step.Id + "\n" + expectedType.FullName + "\n" + field;
            if (!checkedFields.Add(key)) return;

            FieldInfo playerField = typeof(PlayerData).GetField(field, PlayerDataMembers);
            PropertyInfo playerProperty = typeof(PlayerData).GetProperty(field, PlayerDataMembers);
            Type? actualType = playerField?.FieldType ?? playerProperty?.PropertyType;
            if (actualType == expectedType) return;

            string actual = actualType == null ? "missing" : actualType.Name;
            errors.Add("Route '" + route.Id + "', step '" + step.Id +
                "': PlayerData." + field + " should be " + expectedType.Name +
                ", but is " + actual + ".");
        }
    }
}
