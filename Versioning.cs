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
    class Versioning
    {
        public static void VersioningSerial()
        {
            System.Console.WriteLine("\nhello,Versioning!!");
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"]);
            String bucketName = "chttest1";

            //PutBucket
            PutBucketResponse response = s3Client.PutBucket(new PutBucketRequest().WithBucketName(bucketName));

            //PutBucketVersioning
            SetBucketVersioningResponse putVersioningResult = s3Client.SetBucketVersioning(new SetBucketVersioningRequest().WithBucketName(bucketName).WithVersioningConfig(new S3BucketVersioningConfig().WithStatus("Enabled")));
            System.Console.WriteLine("PutBucketVersioning, requestID:{0}\n", putVersioningResult.RequestId);
            
            //GetBucketVersioning
            GetBucketVersioningResponse getVersioningResult = s3Client.GetBucketVersioning(new GetBucketVersioningRequest().WithBucketName(bucketName));
            System.Console.WriteLine("GetBucketVersioning Result:\n {0}\n",getVersioningResult.ResponseXml);

            //DeleteBucket
            System.Console.WriteLine("Delete Bucket!");
            s3Client.DeleteBucket(new DeleteBucketRequest().WithBucketName(bucketName));
            System.Console.WriteLine("END!");
        }
    }
}
