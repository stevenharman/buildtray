using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BuildTray.Logic;
using BuildTray.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BuildTray.Modules;

namespace BuildTray.Test
{
    [TestClass]
    public class When_given_a_NotifyIcon_in_Success_Mode
    {
        private INotifyIcon _icon;

        [TestInitialize]
        public void Setup()
        {
            _icon = new NotifyIconProxy();
            _icon.Success();
        }

        [TestMethod]
        public void Then_Failure_will_set_status_to_failed()
        {
            _icon.Failure();
            _icon.CurrentStatus().ShouldBe(TrayStatus.Failure);
        }

        [TestMethod]
        public void Then_InProgress_will_set_status_to_SuccessInProgress()
        {
            _icon.InProgress();
            _icon.CurrentStatus().ShouldBe(TrayStatus.SuccessInProgress);
        }

        [TestMethod]
        public void Then_Success_will_not_change_the_status()
        {
            _icon.Success();
            _icon.CurrentStatus().ShouldBe(TrayStatus.Success);
        }
    }

    [TestClass]
    public class When_given_a_NotifyIcon_in_Failure_Mode
    {

        private INotifyIcon _icon;

        [TestInitialize]
        public void Setup()
        {
            _icon = new NotifyIconProxy();
            _icon.Failure();
        }

        [TestMethod]
        public void Then_Success_will_set_status_to_Success()
        {
            _icon.Success();
            _icon.CurrentStatus().ShouldBe(TrayStatus.Success);
        }

        [TestMethod]
        public void Then_InProgress_will_set_status_to_FailureInProgress()
        {
            _icon.InProgress();
            _icon.CurrentStatus().ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_Failure_will_not_change_the_status()
        {
            _icon.Failure();
            _icon.CurrentStatus().ShouldBe(TrayStatus.Failure);
        }
    }

    [TestClass]
    public class When_working_with_TrayStatus_enumerations
    {
        [TestMethod]
        public void Then_combining_failure_and_success_will_result_in_failure()
        {
            TrayStatus.Failure.Combine(TrayStatus.Success).ShouldBe(TrayStatus.Failure);
        }

        [TestMethod]
        public void Then_combining_failure_and_succesInProgress_will_result_in_failure_in_progress()
        {
            TrayStatus.Failure.Combine(TrayStatus.SuccessInProgress).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod] 
        public void Then_combining_failure_and_failureInProgress_will_result_in_failure_in_progress()
        {
            TrayStatus.Failure.Combine(TrayStatus.FailureInProgress).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_failure_and_failure_will_result_in_failure()
        {
            TrayStatus.Failure.Combine(TrayStatus.Failure).ShouldBe(TrayStatus.Failure);
        }

        [TestMethod]
        public void Then_combining_success_and_failure_will_result_in_failure()
        {
            TrayStatus.Success.Combine(TrayStatus.Failure).ShouldBe(TrayStatus.Failure);
        }

        [TestMethod]
        public void Then_combining_succes_and_failureInProgress_will_result_in_failureInProgress()
        {
            TrayStatus.Success.Combine(TrayStatus.FailureInProgress).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_success_and_successInProgress_will_result_in_successInProgress()
        {
            TrayStatus.Success.Combine(TrayStatus.SuccessInProgress).ShouldBe(TrayStatus.SuccessInProgress);
        }

        [TestMethod]
        public void Then_combining_success_and_success_will_result_in_success()
        {
            TrayStatus.Success.Combine(TrayStatus.Success).ShouldBe(TrayStatus.Success);
        }

        [TestMethod]
        public void Then_combining_successInProgress_and_failure_will_result_in_failureInProgress()
        {
            TrayStatus.SuccessInProgress.Combine(TrayStatus.Failure).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_successInProgress_and_failureInProgress_will_result_in_failureInProgress()
        {
            TrayStatus.SuccessInProgress.Combine(TrayStatus.FailureInProgress).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_successInProgress_and_success_will_result_in_successInProgress()
        {
            TrayStatus.SuccessInProgress.Combine(TrayStatus.Success).ShouldBe(TrayStatus.SuccessInProgress);
        }

        [TestMethod]        
        public void Then_combining_successInProgress_and_successInProgress_will_result_in_successInProgress()
        {
            TrayStatus.SuccessInProgress.Combine(TrayStatus.SuccessInProgress).ShouldBe(TrayStatus.SuccessInProgress);
        }

        [TestMethod]
        public void Then_combining_failureInProgress_and_failure_will_result_in_failureInProgress()
        {
            TrayStatus.FailureInProgress.Combine(TrayStatus.Failure).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_failureInProgress_and_failureInProgress_will_result_in_failureInProgress()
        {
            TrayStatus.FailureInProgress.Combine(TrayStatus.FailureInProgress).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_failureInProgress_and_success_will_result_in_failureInProgress()
        {
            TrayStatus.FailureInProgress.Combine(TrayStatus.Success).ShouldBe(TrayStatus.FailureInProgress);
        }

        [TestMethod]
        public void Then_combining_failureInProgress_and_successInProgress_will_result_in_failureInProgress()
        {
            TrayStatus.FailureInProgress.Combine(TrayStatus.SuccessInProgress).ShouldBe(TrayStatus.FailureInProgress);
        }
    }
}
