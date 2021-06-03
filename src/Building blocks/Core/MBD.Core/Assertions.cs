using System;
using System.Text.RegularExpressions;
using MBD.Core.DomainObjects;

namespace MBD.Core
{
    public static class Assertions
    {
        public static void IsNotEmpty(Guid value, string message)
        {
            if (value.Equals(Guid.Empty))
            {
                throw new DomainException(message);
            }
        }

        public static void IsNotEmpty(Guid? value, string message)
        {
            if (value == null) return;

            if (value.Equals(Guid.Empty))
            {
                throw new DomainException(message);
            }
        }

        public static void IsTrue(bool value, string message)
        {
            if (!value)
            {
                throw new DomainException(message);
            }
        }

        public static void IsFalse(bool value, string message)
        {
            if (value)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(DateTime value, DateTime comparer, string message)
        {
            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(DateTime? value, DateTime comparer, string message)
        {
            if (value == null) return;

            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(DateTime value, DateTime comparer, string message)
        {
            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(DateTime? value, DateTime comparer, string message)
        {
            if (value == null) return;

            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(DateTime value, DateTime comparer, string message)
        {
            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(DateTime? value, DateTime comparer, string message)
        {
            if (value == null) return;

            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(DateTime value, DateTime comparer, string message)
        {
            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(DateTime? value, DateTime comparer, string message)
        {
            if (value == null) return;

            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(DateTime value, DateTime from, DateTime to, string message)
        {
            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(DateTime? value, DateTime from, DateTime to, string message)
        {
            if (value == null) return;

            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(decimal value, decimal comparer, string message)
        {
            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(decimal? value, decimal comparer, string message)
        {
            if (!value.HasValue) return;

            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(decimal value, decimal comparer, string message)
        {
            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(decimal? value, decimal comparer, string message)
        {
            if (!value.HasValue) return;

            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(decimal value, decimal comparer, string message)
        {
            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(decimal? value, decimal comparer, string message)
        {
            if (!value.HasValue) return;

            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(decimal value, decimal comparer, string message)
        {
            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(decimal? value, decimal comparer, string message)
        {
            if (!value.HasValue) return;

            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(decimal value, decimal comparer, string message)
        {
            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(decimal? value, decimal comparer, string message)
        {
            if (!value.HasValue) return;

            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(decimal value, decimal comparer, string message)
        {
            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(decimal? value, decimal comparer, string message)
        {
            if (!value.HasValue) return;

            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(decimal value, decimal from, decimal to, string message)
        {
            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(decimal? value, decimal from, decimal to, string message)
        {
            if (!value.HasValue) return;

            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(double value, double comparer, string message)
        {
            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(double? value, double comparer, string message)
        {
            if (!value.HasValue) return;

            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(double value, double comparer, string message)
        {
            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(double? value, double comparer, string message)
        {
            if (!value.HasValue) return;

            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(double value, double comparer, string message)
        {
            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(double? value, double comparer, string message)
        {
            if (!value.HasValue) return;

            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(double value, double comparer, string message)
        {
            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(double? value, double comparer, string message)
        {
            if (!value.HasValue) return;

            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(double value, double comparer, string message)
        {
            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(double? value, double comparer, string message)
        {
            if (!value.HasValue) return;

            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(double value, double comparer, string message)
        {
            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(double? value, double comparer, string message)
        {
            if (!value.HasValue) return;

            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(double value, double from, double to, string message)
        {
            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(double? value, double from, double to, string message)
        {
            if (!value.HasValue) return;

            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(float value, float comparer, string message)
        {
            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(float? value, float comparer, string message)
        {
            if (!value.HasValue) return;

            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(float value, float comparer, string message)
        {
            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(float? value, float comparer, string message)
        {
            if (!value.HasValue) return;

            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(float value, float comparer, string message)
        {
            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(float? value, float comparer, string message)
        {
            if (!value.HasValue) return;

            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(float value, float comparer, string message)
        {
            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(float? value, float comparer, string message)
        {
            if (!value.HasValue) return;

            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(float value, float comparer, string message)
        {
            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(float? value, float comparer, string message)
        {
            if (!value.HasValue) return;

            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(float value, float comparer, string message)
        {
            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(float? value, float comparer, string message)
        {
            if (!value.HasValue) return;

            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(float value, float from, float to, string message)
        {
            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(float? value, float from, float to, string message)
        {
            if (!value.HasValue) return;

            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(int value, int comparer, string message)
        {
            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(int? value, int comparer, string message)
        {
            if (!value.HasValue) return;

            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(int value, int comparer, string message)
        {
            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(int? value, int comparer, string message)
        {
            if (!value.HasValue) return;

            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(int value, int comparer, string message)
        {
            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(int? value, int comparer, string message)
        {
            if (!value.HasValue) return;

            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(int value, int comparer, string message)
        {
            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(int? value, int comparer, string message)
        {
            if (!value.HasValue) return;

            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(int value, int comparer, string message)
        {
            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(int? value, int comparer, string message)
        {
            if (!value.HasValue) return;

            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(int value, int comparer, string message)
        {
            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(int? value, int comparer, string message)
        {
            if (!value.HasValue) return;

            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(int value, int from, int to, string message)
        {
            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(int? value, int from, int to, string message)
        {
            if (!value.HasValue) return;

            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(long value, long comparer, string message)
        {
            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterThan(long? value, long comparer, string message)
        {
            if (!value.HasValue) return;

            if (value <= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(long value, long comparer, string message)
        {
            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsGreaterOrEqualsThan(long? value, long comparer, string message)
        {
            if (!value.HasValue) return;

            if (value < comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(long value, long comparer, string message)
        {
            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerThan(long? value, long comparer, string message)
        {
            if (!value.HasValue) return;

            if (value >= comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(long value, long comparer, string message)
        {
            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsLowerOrEqualsThan(long? value, long comparer, string message)
        {
            if (!value.HasValue) return;

            if (value > comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(long value, long comparer, string message)
        {
            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(long? value, long comparer, string message)
        {
            if (!value.HasValue) return;

            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(long value, long comparer, string message)
        {
            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(long? value, long comparer, string message)
        {
            if (!value.HasValue) return;

            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(long value, long from, long to, string message)
        {
            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void IsBetween(long? value, long from, long to, string message)
        {
            if (!value.HasValue) return;

            if (value < from || value > to)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(Guid value, Guid comparer, string message)
        {
            if (value != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(Guid value, Guid comparer, string message)
        {
            if (value == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsNull(object obj, string message)
        {
            if (obj != null)
            {
                throw new DomainException(message);
            }
        }

        public static void IsNotNull(object obj, string message)
        {
            if (obj == null)
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(object obj, object comparer, string message)
        {
            if (obj != comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(object obj, object comparer, string message)
        {
            if (obj == comparer)
            {
                throw new DomainException(message);
            }
        }

        public static void IsNotNullOrEmpty(string value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new DomainException(message);
            }
        }

        public static void IsNullOrEmpty(string value, string message)
        {
            if (!string.IsNullOrEmpty(value))
            {
                throw new DomainException(message);
            }
        }

        public static void HasMinLength(string value, int min, string message)
        {
            int length = value.Trim().Length;
            if (length < min)
            {
                throw new DomainException(message);
            }
        }

        public static void HasMaxLength(string value, int max, string message)
        {
            int length = value.Trim().Length;
            if (length > max)
            {
                throw new DomainException(message);
            }
        }

        public static void HasMinAndMaxLength(string value, int min, int max, string message)
        {
            int length = value.Trim().Length;
            if (length < min || length > max)
            {
                throw new DomainException(message);
            }
        }

        public static void HasLength(string value, int valueLength, string message)
        {
            int length = value.Trim().Length;
            if (length != valueLength)
            {
                throw new DomainException(message);
            }
        }

        public static void Contains(string value, string text, string message)
        {
            if (!value.Contains(text))
            {
                throw new DomainException(message);
            }
        }

        public static void AreEquals(string value, string value2, string message)
        {
            if (!value.Equals(value2))
            {
                throw new DomainException(message);
            }
        }

        public static void ArgumentMatches(string pattern, string value, string message)
        {
            var regex = new Regex(pattern);

            if (!regex.IsMatch(value))
            {
                throw new DomainException(message);
            }
        }

        public static void AreNotEquals(string value, string value2, string message)
        {
            if (value.Equals(value2))
            {
                throw new DomainException(message);
            }
        }
    }
}