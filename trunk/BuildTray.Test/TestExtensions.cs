using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BuildTray.Test
{
    public static class TestExtensions
    {
        public static void ShouldBeTrue(this bool condition)
        {
            Assert.IsTrue(condition);
        }

        public static void ShouldBeTrue(this bool condition, string message)
        {
            Assert.IsTrue(condition, message);
        }

        public static void ShouldBeFalse(this bool condition)
        {
            Assert.IsFalse(condition);
        }

        public static void ShouldBeFalse(this bool condition, string message)
        {
            Assert.IsFalse(condition, message);
        }

        public static void ShouldBe<T>(this T actual, T expected)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldBe<T>(this T actual, T expected, string message)
        {
            Assert.AreEqual(expected, actual, message);
        }

        public static void ShouldNotBe<T>(this T actual, T notExpected)
        {
            Assert.AreNotEqual(notExpected, actual);
        }

        public static void ShouldNotBe<T>(this T actual, T notExpected, string message)
        {
            Assert.AreNotEqual(notExpected, actual, message);
        }

        public static void ShouldBe<T>(this T actual, Type type)
        {
            Assert.IsInstanceOfType(actual, type);
        }

        public static void ShouldBe<T>(this T actual, Type type, string message)
        {
            Assert.IsInstanceOfType(actual, type, message);
        }

        public static void ShouldBeEmpty<T>(this IEnumerable<T> collection)
        {
            collection.ShouldNotBeNull();
            collection.Count().ShouldBe(0);
        }

        public static void ShouldBeEmpty<T>(this IEnumerable<T> collection, string message)
        {
            collection.ShouldNotBeNull(message);
            collection.Count().ShouldBe(0, message);
        }

        public static void ShouldNotBeEmpty<T>(this IEnumerable<T> collection)
        {
            collection.ShouldNotBeNull();
            collection.Count().ShouldNotBe(0);
        }

        public static void ShouldNotBeEmpty<T>(this IEnumerable<T> collection, string message)
        {
            collection.ShouldNotBeNull(message);
            collection.Count().ShouldNotBe(0, message);
        }

        public static void ShouldNotBe<T>(this T actual, Type type, string message)
        {
            Assert.IsNotInstanceOfType(actual, type, message);
        }

        public static void ShouldNotBe<T>(this T actual, Type type)
        {
            Assert.IsNotInstanceOfType(actual, type);
        }

        public static void ShouldBeNull(this object actual)
        {
            Assert.IsNull(actual);
        }

        public static void ShouldBeNull(this object actual, string message)
        {
            Assert.IsNull(actual, message);
        }

        public static void ShouldNotBeNull(this object actual)
        {
            Assert.IsNotNull(actual);
        }

        public static void ShouldNotBeNull(this object actual, string message)
        {
            Assert.IsNotNull(actual, message);
        }

        public static void ShouldBeEqualIgnoringCase(this string actual, string expected)
        {
            if (actual.ToLower() != expected.ToLower())
                Assert.AreEqual(expected, actual);
        }

        public static void ShouldBeEqualIgnoringCase(this string actual, string expected, string message)
        {
            if (actual.ToLower() != expected.ToLower())
                Assert.AreEqual(expected, actual, message);
        }

        public static void ShouldContain(this IEnumerable list, object expected)
        {
            list.ShouldContain(expected, null);
        }

        public static void ShouldContain(this IEnumerable list, object expected, string message)
        {
            bool found = false;

            if (list != null)
                foreach (object o in list)
                    if (o == expected)
                        found = true;

            found.ShouldBeTrue("The <" + (list ?? "NULL") + "> should have contained: <" + (expected ?? "NULL") + ">. " + (message ?? string.Empty));
        }

        public static void ShouldContain<T>(this IEnumerable<T> list, T expected)
        {
            list.Contains(expected).ShouldBeTrue();
        }

        public static void ShouldContain<T>(this IEnumerable<T> list, T expected, string message)
        {

            list.Contains(expected).ShouldBeTrue("The <" + (list.ToString() ?? "NULL") + "> should have contained: <" + (expected.ToString() ?? "NULL") + ">. " + (message ?? string.Empty));
        }

        public static void ShouldNotContain(this IEnumerable list, object expected)
        {
            list.ShouldNotContain(expected, null);
        }

        public static void ShouldNotContain(this IEnumerable list, object expected, string message)
        {
            bool found = false;

            if (list != null)
                foreach (var o in list)
                    if (o == expected)
                        found = true;

            found.ShouldBeFalse("The <" + (list ?? "NULL") + "> should not have contained: <" + (expected ?? "NULL") + ">. " + (message ?? string.Empty));
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> list, T expected)
        {
            list.Contains(expected).ShouldBeFalse();
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> list, T expected, string message)
        {

            list.Contains(expected).ShouldBeFalse("The <" + (list.ToString() ?? "NULL") + "> should not have contained: <" + (expected.ToString() ?? "NULL") + ">. " + (message ?? string.Empty));
        }

        public static void ShouldStartWith(this string actual, string expected)
        {
            actual.ShouldStartWith(expected, null);
        }

        public static void ShouldStartWith(this string actual, string expected, string message)
        {
            if (actual == null || !actual.StartsWith(expected))
            {
                Assert.Fail("Actual: <" + (actual ?? "NULL") + "> did not start with the Expected: <" + (expected ?? "NULL") + ">. " + (message ?? string.Empty));
            }
        }

        public static void ShouldEndWith(this string actual, string expected)
        {
            actual.ShouldEndWith(expected, null);
        }

        public static void ShouldEndWith(this string actual, string expected, string message)
        {
            if (actual == null || !actual.EndsWith(expected))
            {
                Assert.Fail("Actual: <" + (actual ?? "NULL") + "> did not end with the Expected: <" + (expected ?? "NULL") + ">. " + (message ?? string.Empty));
            }
        }

        /// <summary>
        /// Compares two objects by reference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void ShouldBeTheSameAs<T>(this T actual, T expected)
        {
            Assert.AreSame(expected, actual);
        }

        /// <summary>
        /// Compares two objects by reference with a message for failures.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="message"></param>
        public static void ShouldBeTheSameAs<T>(this T actual, T expected, string message)
        {
            Assert.AreSame(expected, actual, message);
        }

        /// <summary>
        /// Makes sure two objects are not the same reference.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        public static void ShouldBeDifferentFrom<T>(this T actual, T expected)
        {
            Assert.AreNotSame(expected, actual);
        }

        /// <summary>
        /// Makes sure two objects are not the same reference with a message for failures.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="message"></param>
        public static void ShouldBeDifferentFrom<T>(this T actual, T expected, string message)
        {
            Assert.AreNotSame(expected, actual, message);
        }
    }
}
