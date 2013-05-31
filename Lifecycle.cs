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
    class Lifecycle
    {
        public static void lifecycleSerial()
        {
            System.Console.WriteLine("\nhello,Lifecyle!!");
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"]);
            String bucketName = "chttest2";

            //PutBucket
            System.Console.WriteLine("PutBucket: {0}\n", bucketName);
            s3Client.PutBucket(new PutBucketRequest().WithBucketName(bucketName));

            //PutBucketLifecycle rule1 day********************************************************************************************
            PutLifecycleConfigurationRequest request = new PutLifecycleConfigurationRequest();
            LifecycleConfiguration configuration = new LifecycleConfiguration();
            List<LifecycleRule> list = new List<LifecycleRule>();
            
            System.Console.WriteLine("PutBucketLicycle Rule1");
            LifecycleRule rule1 = new LifecycleRule();
            LifecycleRuleExpiration days = new LifecycleRuleExpiration();
            days.Days = 5;

            rule1.Id = "Day_rule";
            rule1.Prefix = "day";
            rule1.Status = LifecycleRuleStatus.Enabled;
            rule1.Expiration = days;

            list.Add(rule1);
            
            //PutBucketLifecycle rule2 date********************************************************************************************
            System.Console.WriteLine("PutBucketLicycle Rule2");
            LifecycleRule rule2 = new LifecycleRule();
            LifecycleRuleExpiration date = new LifecycleRuleExpiration();
            DateTime dt = DateTime.Now.AddDays(100);
            date.Date = dt;

            rule2.Id = "Date-rule";
            rule2.Prefix = "date";
            rule2.Status = LifecycleRuleStatus.Enabled;
            rule2.Expiration = date;

            list.Add(rule2);

            configuration.Rules = list;

            request.WithBucketName(bucketName);
            request.WithConfiguration(configuration);
            PutLifecycleConfigurationResponse putResult = s3Client.PutLifecycleConfiguration(request);
            System.Console.WriteLine("PutBucketLifecycle, requestID: {0}\n", putResult.RequestId);


            //GetBucketLifecycle
            GetLifecycleConfigurationResponse getResut = s3Client.GetLifecycleConfiguration(new GetLifecycleConfigurationRequest().WithBucketName(bucketName));
            System.Console.WriteLine("GetBucketLifecycle result:\n {0}\n", getResut.ResponseXml);
            

            //DeleteBucketLifecycle
            DeleteLifecycleConfigurationResponse deleteResult = s3Client.DeleteLifecycleConfiguration(new DeleteLifecycleConfigurationRequest().WithBucketName(bucketName));
            System.Console.WriteLine("DeleteBucketLifecycle, requestID: {0}\n", deleteResult.RequestId);

            //DeleteBucket
            System.Console.WriteLine("Delete Bucket!");
            s3Client.DeleteBucket(new DeleteBucketRequest().WithBucketName(bucketName));
            System.Console.WriteLine("END!");
        }
    }
}
