﻿using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using RestfulMicroserverless.Contracts;

namespace AwsLibrary.SNS
{
    public class SnsClient : ISnsClient
    {
        private static readonly Regex _snsTopicRegex = new Regex("^arn:aws:sns:([a-z]{2}-[a-z]+-[0-9]+):[0-9]{12}:([A-Za-z0-9\\-_]{1,256}$)");
        private readonly AmazonSimpleNotificationServiceClient _awsSnsClient;
        private readonly PublishRequest _publishRequest;
        private readonly string _topicArn;

        public SnsClient(string topicArn)
        {
            var match = _snsTopicRegex.Match(topicArn);
            if (!match.Success)
            {
                throw new ArgumentException("SNS Client Constructor must provide a valid TopicARN string.");
            }


            _topicArn = topicArn;
            _awsSnsClient = new AmazonSimpleNotificationServiceClient(RegionEndpoint.GetBySystemName(match.Groups[1].Value));
            _publishRequest = new PublishRequest { TopicArn = _topicArn };
        }

        public async Task PublishMessageToTopicAsync(string message, ILogger logger)
        {
            logger.LogInfo(() => $"Publishing Message to Topic: {_topicArn}");
            _publishRequest.Message = message;
            try
            {
                var response = await _awsSnsClient.PublishAsync(_publishRequest).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(response.MessageId))
                {
                    logger.LogInfo(() => $"Successfully published the message. MessageId: {response.MessageId}");
                }
            }
            catch (AuthorizationErrorException e)
            {
                logger.LogError(() => "The IAM role in use does not have access to publish to the topic.");
                CatchAmazonSnsException(e, logger);
            }
            catch (EndpointDisabledException e)
            {
                logger.LogError(() => "The SNS Endpoint has been disabled.");
                CatchAmazonSnsException(e, logger);
            }
            catch (InternalErrorException e)
            {
                logger.LogError(() => "AWS SNS Has thrown an internal service error.");
                CatchAmazonSnsException(e, logger);
            }
            catch (InvalidParameterException e)
            {
                logger.LogError(() => "Invalid Parameter in the Publish Request");
                CatchAmazonSnsException(e, logger);
            }
            catch (InvalidParameterValueException e)
            {
                logger.LogError(() => "Invalid Parameter Value in the Publish Request");
                CatchAmazonSnsException(e, logger);
            }
            catch (NotFoundException e)
            {
                logger.LogError(() => $"The Targeted ARN: {_topicArn} cannot be found.");
                CatchAmazonSnsException(e, logger);
            }
        }

        private static void CatchAmazonSnsException(AmazonServiceException e, ILogger logger)
        {
            logger.LogError(() => $"Error Code: {e.ErrorCode}");
            logger.LogError(() => $"Error Type: {e.ErrorType}");
            logger.LogError(() => $"Request ID: {e.RequestId}");
            logger.LogError(() => $"HTTP Status Code: {e.StatusCode}");
            throw new Exception();
        }
    }
}