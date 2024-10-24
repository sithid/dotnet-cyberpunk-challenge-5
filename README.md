# Cyberpunk Challenge 5

## FOR ME
- [] Fix the varchar -> int conversion error in Cloudwatch logs
- [] Remove the port 8080 thing, should route simply to port 80, at least on the load balancer
- [] create the Admin Controller
- [] create the Model for Admin
- [] hide the AdminController from Swagger
- [] figure out the puzzle that students will have to traverse
- [] Test it

## Objectives
### Background Summary:
You made it! Glad to see you made it Netrunner. This is our last challenge before we assault Arasaka HQ. Before we begin we need to leverage some data from their network that'll help prevent casualties in the assault. 

We will need to identify a vulerable location within Arasaka's Pariah-Nexus API that we can use to gain Subnet access. We've been told that the Athena security system is active in the network and will catch intrusion attempts. The Pariah-Nexus API has a route to access Athena to gather recent Security Logs about the devices in question, we think this might prove a benefit to you. ***We have some useful information below that can point you in the right direction.***

We've learned from an informant that there is a code stored somewhere in the data that the API returns that we can use to add ourselves to the AdminGroup, this is your primary objective! Most importantly we need you to build our own web API that our team can use to get full access to the Pariah-Nexus API.

The Pariah-Nexus API is located at `http://pariah-nexus.blackcypher.io`.

### Primary Objective (Recon)
Summary: 
- Build a WebAPI Project that allows us to collect reconnaissance on Arasaka's Pariah-Nexus API and store in our own database.

**Requirements**:
- In order to accomplish this you'll need to create a Model(s) to hold the data about a Cluster, the devices that are part of the cluster, the processes on each device, and the MemoryMapping on that device. It's up to you how you want to organize the data as long as users of your API are able to pull data from your database about all of those resources.
- Your API should make use of DTOs when transferring data to/from the database and to the user.
- Your API should be able to receive a GET request for a particular cluster ID (or name if you choose) and then make a GET request to get a particular Cluster and return data on the Cluster, any/all devices associated, the processes on each device, the memory mapping data of each device, and any associated Athena Data Events.
- Your API should also be able to make a GET request to get a particular Device and return data on the Device, the processes on the device, the memory mapping data of the device, and any associated Athena Data Events. **NOTE: This is because not every device is part of a cluster though most are.**
- Once your API receives data from Pariah-Nexus it should store it in the database if that data doesn't already exist. NOTE: The data that Pariah-Nexus API contains doesn't change often so you shouldn't have to worry about updating existing data. Although it could happen *shrug*
- 

### Seconday Objective (Exploit)
Summary:
- Once you're able to collect reconnaissance on the Pariah-Nexus API then we need to be able to use that information to send requests to your new API and create new users in the AdminGroup on Pariah-Nexus. This will involve digging around the info that your API returns and getting the right Access Key

Details:
- We believe that there is some kind of hidden Administrator controller with endpoints for CRUD (Create, Read, Update, and Delete) capability that's hidden in Swagger but not sure where this is or what the endpoint's path is though we have some clues.
- FIXME: Fill this out
- Once you have the key then you should be able to scan the security events for any mention of the Administrator controller endpoints to find out exactly what the path is.
- When you have both the Key and the Endpiont then you should be able to authenticate successfully to the Administrator endpoints granting you full CRUD access.

### Tertiary Objective (PWN)
Summary:
- ***WARNING: This is completely experimental and may be diffcult to achieve! Not Recommended!***
- Assuming you're able to accomplish the above objectives then you've achieved full Admin capability. If you still have time then it would be useful to implement full CRUD capability for every type of resource that Pariah-Nexus has available, including the Administrator endpoints. This means that if we send a POST request to your API to create a new Cluster then the result is that your API will also send a POST to Pariah-Nexus and it actually creates the cluster; likewise for Device, Processes, MemoryMapping, and AthenaDataEvent. Same goes for PUTs and DELETEs.

## Architecture
The following is the architecture as we understand it.

![Pariah-Nexus](./docs-assets/Cyberpunk%20(student)%20-%20Pariah%20Nexus%20API.png)