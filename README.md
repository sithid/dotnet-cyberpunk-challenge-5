# Cyberpunk Challenge 5

## Objectives

### Background Summary

You made it! Glad to see you here, Netrunner. This is our last challenge before we assault Arasaka HQ. But before we move in, we need some crucial data from their network that will help prevent casualties during the assault. Your skills will make all the difference.

We need to identify a vulnerable location within Arasaka's **Pariah-Nexus API** that we can exploit for subnet access. Our intel suggests that the **Athena security system** is active in the network and will immediately detect intrusion attempts. Fortunately, the Pariah-Nexus API has an endpoint that accesses Athena to gather recent security logs about the devices in question. This might just be the key to your mission. ***We've gathered some intel that could point you in the right direction.***

We've also learned from an informant that there’s a code hidden in the data returned by the API—a code we can use to add ourselves to the AdminGroup. Reconnaissance is the bare minimum, but gaining access to the AdminGroup will make this gig perfect. Most importantly, we need you to build a web API that our team can use to gain full access to the Pariah-Nexus API. Note that the Pariah-Nexus API resets itself at midnight, and you may notice a service interruption from midnight to approximately 12:10. Let’s get to work.

The **Pariah-Nexus API** is located at:

```
http://pariah-nexus.bla
ckcypher.io
```

You can also access **Swagger** documentation at:

```
http://pariah-nexus.blackcypher.io/swagger/index.html
```

This will show you all the endpoints currently available for use.

## Primary Objective: Reconnaissance

**Summary**: Build a web API that allows us to collect reconnaissance on Arasaka’s Pariah-Nexus API and store it in our own database.

### Requirements

- Develop a model (or models) that can store data about ****clusters****, ****devices****, ****processes****, and ****memory mappings****. You decide how to organize this data, but users of your API must be able to access all of it from your database.
- Use **DTOs (Data Transfer Objects)** when transferring data between your database and the user.
- Your API should receive a **GET request** for a specific cluster (identified by ID or name), make a GET request to Pariah-Nexus, and return information on the cluster, all associated devices, the processes running on those devices, memory mapping data, and any linked **Athena Data Events**.
- Your API should also support a **GET request** to retrieve a particular **device** and its associated data, even if the device isn’t part of a cluster.
  - Note: Not every device belongs to a cluster, but most do.
- Whenever your API retrieves data from Pariah-Nexus, it should **store that data in your database** if it doesn’t already exist. The data doesn’t change often, but keep that in mind for potential future updates.

### Hints

- **Finding the Admin Endpoint**: One of the clusters has a vulnerable device that can provide valuable information. Look for a **device within a cluster** whose environment is `"DevForge"`, region is `"sa-east-1"`, and was created in **2024**. This device is a **Bio-Organic Processor**, and its **public key** (base64-encoded) contains instructions for locating the **Administrator Endpoint**. Keep in mind, there may be more than one device that fits these criteria.

- **Athena Access Key**: To access Athena Data Events, find a device belonging to a cluster in the `"TechHub"` environment, with **less than 15 nodes** and **less than 30 CPU cores**. The device should be running a **`Warcraft3.exe`**\*\* process\*\* and have **560GB of ROM**. Once you find it, you'll get an access key for Athena.

  - **Note**: Due to recent intrusion attempts by the Maelstrom gang, nearly all Athena access keys have been revoked. Only a few valid keys remain, so be prepared for some trial and error if you don't have the right key.

## Secondary Objective: Exploitation

**Summary**: After collecting reconnaissance on the Pariah-Nexus API, use that information to gain access to the **AdminGroup** on Pariah-Nexus. This means finding the right access key hidden in the data and using it effectively.

### Details

- We suspect that there’s a hidden **Administrator controller** that provides **CRUD** (Create, Read, Update, Delete) capabilities. Unfortunately, it’s hidden from Swagger documentation, so you’ll need to dig for clues.
- Once you have the **access key**, scan the security events for any mention of these hidden **Administrator controller endpoints**. This will help you determine their exact paths.
- Once you have both the key and the endpoint, you should be able to authenticate successfully, granting you full CRUD access to the **Administrator endpoints**.

### Hints

- **Getting the Athena Access Key**: One of the **Athena Data Events** contains information about the administrator key. Analyze the **error message** within the event to get the **Admin Key** you need to authenticate.
  - Look for a device with **less than 15 nodes**, **less than 30 CPU cores**, and a memory capacity of **560GB of ROM** running **`Warcraft3.exe`**.

## Tertiary Objective: PWN

***Warning: This is completely experimental and may be extremely difficult. Proceed at your own risk!***

If you manage to achieve **full Admin capability**, you’ve unlocked a new potential—total control. The optional objective is to implement full CRUD capabilities for every resource available in the Pariah-Nexus API, including the **Administrator endpoints**.

- If a **POST request** is made to your API to create a new cluster, your API should also send a **POST** request to Pariah-Nexus to create it.
- The same should apply for **devices**, **processes**, **memory mappings**, and **AthenaDataEvents** for all **POST**, **PUT**, and **DELETE** operations.

### Hints

- **Full CRUD Implementation**: Once you've got admin capabilities, test your CRUD operations. For example, try creating a **cluster** through your API and ensure it reflects on the **Pariah-Nexus API**. This will confirm your total control over the environment.

## Architecture

The following is the architecture as we understand it, involving multiple integrated systems, including **Pariah-Nexus API**, **Athena Security System**, and your **web API**. This architecture will help you connect the dots for accessing and storing valuable data while maintaining an efficient and organized infrastructure.

![Architecture Diagram](./docs-assets/Cyberpunk%20(student)%20-%20Pariah%20Nexus%20API%20(1).png)