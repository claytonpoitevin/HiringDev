# http://awseb-e-u-AWSEBLoa-9OGLZBL63GU4-421507736.us-east-1.elb.amazonaws.com/ - api

# http://awseb-e-m-awsebloa-11n2c7i899ml2-1618593881.us-east-1.elb.amazonaws.com/ - client

# ec2-54-88-183-215.compute-1.amazonaws.com:27017 - mongodb

cd ./src/HiringDev.Client
dotnet eb deploy-environment
cd ../HiringDev.Service
dotnet eb deploy-environment