﻿using System;
using System.Linq.Expressions;
using NSubstitute;
using TestStack.Seleno.Tests.TestObjects;
using SUT = TestStack.Seleno.Configuration.SelenoHost;

namespace TestStack.Seleno.Tests.Configuration.SelenoHost
{
    public class When_navigating_to_initial_page_from_controller_action : SelenoHostSpecification
    {
        private readonly Expression<Action<TestController>> _controllerAction = x => x.Index();

        public void Given_the_Seleno_Application_is_configured()
        {
            SUT.Run(appConfigurator: AppConfigurator);
        }
        
        public void When_navigating_to_initial_page()
        {
            SUT.NavigateToInitialPage<TestController, TestPage>(_controllerAction);
        }
        
        public void Then_it_should_invoke_PageNavigator_To_method_with_controller_action()
        {
            PageNavigator
                .Received()
                .To<TestController, TestPage>(_controllerAction);
        }
    }
}