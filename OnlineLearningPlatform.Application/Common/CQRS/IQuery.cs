using MediatR;

namespace OnlineLearningPlatform.Application.Common.CQRS;

/// <summary>
/// Marker interface for queries returning a response of type TResponse.
/// Wraps MediatR.IRequest to keep intent explicit in the application layer.
/// </summary>
public interface IQuery<TResponse> : IRequest<TResponse>
{
}
