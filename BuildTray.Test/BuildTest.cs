using System.Collections.Generic;
using System.Linq;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace BuildTray.Test
{
    [TestClass]
    public class When_given_an_in_progress_Build : Specification
    {
        private Build _build;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _build = new Build { Status = BuildStatuses.InProgress };
        }

        [TestMethod]
        public void Then_the_GetFailedTests_method_should_return_null()
        {
            _build.GetFailedTests().ShouldBeNull();
        }
    }

    [TestClass]
    public class When_given_a_passed_Build : Specification
    {
        private Build _build;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _build = new Build { Status = BuildStatuses.Passed };
        }

        [TestMethod]
        public void Then_the_GetFailedTests_method_should_return_null()
        {
            _build.GetFailedTests().ShouldBeNull();
        }
    }

    [TestClass]
    public class When_given_a_failed_Build_with_no_log_location : Specification
    {
        private Build _build;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _build = new Build {Status = BuildStatuses.Failed, LogLocation = null};
        }

        [TestMethod]
        public void Then_the_GetFailedTests_method_should_return_null()
        {
            _build.GetFailedTests().ShouldBeNull();
        }
    }

    [TestClass]
    public class When_given_a_failed_Build_that_returns_null_when_attempting_to_read_failed_tests_from_the_file : Specification
    {
        private Build _build;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _build = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", PreviousBuild = null };

            var service = Mock<IIntegrationService>();
            service.Stub(o => o.GetFailedTestsFromFileForBuild(_build)).Return( null );

            IntegrationExtension.IntegrationService = service;

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_the_tests_returned_from_the_integration_service_should_be_returned()
        {
            _build.GetFailedTests().ShouldBeNull();
        } 
    }

    [TestClass]
    public class When_given_a_failed_Build_with_no_previous_Build : Specification
    {
        private Build _build;
        private FailedTest _failedTest;
        private IIntegrationService _service;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _build = new Build {Status = BuildStatuses.Failed, LogLocation = "TEST", PreviousBuild = null};
            _failedTest = new FailedTest();

            _service = Mock<IIntegrationService>();
            _service.Stub(o => o.GetFailedTestsFromFileForBuild(_build)).Return(new List<FailedTest>{_failedTest});
            
            IntegrationExtension.IntegrationService = _service;

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_the_tests_returned_from_the_integration_service_should_be_returned()
        {
            _build.GetFailedTests().Single().ShouldBe(_failedTest);
        }

        [TestMethod]
        public void Then_the_integration_service_should_be_asked_to_retrieve_the_tests()
        {
            _build.GetFailedTests();
            _service.AssertWasCalled(o => o.GetFailedTestsFromFileForBuild(_build));
        }
    }

    [TestClass]
    public class When_given_a_failed_Build_with_FailedTests_already_set : Specification
    {
        private Build _build;
        private List<FailedTest> _failedTests;
        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _failedTests = new List<FailedTest>();
            _build = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", PreviousBuild = null, FailedTests = _failedTests};
        }

        [TestMethod]
        public void Then_the_failed_tests_returned_should_be_the_same_as_previously_determined()
        {
            _build.GetFailedTests().ShouldBeTheSameAs(_failedTests);
        }
    }

    [TestClass]
    public class When_given_a_failed_Build_with_a_previous_Build_and_two_tests_failing_on_each_and_one_is_an_equal_test : Specification
    {
        private Build _build;

        private FailedTest _originalFailedTest;
        private FailedTest _newFailedTest;
        private FailedTest _otherOriginalFailedTest;
        private FailedTest _otherNewFailedTest;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _otherOriginalFailedTest = new FailedTest { ClassName = "Class", FailedBy = "Foo", Output = "Output", TestName = "FixedTest" };
            _originalFailedTest = new FailedTest { ClassName = "Class", FailedBy = "Foo", Output = "Output", TestName = "Test" };
            _newFailedTest = new FailedTest { ClassName = "Class", FailedBy = "SomeoneElse", Output = "Output", TestName = "Test" };
            _otherNewFailedTest = new FailedTest { ClassName = "Class", FailedBy = "SomeoneElse", Output = "Output", TestName = "OtherTest" };

            _build = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", PreviousBuild = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", FailedTests = new List<FailedTest> { _originalFailedTest, _otherOriginalFailedTest } } };

            var service = Mock<IIntegrationService>();
            service.Stub(o => o.GetFailedTestsFromFileForBuild(_build)).Return(new List<FailedTest> { _newFailedTest, _otherNewFailedTest });

            IntegrationExtension.IntegrationService = service;

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_calling_GetFailedTests_should_return_two_failures()
        {
            _build.GetFailedTests().Count().ShouldBe(2);
        }

        [TestMethod]
        public void Then_calling_GetFailedTests_should_have_the_test_called_Test_failed_by_Foo()
        {
            _build.GetFailedTests().Single(t => t.TestName == "Test").FailedBy.ShouldBe("Foo");
        }

        [TestMethod]
        public void Then_calling_GetFailedTests_should_have_the_test_called_OtherTest_failed_by_SomeoneElse()
        {
            _build.GetFailedTests().Single(t => t.TestName == "OtherTest").FailedBy.ShouldBe("SomeoneElse");
        }

        [TestMethod]
        public void Then_calling_GetFailedTests_should_not_have_any_tests_called_FixedTest_failing()
        {
            _build.GetFailedTests().Any(t => t.TestName == "FixedTest").ShouldBeFalse();
        }
    }

    [TestClass]
    public class When_given_a_failed_Build_with_a_previous_Build_and_a_different_test_failing_on_each_build : Specification
    {
        private Build _build;

        private FailedTest _originalFailedTest;
        private FailedTest _newFailedTest;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _originalFailedTest = new FailedTest { ClassName = "Class", FailedBy = "Foo", Output = "Output", TestName = "Test" };
            _newFailedTest = new FailedTest { ClassName = "Class", FailedBy = "Foo", Output = "Output", TestName = "TestTwo" };

            _build = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", PreviousBuild = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", FailedTests = new List<FailedTest> { _originalFailedTest } } };

            var service = Mock<IIntegrationService>();
            service.Stub(o => o.GetFailedTestsFromFileForBuild(_build)).Return(new List<FailedTest> { _newFailedTest });

            IntegrationExtension.IntegrationService = service;

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_only_one_test_failure_should_be_returned()
        {
            _build.GetFailedTests().Count().ShouldBe(1);
        }

        [TestMethod]
        public void Then_the_most_recent_failure_should_be_the_only_test_failure()
        {
            _build.GetFailedTests().Single().TestName.ShouldBe("TestTwo");
        }
    }

    [TestClass]
    public class When_given_a_failed_Build_with_a_previous_Build_and_the_same_test_failing_twice_in_a_row : Specification
    {
        private Build _build;
        
        private FailedTest _originalFailedTest;
        private FailedTest _newFailedTest;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _originalFailedTest = new FailedTest { ClassName = "Class", FailedBy = "Foo", Output = "Output", TestName = "Test"};
            _newFailedTest = new FailedTest { ClassName = "Class", FailedBy = "SomeoneElse", Output = "Output", TestName = "Test" };

            _build = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", PreviousBuild = new Build { Status = BuildStatuses.Failed, LogLocation = "TEST", FailedTests = new List<FailedTest> { _originalFailedTest } } };

            var service = Mock<IIntegrationService>();
            service.Stub(o => o.GetFailedTestsFromFileForBuild(_build)).Return(new List<FailedTest> { _newFailedTest });

            IntegrationExtension.IntegrationService = service;

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_only_one_failure_should_be_returned()
        {
            _build.GetFailedTests().Count().ShouldBe(1);
        }

        [TestMethod]
        public void Then_foo_should_still_be_responsible_for_the_failed_test()
        {
            _build.GetFailedTests().Single().FailedBy.ShouldBe("Foo");
        }
    }
}
