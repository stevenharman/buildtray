using System;
using System.Collections.Generic;
using System.Linq;
using BuildTray.Logic;
using BuildTray.Logic.Entities;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace BuildTray.Test
{
    [TestClass]
    public class When_given_a_Build_Process_Timer_that_has_a_BuildInfo_that_causes_an_exception_in_the_callback : Specification
    {
        protected ITFSServerProxy _proxy;
        protected BuildProcessTimer _buildProcessTimer;
        private ProcessTimerEventHarness _eventHarness;
        private IIntegrationService _integrationService;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _proxy = Mock<ITFSServerProxy>();
            _integrationService = Mock<IIntegrationService>();
            _integrationService.Stub(o => o.GetMaxStartDateForBuildDetailList(null)).IgnoreArguments().Throw(new ApplicationException("Test"));
            IntegrationExtension.IntegrationService = _integrationService;

            _buildProcessTimer = new BuildProcessTimer(_proxy, Stub<IConfigurationData>());
            _eventHarness = new ProcessTimerEventHarness(_buildProcessTimer);

            var buildInfo = new BuildInfo { ServerUrl = new Uri("C:\\NUL"), ProjectName = "Project Name", BuildName = "Build Name" };
            _buildProcessTimer.AddBuild(buildInfo);

            _proxy.Stub(o => o.GetBuildDetails(buildInfo.ServerUrl, buildInfo.ProjectName, buildInfo.BuildName)).Return(new List<IBuildDetail>());

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_the_ThreadExceptions_event_should_be_fired_once()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.ThreadExceptionEvents.Count.ShouldBe(1);
        }

        [TestMethod]
        public void Then_the_ThreadException_event_should_have_the_same_message_as_the_actual_exception()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.ThreadExceptionEvents.First().Exception.Message.ShouldBe("Test");
        }
    }

    [TestClass]
    public class When_given_a_Build_Process_Timer_with_a_BuildInfo_that_is_set_up_to_return_a_given_list_of_build_results : Specification
    {
        protected ITFSServerProxy _proxy;
        protected BuildProcessTimer _buildProcessTimer;
        private ProcessTimerEventHarness _eventHarness;
        private IIntegrationService _integrationService;

        protected override void Before_each_spec()
        {
            base.Before_each_spec();
            _proxy = Mock<ITFSServerProxy>();
            _integrationService = Mock<IIntegrationService>();
            _integrationService.Stub(o => o.GetMaxStartDateForBuildDetailList(null)).IgnoreArguments().Return(new DateTime(2008, 1, 1, 1, 2, 0, 0));
            IntegrationExtension.IntegrationService = _integrationService;

            _buildProcessTimer = new BuildProcessTimer(_proxy, Stub<IConfigurationData>());
            _eventHarness = new ProcessTimerEventHarness(_buildProcessTimer);

            var buildResults = new List<IBuildDetail>
                                   {
                                        CreateBuildDetail(BuildStatus.PartiallySucceeded, new DateTime(2008, 1, 1, 1, 0, 0), "1", false),
                                        CreateBuildDetail(BuildStatus.Failed, new DateTime(2008, 1, 1, 1, 1, 0), "2", true),
                                        CreateBuildDetail(BuildStatus.Succeeded, new DateTime(2008, 1, 1, 1, 2, 0), "3", false),
                                        CreateBuildDetail(BuildStatus.Failed, new DateTime(2008, 1, 1, 1, 3, 0), "4", true),
                                        CreateBuildDetail(BuildStatus.InProgress, new DateTime(2008, 1, 1, 1, 4, 0), "5", false)
                                   };

            var buildInfo = new BuildInfo {ServerUrl = new Uri("C:\\NUL"), ProjectName = "Project Name", BuildName = "Build Name"};
            _buildProcessTimer.AddBuild(buildInfo);

            _proxy.Stub(o => o.GetBuildDetails(buildInfo.ServerUrl, buildInfo.ProjectName, buildInfo.BuildName)).Return(buildResults);

            Mocks.ReplayAll();
        }

        protected override void After_each_spec()
        {
            IntegrationExtension.IntegrationService = null;
            base.After_each_spec();
        }

        [TestMethod]
        public void Then_the_BuildStarted_event_should_have_fired_one_time()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildStartedEvents.Count.ShouldBe(1);
        }

        [TestMethod]
        public void Then_the_BuildStarted_event_should_have_been_called_with_a_build_in_progress()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildStartedEvents.First().Build.Status.ShouldBe(BuildStatus.InProgress);
        }

        [TestMethod]
        public void Then_the_BuildIgnored_event_should_have_fired_two_times()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildIgnoredEvents.Count.ShouldBe(2);
        }

        [TestMethod]
        public void Then_all_of_the_ignored_events_status_should_have_been_Failed()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildIgnoredEvents.Any(o => o.Build.Status != BuildStatus.Failed).ShouldBeFalse();
        }

        [TestMethod]
        public void Then_the_BuildCompleted_event_should_have_fired_two_times()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildCompletedEvents.Count.ShouldBe(2);
        }

        [TestMethod]
        public void Then_the_MostRecentStartDate_should_be_the_result_of_the_integration_service_GetMaxStartDate()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildCompletedEvents.First().MostRecentStartDate.ShouldBe(new DateTime(2008, 1, 1, 1, 2, 0));
        }

        [TestMethod]
        public void Then_the_first_completed_build_should_be_partially_succeeded()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildCompletedEvents.First().Build.Status.ShouldBe(BuildStatus.PartiallySucceeded);
        }

        [TestMethod]
        public void Then_the_second_completed_build_should_be_succeeded()
        {
            _buildProcessTimer.TimerCallback(null);
            _eventHarness.BuildCompletedEvents.Skip(1).First().Build.Status.ShouldBe(BuildStatus.Succeeded);
        }

        private IBuildDetail CreateBuildDetail(BuildStatus status, DateTime startTime, string buildNumber, bool isIgnored)
        {
            var detail = Stub<IBuildDetail>();
            detail.Status = status;
            detail.Stub(o => o.StartTime).Return(startTime).Repeat.Any();
            detail.Stub(o => o.Uri).Return(new Uri("C:\\NUL\\" + buildNumber)).Repeat.Any();

            _integrationService.Stub(o => o.IsBuildIgnored(detail)).Return(isIgnored).Repeat.Any();
            return detail;
        }
    }

    public class ProcessTimerEventHarness
    {
        public List<BuildDetailEventArgs> BuildStartedEvents = new List<BuildDetailEventArgs>();
        public List<BuildDetailEventArgs> BuildCompletedEvents = new List<BuildDetailEventArgs>();
        public List<BuildDetailEventArgs> BuildIgnoredEvents = new List<BuildDetailEventArgs>();
        public List<ExceptionEventArgs> ThreadExceptionEvents = new List<ExceptionEventArgs>();

        public ProcessTimerEventHarness(IBuildProcessTimer timer)
        {
            timer.BuildCompleted += BuildCompleted;
            timer.BuildIgnored += BuildIgnored;
            timer.BuildStarted += BuildStarted;
            timer.ThreadException += ThreadException;
        }

        public void BuildStarted(object sender, BuildDetailEventArgs args)
        {
            BuildStartedEvents.Add(args);
        }

        public void BuildCompleted(object sender, BuildDetailEventArgs args)
        {
            BuildCompletedEvents.Add(args);
        }

        public void BuildIgnored(object sender, BuildDetailEventArgs args)
        {
            BuildIgnoredEvents.Add(args);
        }

        public void ThreadException(object sender, ExceptionEventArgs args)
        {
            ThreadExceptionEvents.Add(args);
        }
    }
    
    
}
