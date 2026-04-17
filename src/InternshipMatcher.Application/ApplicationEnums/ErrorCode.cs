using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipMatcher.Application.ApplicationEnums;
public enum ErrorCodes
{
    // General
    UnknownError = 0,
    ValidationFailed = 1,
    BadRequest = 2,
    NotFound = 3,
    Unauthorized = 4,
    Forbidden = 5,

    // Auth
    InvalidCredentials = 100,
    TokenExpired = 101,
    TokenInvalid = 102,
    UserNotFound = 103,
    UserAlreadyExists = 104,

    // AI Matching
    EmptySkills = 200,
    InvalidJobDescription = 201,
    MatchingFailed = 202,

    // External services
    ExternalServiceError = 300
}
