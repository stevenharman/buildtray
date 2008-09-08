using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace BuildTray.Test
{
    [TestClass]
    public class Specification
    {
        private MockRepository _mock;

        protected virtual void Before_each_spec()
        {
        }

        protected virtual void After_each_spec()
        {
        }

        [TestInitialize]
        public void Setup()
        {
            _mock = new MockRepository();

            Before_each_spec();
        }

        [TestCleanup]
        public void TearDown()
        {
            After_each_spec();
        }

        public MockRepository Mocks { get { return _mock; } }

        public T Mock<T>()
        {
            return _mock.DynamicMock<T>();
        }

        public T Mock<T>(params object[] args)
        {
            return _mock.DynamicMock<T>(args);
        }

        public T Stub<T>()
        {
            return _mock.Stub<T>();
        }

        public T Stub<T>(params object[] args)
        {
            return _mock.Stub<T>(args);
        }

        public T Partial<T>()
            where T : class 
        {
            return _mock.PartialMock<T>();
        }

        public T Partial<T>(params object[] args)
            where T : class
        {
            return _mock.PartialMock<T>(args);
        }

        public void BackToRecord(object mock)
        {
            _mock.BackToRecord(mock);
        }

        public IDisposable Record { get { return _mock.Record(); } }
        public IDisposable Playback { get { return _mock.Playback(); } }
        public IDisposable PlaybackOnly {
            get
            {
                using (Record)
                {
                }
                return Playback;
            }
        }

    }
}
