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
using System.IO;

namespace TestNetSDK
{
    public class Object
    {
        public static void objectSerial(String filePath)
        {
            System.Console.WriteLine("\nhello,object!!");
            NameValueCollection appConfig = ConfigurationManager.AppSettings;

            AmazonS3 s3Client = AWSClientFactory.CreateAmazonS3Client(appConfig["AWSAccessKey"], appConfig["AWSSecretKey"]);
            String bucketName = "chttest3";
            String objectName = "hello";
            //String filePath = "D:\\Visual Studio Project\\TestNetSDK\\TestNetSDK\\pic.jpg";

            //PutBucket
            System.Console.WriteLine("PutBucket: {0}\n",bucketName);
            PutBucketResponse response = s3Client.PutBucket(new PutBucketRequest().WithBucketName(bucketName));

            //PutObject
            System.Console.WriteLine("PutObject!\n");
            PutObjectRequest request = new PutObjectRequest();
            request.WithBucketName(bucketName);
            request.WithKey(objectName);
            request.WithFilePath(filePath);
            PutObjectResponse PutResult = s3Client.PutObject(request);
            System.Console.WriteLine("Uploaded Object Etag: {0}\n", PutResult.ETag);

            //HeadObject
            System.Console.WriteLine("HeadObject!\n");
            GetObjectMetadataResponse HeadResult = s3Client.GetObjectMetadata(new GetObjectMetadataRequest().WithBucketName(bucketName).WithKey(objectName));
            System.Console.WriteLine("HeadObject: (1)ContentLength: {0} (2)ETag: {1}\n", HeadResult.ContentLength,HeadResult.ETag);
            

            //GetObject
            System.Console.WriteLine("GetObject!\n");
            GetObjectResponse GetResult =  s3Client.GetObject(new GetObjectRequest().WithBucketName(bucketName).WithKey(objectName).WithByteRange(1,15));
            
            
            Stream responseStream = GetResult.ResponseStream; 
            StreamReader reader = new StreamReader(responseStream);
            System.Console.WriteLine("Get Object Content:\n {0}\n", reader.ReadToEnd()); 

            System.Console.WriteLine("Get Object ETag:\n {0}\n", GetResult.ETag);

            //CopyObject
            CopyObjectResponse CopyResult = s3Client.CopyObject(new CopyObjectRequest().WithSourceBucket(bucketName).WithSourceKey(objectName).WithDestinationBucket(bucketName).WithDestinationKey("hihi"));
            System.Console.WriteLine("CopyObject: ETag: {0} \n",CopyResult.ETag);

            //DeleteObject
            System.Console.WriteLine("Delete Object!\n");
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey(objectName));
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey("hihi")); //copied object

            //==============================jerry add ==============================
            System.Console.WriteLine("Jerry Add!\n");
            String objectName2 = "hello2";
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("A", "ABC");

            System.Console.WriteLine("PutObject!\n");
            PutObjectRequest request2 = new PutObjectRequest();
            request2.WithBucketName(bucketName);
            request2.WithKey(objectName2);
            request2.WithAutoCloseStream(true);
            request2.WithCannedACL(S3CannedACL.BucketOwnerFullControl);
            request2.WithContentBody("test");
            request2.WithContentType("test/xml");
            request2.WithGenerateChecksum(true);
            request2.WithMD5Digest("CY9rzUYh03PK3k6DJie09g==");

            request2.WithMetaData(nvc);
            request2.WithWebsiteRedirectLocation("http://hicloud.hinet.net/");
            PutObjectResponse PutResult2 = s3Client.PutObject(request2);
            System.Console.WriteLine("Uploaded Object Etag: {0}\n", PutResult2.ETag);

            System.Console.WriteLine("GetObject!\n");
            GetObjectRequest request3 = new GetObjectRequest();
            request3.WithBucketName(bucketName);
            request3.WithKey(objectName2);
            request3.WithByteRange(1, 2);
            DateTime datetime = DateTime.UtcNow;
            // 似乎不支援 datetime
            request3.WithModifiedSinceDate(datetime.AddHours(-1));
            request3.WithUnmodifiedSinceDate(datetime.AddHours(1));
            request3.WithETagToMatch(PutResult2.ETag);
            request3.WithETagToNotMatch("notMatch");
            GetObjectResponse GetResult2 = s3Client.GetObject(request3);
            Stream responseStream2 = GetResult2.ResponseStream;
            StreamReader reader2 = new StreamReader(responseStream2);
            System.Console.WriteLine("Get Object Content(es):\n {0}\n", reader2.ReadToEnd()); 
            System.Console.WriteLine("Get Object ETag:\n {0}\n", GetResult2.ETag);

            System.Console.WriteLine("HeadObject!\n");
            GetObjectMetadataRequest request4 = new GetObjectMetadataRequest();
            request4.WithBucketName(bucketName);
            request4.WithKey(objectName2);
            DateTime datetime2 = DateTime.UtcNow;
            // 似乎不支援 datetime
            request4.WithModifiedSinceDate(datetime2.AddHours(-1));
            request4.WithUnmodifiedSinceDate(datetime2.AddHours(1));
            request4.WithETagToMatch(PutResult2.ETag);
            request4.WithETagToNotMatch("notMatch");
            GetObjectMetadataResponse HeadResult2 = s3Client.GetObjectMetadata(request4);
            System.Console.WriteLine("HeadObject: (1)ContentLength: {0} (2)ETag: {1}\n", HeadResult2.ContentLength, HeadResult2.ETag);

            CopyObjectRequest request5 = new CopyObjectRequest();
            request5.WithSourceBucket(bucketName);
            request5.WithSourceKey(objectName2);
            request5.WithDestinationBucket(bucketName);
            request5.WithDestinationKey("hihi2");
            DateTime datetime3 = DateTime.UtcNow;
            // 似乎不支援 datetime
            request5.WithModifiedSinceDate(datetime3.AddHours(-1));
            request5.WithUnmodifiedSinceDate(datetime3.AddHours(1));
            request5.WithETagToMatch(PutResult2.ETag);
            request5.WithETagToNotMatch("notMatch");
            request5.WithMetaData(nvc);
            request5.WithCannedACL(S3CannedACL.PublicRead);
            request5.WithContentType("test/xml");
            request5.WithWebsiteRedirectLocation("http://hicloud.hinet.net/");
            CopyObjectResponse CopyResult2 = s3Client.CopyObject(request5);
            System.Console.WriteLine("CopyObject: ETag: {0} \n", CopyResult2.ETag);

            System.Console.WriteLine("Delete Object!\n");
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey(objectName2));
            s3Client.DeleteObject(new DeleteObjectRequest().WithBucketName(bucketName).WithKey("hihi2")); //copied object
            //==============================jerry add end==============================

            //DeleteBucket
            System.Console.WriteLine("Delete Bucket!\n");
            //s3Client.DeletesBucket(new DeleteBucketRequest().WithBucketName(bucketName));
            s3Client.DeleteBucket(new DeleteBucketRequest().WithBucketName(bucketName));
            System.Console.WriteLine("END!");
        }
    }
}
