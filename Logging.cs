using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace TestNetSDK
{
    class Logging
    {
        public static void loggingSerial()
        {
            System.Console.WriteLine("\nhello,Logging!!");
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"]);
            String bucketName = "chttest1";
            String logBucketName = "chttest2";

            //PutBucket
            System.Console.WriteLine("PutBucket: {0} + {1}\n", bucketName,logBucketName);
            s3Client.PutBucket(new PutBucketRequest().WithBucketName(bucketName));
            s3Client.PutBucket(new PutBucketRequest().WithBucketName(logBucketName));
            
            //PutBucketACL for logDelivery user
            SetACLRequest aclRequest = new SetACLRequest();
            aclRequest.WithBucketName(logBucketName);
            aclRequest.WithCannedACL(S3CannedACL.LogDeliveryWrite);
            SetACLResponse setACLResult = s3Client.SetACL(aclRequest);
            System.Console.WriteLine("SetBucketACL, requestID:{0}\n",setACLResult.RequestId);

            //PutBucketLogging
            S3BucketLoggingConfig config = new S3BucketLoggingConfig();
            config.WithTargetBucketName(logBucketName);
            config.WithTargetPrefix("log-prefix");
            EnableBucketLoggingResponse setLoggingResult = s3Client.EnableBucketLogging(new EnableBucketLoggingRequest().WithBucketName(bucketName).WithLoggingConfig(config));
            System.Console.WriteLine("SetBucketLogging, requestID:{0}\n", setLoggingResult.RequestId);

            //GetBucketLogging
            GetBucketLoggingResponse getLoggingResult = s3Client.GetBucketLogging(new GetBucketLoggingRequest().WithBucketName(bucketName));
            System.Console.WriteLine("GetBucketLogging:\n {0}\n", getLoggingResult.ResponseXml);

            //jerry add
            S3BucketLoggingConfig config2 = new S3BucketLoggingConfig();
            config2.WithTargetBucketName(logBucketName);
            config2.WithTargetPrefix("log-prefix");

            S3Grant grant = new  S3Grant();
            S3Grantee grantee = new S3Grantee();

            grantee.WithCanonicalUser("262ab9144f28c47d5f65ad45d5a4930a6547e12c175762cb14dbae95ec7c0680", "HNJERRY");
            grant.WithGrantee(grantee);
            //grant_list.Add(grant);     
            config2.AddGrant(grantee, S3Permission.FULL_CONTROL);
            //config2.WithGrants(grant_list);
            EnableBucketLoggingResponse setLoggingResult2 = s3Client.EnableBucketLogging(new EnableBucketLoggingRequest().WithBucketName(bucketName).WithLoggingConfig(config2));
            System.Console.WriteLine("SetBucketLogging, requestID:{0}\n", setLoggingResult.RequestId);

            GetBucketLoggingResponse getLoggingResult2 = s3Client.GetBucketLogging(new GetBucketLoggingRequest().WithBucketName(bucketName));
            System.Console.WriteLine("GetBucketLogging:\n {0}\n", getLoggingResult2.ResponseXml);
            //jerry add end


            //DeleteBucket
            System.Console.WriteLine("Delete Bucket!");
            s3Client.DeleteBucket(new DeleteBucketRequest().WithBucketName(bucketName));
            s3Client.DeleteBucket(new DeleteBucketRequest().WithBucketName(logBucketName));
            System.Console.WriteLine("END!");
        }
    }
}
