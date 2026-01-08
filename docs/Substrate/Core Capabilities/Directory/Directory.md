# Directory Service

Substrate processes billions of requests every day. The read and write APIs are provided by Substrate platform are processed by hundreds of thousands of machines. The codes for processing an "Api Request" often needs to access necessary metadata like: user info, groups, configuration, machines info, role, authentication states and etc..

Directory service is responsible for storing and retrieving this metadata. And it does this over 11 trillion time a day.

### Background
Substrate Directory for enterprise is an evolution from the Exchange On-Premises infrastructure which is the **native Windows Active Directory** - it is a server role within Windows used for on-premises organizations to store user and tenant level information. This server role was optimized to operate at could scale.




