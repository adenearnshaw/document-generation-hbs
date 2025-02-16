﻿using System.Runtime.Serialization;

namespace A10w.DocumentGeneration;

/// <summary>
/// Exception raised by DocumentGeneration Library
/// </summary>
[Serializable]
public class DocumentGenerationException : Exception
{
    /// <summary>
    /// Default Constructor
    /// </summary>
    public DocumentGenerationException()
    {
    }

    /// <summary>
    /// Constructor with Message
    /// </summary>
    /// <param name="message">Exception Message</param>
    public DocumentGenerationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Constructor with Message and Inner Exception
    /// </summary>
    /// <param name="message">Exception Message</param>
    /// <param name="innerException">Inner Exception</param>
    public DocumentGenerationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Constructor with SerializationInfo
    /// </summary>
    /// <param name="info">SerializationInfo</param>
    /// <param name="context">Streaming Context</param>
    protected DocumentGenerationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
