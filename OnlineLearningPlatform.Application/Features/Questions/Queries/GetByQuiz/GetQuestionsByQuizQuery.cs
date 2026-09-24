using MediatR;
using OnlineLearningPlatform.Application.DTOs.Quiz;
using System.Collections.Generic;

namespace OnlineLearningPlatform.Application.Features.Questions.Queries.GetByQuiz;

public record GetQuestionsByQuizQuery(int QuizId) : IRequest<IEnumerable<QuestionDto>>;
