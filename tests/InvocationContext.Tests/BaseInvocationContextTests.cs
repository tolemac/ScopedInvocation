using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace InvocationContext.Tests
{
    public class BaseInvocationContextTests
    {
        [Fact]
        public async Task EventsAreLaunchedWhenCallThroughInvocationContext()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);
            var executionFlag = false;
            var successFlag = false;
            var completeFlag = false;

            ic.Working.ShouldBeFalse();
            await ic.InvokeAsync(options =>
            {
                options.OnActionSuccessAsync = token =>
                {
                    successFlag = true;
                    return Task.CompletedTask;
                };
                options.OnCompleteAsync = token =>
                {
                    completeFlag = true;
                    return Task.CompletedTask;
                };
            }, (sp) =>
            {
                ic.Working.ShouldBeTrue();
                executionFlag = true;
                return Task.CompletedTask;
            });

            ic.Working.ShouldBeFalse();
            executionFlag.ShouldBeTrue();
            successFlag.ShouldBeTrue();
            completeFlag.ShouldBeTrue();
        }

        [Fact]
        public async Task ExceptionEventsIsLaunchedWhenCallThroughInvocationContext()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);
            var successFlag = false;
            var completeFlag = false;
            var actionExceptionFlag = false;
            var invocationExceptionFlag = false;

            (await Should.ThrowAsync<Exception>(async () =>
            {
                await ic.InvokeAsync(options =>
                {
                    options.OnActionSuccessAsync = token =>
                    {
                        successFlag = true;
                        return Task.CompletedTask;
                    };
                    options.OnCompleteAsync = token =>
                    {
                        completeFlag = true;
                        return Task.CompletedTask;
                    };
                    options.OnActionExceptionAsync = (ex, token) =>
                    {
                        actionExceptionFlag = true;
                        return Task.FromResult(true);
                    };
                    options.OnInvocationException = (ex, token) =>
                    {
                        invocationExceptionFlag = true;
                        return Task.CompletedTask;
                    };
                }, (sp) => throw new Exception("Test exception"));
            })).Message.ShouldBe("Test exception");

            successFlag.ShouldBeFalse();
            completeFlag.ShouldBeTrue();
            actionExceptionFlag.ShouldBeTrue();
            invocationExceptionFlag.ShouldBeTrue();
        }

        [Fact]
        public async Task IfReturnFalseOnActionExceptionFuncTheInvocationNotThrowsAnyException()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);
            var actionExceptionFlag = false;
            var invocationExceptionFlag = false;

            await Should.NotThrowAsync(async () =>
            {
                await ic.InvokeAsync(options =>
                {
                    options.OnActionExceptionAsync = (ex, token) =>
                    {
                        actionExceptionFlag = true;
                        return Task.FromResult(false);
                    };
                    options.OnInvocationException = (ex, token) => throw new Exception("Not raised");
                }, (sp) => throw new Exception("Test exception"));
            });

            actionExceptionFlag.ShouldBeTrue();
            invocationExceptionFlag.ShouldBeFalse();
        }

        [Fact]
        public async Task WhenOnActionSuccessThorwsExceptionItThrowThroughInvocation()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);

            (await Should.ThrowAsync<Exception>(async () =>
            {
                await ic.InvokeAsync(
                    options => { options.OnActionSuccessAsync = token => throw new Exception("Test exception"); },
                    (sp) => Task.CompletedTask
                );
            })).Message.ShouldBe("Test exception");
        }

        [Fact]
        public async Task WhenOnActionSuccessThrowsExceptionThrowsTheInvocationThrowsErrorAllwaisIncludeWhenOnActionExceptionSaysNotRethrow()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);

            (await Should.ThrowAsync<Exception>(async () =>
            {
                await ic.InvokeAsync(
                    options =>
                    {
                        options.OnActionSuccessAsync = token => throw new Exception("Test exception");
                        options.OnActionExceptionAsync = (ex, token) => Task.FromResult(false);
                    },
                    (sp) => Task.CompletedTask
                );
            })).Message.ShouldBe("Test exception");
        }

        [Fact]
        public async Task WhenOnActionExceptionThrowsTheInvocationThrowsAnAggregateExceptionWithBothExceptionsInnerAndOutter()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);

            var ex = (await Should.ThrowAsync<Exception>(async () =>
            {
                await ic.InvokeAsync(
                    options =>
                    {
                        options.OnActionExceptionAsync = (_, token) =>
                            throw new Exception("Test exception from OnActionException func");
                    },
                    (sp) => throw new Exception("Test exception")
                );
            })) as AggregateException;

            ex.ShouldNotBeNull();
            ex.InnerExceptions.First().Message.ShouldBe("Test exception");
            ex.InnerExceptions.Last().Message.ShouldBe("Test exception from OnActionException func");
        }

        [Fact]
        public async Task WhenOnCompleteThrowsTheInvocationThrowsTheException()
        {
            var ic = new BaseInvocationContext<InvocationContextData, BaseInvocationContextOptions>(null, null);

            (await Should.ThrowAsync<Exception>(async () =>
            {
                await ic.InvokeAsync(
                    options =>
                    {
                        options.OnCompleteAsync = token => throw new Exception("Test exception");
                    },
                    sp => Task.CompletedTask
                );
            })).Message.ShouldBe("Test exception");
        }
    }
}
