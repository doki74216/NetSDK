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
    public class Bucket
    {
        public static void bucketSerial(String filePath)
        {
            System.Console.WriteLine("\nhello,Bucket!!");
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"]);
            String bucketName = "chttest2";
            //String bucketRegionName = "EU";

            //PutBucket
            PutBucketRequest Brequest = new PutBucketRequest();
            Brequest.WithBucketName(bucketName);
            //request.WithBucketRegionName(bucketRegionName);
            //Brequest.WithBucketRegion(S3Region.EU);
            Brequest.WithCannedACL(S3CannedACL.PublicRead);
            PutBucketResponse response = s3Client.PutBucket(Brequest);
            //PutBucketResponse response = s3Client.PutBucket(new PutBucketRequest().WithBucketName(bucketName));
            

            //GetService
            ListBucketsResponse result = s3Client.ListBuckets();
            System.Console.WriteLine("Get Service Result:\n {0}\n", result.ResponseXml);

            //********************************************************************************************************************************
            //PutObject-apple.jpg
            System.Console.WriteLine("PutObject!\n");
            s3Client.PutObject(new PutObjectRequest().WithBucketName(bucketName).WithKey("apple.jpg").WithFilePath(filePath));

            //PutObject-sample.jpg
            s3Client.PutObject(new PutObjectRequest().WithBucketName(bucketName).WithKey("photos/2006/January/sample.jpg").WithFilePath(filePath));

            //PutObject-sample2.jpg
            s3Client.PutObject(new PutObjectRequest().WithBucketName(bucketName).WithKey("photos/2006/January/sample2.jpg").WithFilePath(filePath));

            //PutObject-asset.txt
            s3Client.PutObject(new PutObjectRequest().WithBucketName(bucketName).WithKey("asset.txt").WithFilePath(filePath));
            //********************************************************************************************************************************

            //GetBucket
            ListObjectsResponse objects = s3Client.ListObjects(new ListObjectsRequest().WithBucketName(bucketName));
            System.Console.WriteLine("Get Bucket Result:\n {0}\n", objects.ResponseXml);

            ListObjectsResponse Prefixobjects = s3Client.ListObjects(new ListObjectsRequest().WithBucketName(bucketName).WithPrefix("photos/"));
            System.Console.WriteLine("Get Bucket With Prefix Result:\n {0}\n", Prefixobjects.ResponseXml);

            ListObjectsResponse Delimiterobjects = s3Client.ListObjects(new ListObjectsRequest().WithBucketName(bucketName).WithDelimiter("/"));
            System.Console.WriteLine("Get Bucket With Delimiter Result:\n {0}\n", Delimiterobjects.ResponseXml);

            ListObjectsResponse PDobjects = s3Client.ListObjects(new ListObjectsRequest().WithBucketName(bucketName).WithDelimiter("/").WithPrefix("photos/"));
            System.Console.WriteLine("Get Bucket With delimeter & prefix Result:\n {0}\n", PDobjects.ResponseXml);

            ListObjectsResponse MaxKeyobjects = s3Client.ListObjects(new ListObjectsRequest().WithBucketName(bucketName).WithMaxKeys(2));
            System.Console.WriteLine("Get Bucket With MaxKey Result:\n {0}\n", MaxKeyobjects.ResponseXml);

            ListObjectsResponse Markerobjects = s3Client.ListObjects(new ListObjectsRequest().WithBucketName(bucketName).WithMarker("apple.jpg"));
            System.Console.WriteLine("Get Bucket With Marker Result:\n {0}\n", Markerobjects.ResponseXml);

            //********************************************************************************************************************************
            //DeleteObject-apple.jpg
            System.Console.WriteLine("Delete Object!\n");
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey("apple.jpg"));
            
            //DeleteObject-sample.jpg
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey("photos/2006/January/sample.jpg"));
            
            //DeleteObject-sample2.jpg
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey("photos/2006/January/sample2.jpg"));
           
            //DeleteObject-asset.txt
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey("asset.txt"));
            //********************************************************************************************************************************
            
            
            //DeleteBucket
            System.Console.WriteLine("Delete Bucket!");
            s3Client.DeleteBucket(new DeleteBucketRequest().WithBucketName(bucketName));
            System.Console.WriteLine("END!");
        }
    }
}
