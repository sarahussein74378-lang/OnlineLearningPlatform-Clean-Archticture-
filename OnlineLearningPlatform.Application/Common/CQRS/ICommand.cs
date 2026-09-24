using MediatR;

namespace OnlineLearningPlatform.Application.Common.CQRS;

/// <summary>
/// Marker interface for commands returning a response of type TResponse.
/// Wraps MediatR.IRequest to keep intent explicit in the application layer.
/// </summary>
public interface ICommand<TResponse> : IRequest<TResponse>
{
}
