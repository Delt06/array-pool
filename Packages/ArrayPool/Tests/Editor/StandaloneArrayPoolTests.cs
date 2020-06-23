using System;
using System.Linq;
using NUnit.Framework;

namespace DELTation.Pools.Tests.Editor
{
    [TestFixture]
    public sealed class StandaloneArrayPoolTests
    {
        private StandaloneArrayPool<int> _pool;

        [SetUp]
        public void SetUp()
        {
            _pool = new StandaloneArrayPool<int>();
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-5)]
        public void Ctor_InvalidMaxSupportedLength_ThrowsArgumentOutOfRangeException(int length)
        {
            Assert.That(() => new StandaloneArrayPool<int>(length), 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(10)]
        public void Rent_WithValidLength_ReturnsRentedArrayWithCorrectLength(int length)
        {
            var array = _pool.Rent(length);
            
            Assert.That(array.Length, Is.GreaterThanOrEqualTo(length));
            Assert.That(_pool.InRent(array));
        } 
        
        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-2)]
        [TestCase(-10)]
        public void Rent_WithInvalidLength_ThrowsArgumentOutOfRangeException(int length)
        {
            Assert.That(() => _pool.Rent(length), 
                Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Return_AfterRented_BecomesNotRented()
        {
            var array = _pool.Rent(12);

            _pool.Return(array);
            
            Assert.That(_pool.InRent(array), Is.False);
        }

        [Test]
        [TestCase(10)]
        [TestCase(50)]
        [TestCase(150)]
        public void HeavyTest(int count)
        {
            int GetLength(int i) => i + 1;
            var arrays = Enumerable.Range(0, count)
                .Select(i => _pool.Rent(GetLength(i)))
                .ToList();
            
            Assert.That(arrays.All(a => _pool.InRent(a)));

            for (var i = 0; i < count; i++)
            {
                Assert.That(arrays[i].Length, Is.GreaterThanOrEqualTo(GetLength(i)));
                Assert.That(_pool.InRent(arrays[i]));
            }

            foreach (var array in arrays)
            {
                _pool.Return(array);
            }

            Assert.That(arrays.All(a => !_pool.InRent(a)));
        }
    }
}